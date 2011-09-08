using System;
using System.Collections.Generic;
using Nancy.Authentication.Facebook.FormsApplicationAuthentication;

namespace Nancy.Authentication.Facebook.Model
{
    public class FacebookUser:IFacebookUserIdentity
    {
        public Guid UserId { get; set;}
        public long FacebookUserId { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }

        public IEnumerable<string> Claims
        {
            get;
            set; 
        }
    }
}