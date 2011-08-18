using Facebook;
using Nancy.Authentication.Facebook.Repository;
using Nancy.Authentication.Facebook.SecurityExtensions;
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
                               var facebookId = long.Parse(Context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString());
                               var user = new InMemoryUserCache().GetUser(facebookId);
                               var client = new FacebookClient(user.AccessToken);
                               dynamic me = client.Get("me");
                               return string.Format("<h1>Welcome {0} </h1><p><img src='https://graph.facebook.com/{1}/picture'/>You have logged in using facebook</p>", me.name, me.username);
                           };

        }  
    }
}