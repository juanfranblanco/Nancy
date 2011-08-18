using System;
using Facebook;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Extensions;
using Nancy.Security;

namespace Nancy.Authentication.Facebook
{
    public class FacebookAuthentication
    {
        public static bool Enabled { get; private set; }

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

        public static FacebookAuthenticationConfiguration Configuration { get; private set; }

        public static FacebookOAuthClient GetFacebookOAuthClient()
        {
            var oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(Configuration.GetOathRedirectUrl());
            return oAuthClient;
        }

        //The base path is not part the context request.. so this needs to be configured :(.
        //This should go in a helper
        //What about 2 / in the url?
        private static string GetRequestUriAbsolutePath(NancyContext context)
        {
            var url = context.Request.Url;
            return Configuration.BasePath + "/" + url.Path + url.Query;
        }

        public static bool IsOAthResultSuccess(NancyContext context)
        {
            FacebookOAuthResult oauthResult;
            var stringUri = GetRequestUriAbsolutePath(context);

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

        public static void AddAuthenticatedUserToCache(string code, Guid userId)
        {
            string accessToken = GetAccessToken(code);
            var facebookClient = new FacebookClient(accessToken);
            dynamic me = facebookClient.Get("me?fields=id,name");
            long facebookId = Convert.ToInt64(me.id);
            Configuration.FacebookUserCache.AddUserToCache(userId, facebookId, accessToken, (string)me.name);
        }

        public static Response RedirectToFaceobookLoginUrl(NancyContext context)
        {
            return context.GetRedirect(GetFacebookOAuthClient().GetLoginUrl().AbsoluteUri);
        }


        public static Response CheckUserIsNothAuthorisedByFacebookAnymore(NancyContext context)
        {
            if (Enabled)
            {
                var facebookUserCache = Configuration.FacebookUserCache;

                long? facebookId = null;
                try
                {

                    if (AuthenticatedUserNameHasValue(context))
                    {
                        facebookId = long.Parse(context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString());
                        var client = new FacebookClient(facebookUserCache.GetAccessToken(facebookId.Value));
                        dynamic me = client.Get("me");
                    }
                }
                catch (FacebookOAuthException)
                {
                    //If an exception gets thrown the access token is no longer valid
                    RemoveUserFromCache(context, facebookId, facebookUserCache);
                    return context.GetRedirect(Configuration.LoginPath);
                }
            }
            return context.Response;

        }

        private static void RemoveUserFromCache(NancyContext context, long? facebookId, IFacebookUserCache facebookUserCache)
        {
            context.Items[SecurityConventions.AuthenticatedUsernameKey] = null;
            if (facebookId.HasValue) facebookUserCache.RemoveUserFromCache(facebookId.Value);
        }

        private static bool AuthenticatedUserNameHasValue(NancyContext context)
        {
            return context.Items.ContainsKey(SecurityConventions.AuthenticatedUsernameKey) && context.Items[SecurityConventions.AuthenticatedUsernameKey] != null && context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString() != String.Empty;
        }

    }
}