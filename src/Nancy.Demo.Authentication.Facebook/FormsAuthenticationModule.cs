using System;
using Nancy.Authentication.Facebook.Helpers;
using Nancy.Authentication.Forms;

namespace Nancy.Demo.Authentication.Facebook
{
    public class FormsAuthenticationModule:NancyModule
    {
        public FormsAuthenticationModule()
        {
            Get["/logoutApp"] = x =>
                                    {
                                        return this.LogoutAndRedirect("~/");
                                    };

            Get["/loginAppAfterFacebookOAth/{facebookTemporaryUserId}"] = x =>
                                 {
                                       var userIdentifier = GuidEncoder.Decode((string)x.facebookTemporaryUserId);
                                       //Here we can retrieve the facebook Id using the user Identifier
                                       //then use the facebookId to get the real guid from the database or store or
                                       //any other strategy that we may have to store users.
                                       //if for example we just use the facebook id as the key we can just leave the temporary guid,
                                       return this.LoginAndRedirect((Guid)userIdentifier);
                                  };

        }

    }
}