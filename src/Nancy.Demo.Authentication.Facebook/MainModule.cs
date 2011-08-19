using System.Collections.Generic;
using System.Dynamic;
using Facebook;
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
                               var client = FacebookAuthentication.GetFacebookClient(Context);
                               dynamic me = client.Get("me");
                               return View["index", me];
                           };

            Post["/"] = x =>
                            {
                                var client = FacebookAuthentication.GetFacebookClient(Context);
                                dynamic parameters = new ExpandoObject();
                                parameters.message = (string)this.Request.Form.Message;
                                client.Post("me/feed", parameters);
                                return Response.AsRedirect("/");
                            };

        } 
    }
}