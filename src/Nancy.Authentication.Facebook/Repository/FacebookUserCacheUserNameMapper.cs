using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Nancy.Authentication.Forms;

namespace Nancy.Authentication.Facebook.Repository
{
    public class FacebookUserCacheUserNameMapper:IUsernameMapper
    {
        private IFacebookUserCache facebookUserCache;

        public FacebookUserCacheUserNameMapper(IFacebookUserCache facebookUserCache)
        {
            if (facebookUserCache == null) throw new ArgumentNullException("facebookUserCache");
            this.facebookUserCache = facebookUserCache;
        }

        public string GetUsernameFromIdentifier(Guid identifier)
        {
            var facebookId = facebookUserCache.GetFacebookId(identifier);
            return facebookId == null ? null : facebookId.ToString();
            //returning null will trigger a non authenticated user
        }
    }
}