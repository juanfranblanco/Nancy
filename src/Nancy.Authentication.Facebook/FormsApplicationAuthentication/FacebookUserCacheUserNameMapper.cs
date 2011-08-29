using System;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Authentication.Forms;

namespace Nancy.Authentication.Facebook.FormsApplicationAuthentication
{

    public class FacebookUserCacheUserNameMapper:IUsernameMapper
    {
        private IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache;

        public FacebookUserCacheUserNameMapper(IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache)
        {
            if (facebookCurrentAuthenticatedUserCache == null) throw new ArgumentNullException("facebookCurrentAuthenticatedUserCache");
            this.facebookCurrentAuthenticatedUserCache = facebookCurrentAuthenticatedUserCache;
        }

        public string GetUsernameFromIdentifier(Guid identifier)
        {
            var facebookId = facebookCurrentAuthenticatedUserCache.GetFacebookId(identifier);
            return facebookId == null ? null : facebookId.ToString();
            //returning null will trigger a non authenticated user
        }
    }
}