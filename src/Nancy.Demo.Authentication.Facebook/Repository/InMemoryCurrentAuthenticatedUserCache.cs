using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Authentication.Facebook.Model;
using Nancy.Authentication.Facebook.Modules;
using Nancy.Authentication.Facebook.Repository;

namespace Nancy.Demo.Authentication.Facebook
{
    public class InMemoryCurrentAuthenticatedUserCache : IFacebookCurrentAuthenticatedUserCache
    {
        static readonly IDictionary<long, FacebookUser> users = new ConcurrentDictionary<long, FacebookUser>();

        public void AddUserToCache(FacebookUser user)
        {
            users[user.FacebookId] = user;
        }

        public void AddUserToCache(long facebookId, string accessToken, string name)
        {
            AddUserToCache(new FacebookUser
            {
                AccessToken = accessToken,
                FacebookId = facebookId,
                Name = name
            });
        }

        public long? GetFacebookId(Guid userId)
        {
            var usersFound = users.Where(x => x.Value.UserId == userId);
            if (usersFound.Count() > 0)
            {
                return usersFound.First().Value.FacebookId;
            }
            return null;
        }

        public string GetAccessToken(long facebookId)
        {
            return GetUser(facebookId).AccessToken;
        }

        public FacebookUser GetUser(long facebookId)
        {
            return users[facebookId];
        }

        public void RemoveUserFromCache(long facebookId)
        {
            users.Remove(facebookId);
        }
    }
}