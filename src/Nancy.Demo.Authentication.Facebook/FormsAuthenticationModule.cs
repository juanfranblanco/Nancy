using System;
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
        }

    }
}