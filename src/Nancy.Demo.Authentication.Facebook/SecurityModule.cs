using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Authentication.Facebook.Modules;

namespace Nancy.Demo.Authentication.Facebook
{
    public class SecurityModule:FacebookSecurityModule
    {
        public override string GetBasePath()
        {
           return "http://localhost:81";
        }
    }
}