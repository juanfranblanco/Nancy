using System;
using Nancy.Authentication.Facebook.FormsApplicationAuthentication;

namespace Nancy.Demo.Authentication.Facebook
{
    public class ApplicationAuthenticator : FormsApplicationAuthenticator
    {
        public override Guid GetUserUniqueIdentifier(long facebookId)
        {
            //Here we could check if there is a matching identifier for the facebookId in a store.
            //instead of just creating a temporary one
            var temporaryIdentifier = Guid.NewGuid();
            SetFacebookUserCachedApplicationUniqueIdentifier(facebookId, temporaryIdentifier);
            return temporaryIdentifier;
        }

        private void SetFacebookUserCachedApplicationUniqueIdentifier(long facebookId, Guid temporaryIdentifier)
        {
            var inMemoryUserCache = new InMemoryCurrentAuthenticatedUserCache();
            var facebookUser = inMemoryUserCache.GetUser(facebookId);
            facebookUser.UserId = temporaryIdentifier;
        }
    }
}