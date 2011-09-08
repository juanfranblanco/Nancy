using System;
using Nancy.Authentication.Facebook.FormsApplicationAuthentication;
using Nancy.Security;

namespace Nancy.Authentication.Facebook.Repository
{
    public interface IFacebookCurrentAuthenticatedUserCache
    {
        void AddUserToCache(long facebookId, string accessToken, string name);
        long? GetFacebookId(Guid userId);
        string GetAccessToken(long facebookId);
        void RemoveUserFromCache(long facebookId);
        IFacebookUserIdentity GetUser(Guid identifier);
    }
}