using System;

namespace Nancy.Authentication.Facebook.Repository
{
    public interface IFacebookUserCache
    {
        void AddUserToCache(Guid userId, long facebookId, string accessToken, string name);
        long? GetFacebookId(Guid userId);
        string GetAccessToken(long facebookId);
        void RemoveUserFromCache(long facebookId);
    }
}