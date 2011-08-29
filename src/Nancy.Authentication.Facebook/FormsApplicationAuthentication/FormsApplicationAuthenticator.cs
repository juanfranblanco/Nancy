using System;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace Nancy.Authentication.Facebook.FormsApplicationAuthentication
{
    public abstract class FormsApplicationAuthenticator:IApplicationAuthenticator
    {
        public virtual Response UserLoggedInRedirectResponse(NancyContext context, long facebookId)
        {
            var uniqueIdentifier = GetUserUniqueIdentifier(facebookId);

            return FormsAuthentication.UserLoggedInRedirectResponse(context, uniqueIdentifier);
        }

        public virtual bool IsAuthenticatedIntoTheApplication(NancyContext context)
        {
            return AuthenticatedUserNameHasValue(context);
        }

        public virtual bool AuthenticatedUserNameHasValue(NancyContext context)
        {
            return context.Items.ContainsKey(SecurityConventions.AuthenticatedUsernameKey) && context.Items[SecurityConventions.AuthenticatedUsernameKey] != null && context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString() != String.Empty;
        }

        public virtual long? GetFacebookId(NancyContext context)
        {
            if (AuthenticatedUserNameHasValue(context))
            {
                return long.Parse(context.Items[SecurityConventions.AuthenticatedUsernameKey].ToString());
            }
            return null;
        }

        public virtual void SetAsNotAuthenticated(NancyContext context)
        {
            context.Items[SecurityConventions.AuthenticatedUsernameKey] = null;
        }

        public abstract Guid GetUserUniqueIdentifier(long facebookId);


    }
}