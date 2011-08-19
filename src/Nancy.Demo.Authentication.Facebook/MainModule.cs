﻿using Facebook;
using Nancy.Authentication.Facebook;
using Nancy.Authentication.Facebook.FacebookExtensions;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Security;

namespace Nancy.Demo.Authentication.Facebook
{
    public class MainModule:NancyModule
    {
        public MainModule()
        {
            this.RequiresAuthentication();
            this.RequiresFacebookLoggedIn();

            Get["/"] = parameters =>
                           {

                               var facebookId = FacebookAuthentication.GetFacebookIdFromContext(Context);
                               var user = new InMemoryUserCache().GetUser(facebookId.Value);
                               var client = new FacebookClient(user.AccessToken);
                               dynamic me = client.Get("me");
                               return string.Format("<h1>Welcome {0} </h1><p><img src='https://graph.facebook.com/{1}/picture'/>You have logged in using facebook</p>", me.name, me.username);
                           };

        }  
    }
}