using System;
using System.Collections.Generic;
using Facebook;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Extensions;
using Nancy.Security;

namespace Nancy.Authentication.Facebook
{
    public class FacebookAuthentication
    {
        public static bool Enabled { get; private set; }

        public static FacebookAuthenticationConfiguration Configuration { get; private set; }

        public static IFacebookUserCache FacebookUserCache
        {
            get { return Configuration != null ? Configuration.FacebookUserCache : null; }
        }

        public static void Enable(FacebookAuthenticationConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (!configuration.IsValid)
            {

                throw new ArgumentException("Configuration is invalid", "configuration");
            }

            Enabled = true;
            Configuration = configuration;
        }

        public static FacebookOAuthClient GetFacebookOAuthClient()
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(Configuration.GetOathAbsoluteUrl());
            return oAuthClient;
        }

        /// <summary>
        /// Uses the configured application authenticator to login and redirect
        /// </summary>
        public static Response LoginAndRedirect(NancyContext context, long facebookId)
        {
            return Configuration.ApplicationAuthenticator.UserLoggedInRedirectResponse(context, facebookId);
        }

        public static Response LogoutAndRedirect(NancyContext context, string path)
        {
            var facebookId = GetFacebookIdFromContext(context);

            if (facebookId.HasValue)
            {
                var accessToken = FacebookUserCache.GetAccessToken(facebookId.Value);
                //This method of login out comes from http://forum.developers.facebook.net/viewtopic.php?id=87109 it seems to be an issue with login out.
                return context.GetRedirect(Configuration.GetFacebookLogoutUrl(path, accessToken));
            }

            return null;
        }

        public static long? GetFacebookIdFromContext(NancyContext context)
        {
            if (AuthenticatedUserNameHasValue(context))
            {
                return long.Parse(context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString());
            }
            return null;
        }

        public static bool IsOAthResultSuccess(NancyContext context)
        {
            FacebookOAuthResult oauthResult;
            var stringUri = Configuration.GetRequestAbsoluteUrl(context);
            if (FacebookOAuthResult.TryParse(stringUri, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetAccessToken(string code)
        {
            var oAuthClient = GetFacebookOAuthClient();
            dynamic tokenResult = oAuthClient.ExchangeCodeForAccessToken(code);
            return tokenResult.access_token;
        }

        //hmmm 2 functions? return the facebookId and add to cache..
        /// <summary>
        /// Retrieves and adds an authenticated user to the cache, using the authorisation code provided by facebook
        /// </summary>
        /// <param name="code">Authorisation code provided by facebook</param>
        /// <returns>The facebook id of the user</returns>
        public static long RetrieveAndAddAuthenticatedUserToCache(string code)
        {
            string accessToken = GetAccessToken(code);
            var facebookClient = new FacebookClient(accessToken);
            dynamic me = facebookClient.Get("me");
            long facebookId = Convert.ToInt64(me.id);
            FacebookUserCache.AddUserToCache(facebookId, accessToken, (string)me.name);
            return facebookId;
        }

        public static Response RedirectToFacebookLoginUrl(NancyContext context)
        {
            //All the login parameters should be configurable
            var loginParameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(Configuration.ExtendedPermissions))
            {
                loginParameters["scope"] = Configuration.ExtendedPermissions;
            }

            return context.GetRedirect(GetFacebookOAuthClient().GetLoginUrl(loginParameters).AbsoluteUri);
        }

        public static Response CheckUserIsNothAuthorisedByFacebookAnymore(NancyContext context)
        {
            if (Enabled)
            {
                var facebookId = GetFacebookIdFromContext(context);
               
                try
                {
                    if (facebookId != null)
                    {
                        var client = GetFacebookClient(context);
                        dynamic me = client.Get("me");
                    }

                }
                catch (FacebookOAuthException)
                {
                    //If an exception gets thrown the access token is no longer valid
                    RemoveUserFromCache(context, facebookId);
                    return context.GetRedirect(Configuration.LoginPath);
                }
            }
            return context.Response;

        }

        private static void RemoveUserFromCache(NancyContext context, long? facebookId)
        {
            context.Items[SecurityConventions.AuthenticatedUsernameKey] = null;
            if (facebookId.HasValue) FacebookUserCache.RemoveUserFromCache(facebookId.Value);
        }

        private static bool AuthenticatedUserNameHasValue(NancyContext context)
        {
            return context.Items.ContainsKey(SecurityConventions.AuthenticatedUsernameKey) && context.Items[SecurityConventions.AuthenticatedUsernameKey] != null && context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString() != String.Empty;
        }

        public static FacebookClient GetFacebookClient(NancyContext context)
        {
            var facebookId = GetFacebookIdFromContext(context);
            return GetFacebookClient(facebookId);
        }

        public static FacebookClient GetFacebookClient(long? facebookId)
        {
            if (facebookId.HasValue)
            {
                return new FacebookClient(FacebookUserCache.GetAccessToken(facebookId.Value));
            }

            return null;
        }
    }
}