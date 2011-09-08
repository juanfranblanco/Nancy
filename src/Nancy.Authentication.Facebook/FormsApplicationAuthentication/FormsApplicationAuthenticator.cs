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
            return context.CurrentUser != null && !String.IsNullOrWhiteSpace(context.CurrentUser.UserName);
        }

        public virtual long? GetFacebookId(NancyContext context)
        {
            if (IsAuthenticatedIntoTheApplication(context))
            {
                if (context.CurrentUser is IFacebookUserIdentity)
                {
                    return ((IFacebookUserIdentity) context.CurrentUser).FacebookUserId;
                }
            }
            return null;
        }

        public virtual void SetAsNotAuthenticated(NancyContext context)
        {
            context.CurrentUser = null;
        }

        public abstract Guid GetUserUniqueIdentifier(long facebookId);

    }
}