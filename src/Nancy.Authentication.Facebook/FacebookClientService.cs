using System;
using Facebook;
using Nancy.Authentication.Facebook.Repository;

namespace Nancy.Authentication.Facebook
{
    public class FacebookClientService
    {
        private readonly IApplicationAuthenticator applicationAuthenticator;
        private readonly IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache;
        private readonly IFacebookSDKObjectBuilder facebookSDKObjectBuilder;

        public FacebookClientService(IApplicationAuthenticator applicationAuthenticator, IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache, IFacebookSDKObjectBuilder facebookSDKObjectBuilder)
        {
            if (applicationAuthenticator == null) throw new ArgumentNullException("applicationAuthenticator");
            if (facebookCurrentAuthenticatedUserCache == null) throw new ArgumentNullException("facebookCurrentAuthenticatedUserCache");
            if (facebookSDKObjectBuilder == null) throw new ArgumentNullException("facebookSDKObjectBuilder");

            this.applicationAuthenticator = applicationAuthenticator;
            this.facebookCurrentAuthenticatedUserCache = facebookCurrentAuthenticatedUserCache;
            this.facebookSDKObjectBuilder = facebookSDKObjectBuilder;
        }

        public  FacebookClient GetFacebookClient(NancyContext context)
        {
            var facebookId = applicationAuthenticator.GetFacebookId(context);
            return GetFacebookClient(facebookId);
        }

        public FacebookClient GetFacebookClient(long? facebookId)
        {
            if (facebookId.HasValue)
            {
                var accessToken = facebookCurrentAuthenticatedUserCache.GetAccessToken(facebookId.Value);
                return facebookSDKObjectBuilder.CreateNewFacebookClient(accessToken);
            }

            return null;
        }

        public dynamic GetFacebookMe(string accessToken)
        {
            var facebookClient = facebookSDKObjectBuilder.CreateNewFacebookClient(accessToken);
            return facebookClient.Get("me");
        }

        public dynamic GetFacebookMe(NancyContext context)
        {
            var client = GetFacebookClient(context);
            return client != null ? client.Get("me") : null;
        }
    }
}