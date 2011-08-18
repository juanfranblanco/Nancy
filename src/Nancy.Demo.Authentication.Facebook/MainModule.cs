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
                               return "<h1>Welcome " + me.name + "</h1><p>You have logged in using facebook</p>";
                           };

        }  
    }
}