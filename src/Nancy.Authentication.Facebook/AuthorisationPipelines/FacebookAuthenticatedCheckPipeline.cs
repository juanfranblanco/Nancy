using System;
using Facebook;
using Nancy.Authentication.Facebook.Modules;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Security;

namespace Nancy.Authentication.Facebook.AuthorisationPipelines
{
    public static class FacebookAuthenticatedCheckPipeline
    {
        public static Response CheckUserIsNothAuthorisedByFacebookAnymore(NancyContext context)
        {
            if (FacebookSecurityModule.Enabled)
            {
                var facebookUserCache = FacebookSecurityModule.Configuration.FacebookUserCache;

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
                    return new Response() {StatusCode = HttpStatusCode.Unauthorized};
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