using Nancy.Security;

namespace Nancy.Authentication.Facebook.FormsApplicationAuthentication
{
    public interface IFacebookUserIdentity:IUserIdentity
    {
        long FacebookUserId { get; }
        string AccessToken { get; }
    }
}