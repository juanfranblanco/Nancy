using System;
using Facebook;
using Nancy.Authentication.Facebook.Repository;

namespace Nancy.Authentication.Facebook
{
    public class FacebookClientService
    {
        private readonly IApplicationAuthenticator applicationAuthenticator;
        private readonly IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache;
        private readonly IFacebookAPIFactory facebookAPIFactory;

        public FacebookClientService(IApplicationAuthenticator applicationAuthenticator, IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache, IFacebookAPIFactory facebookAPIFactory)
        {
            if (applicationAuthenticator == null) throw new ArgumentNullException("applicationAuthenticator");
            if (facebookCurrentAuthenticatedUserCache == null) throw new ArgumentNullException("facebookCurrentAuthenticatedUserCache");
            if (facebookAPIFactory == null) throw new ArgumentNullException("facebookAPIFactory");

            this.applicationAuthenticator = applicationAuthenticator;
            this.facebookCurrentAuthenticatedUserCache = facebookCurrentAuthenticatedUserCache;
            this.facebookAPIFactory = facebookAPIFactory;
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
                return facebookAPIFactory.CreateNewFacebookClient(accessToken);
            }

            return null;
        }

        public dynamic GetFacebookMe(string accessToken)
        {
            var facebookClient = facebookAPIFactory.CreateNewFacebookClient(accessToken);
            return facebookClient.Get("me");
        }

        public dynamic GetFacebookMe(NancyContext context)
        {
            var client = GetFacebookClient(context);
            return client != null ? client.Get("me") : null;
        }
    }
}