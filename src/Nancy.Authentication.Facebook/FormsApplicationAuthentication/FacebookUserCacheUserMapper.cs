using System;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace Nancy.Authentication.Facebook.FormsApplicationAuthentication
{
    public class FacebookUserCacheUserMapper:IUserMapper
    {
        private IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache;

        public FacebookUserCacheUserMapper(IFacebookCurrentAuthenticatedUserCache facebookCurrentAuthenticatedUserCache)
        {
            if (facebookCurrentAuthenticatedUserCache == null) throw new ArgumentNullException("facebookCurrentAuthenticatedUserCache");
            this.facebookCurrentAuthenticatedUserCache = facebookCurrentAuthenticatedUserCache;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier)
        {
            return facebookCurrentAuthenticatedUserCache.GetUser(identifier);
        }
    }
}