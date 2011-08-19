using System;
using Nancy.Authentication.Facebook;
using Nancy.Authentication.Forms;

namespace Nancy.Demo.Authentication.Facebook
{
    public class ApplicationAuthenticator:IApplicationAuthenticator
    {
        public Response UserLoggedInRedirectResponse(NancyContext context, long facebookId)
        {
            //Here we could check if there is a matching identifier for the facebookId in a store.
            //instead of just creating a temporary one
            var temporaryIdentifier = Guid.NewGuid();

            SetFacebookUserCachedApplicationUniqueIdentifier(facebookId, temporaryIdentifier);

            return FormsAuthentication.UserLoggedInRedirectResponse(context, temporaryIdentifier);
        }

        private void SetFacebookUserCachedApplicationUniqueIdentifier(long facebookId, Guid temporaryIdentifier)
        {
            var inMemoryUserCache = new InMemoryUserCache();
            var facebookUser = inMemoryUserCache.GetUser(facebookId);
            facebookUser.UserId = temporaryIdentifier;
        }
    }
}