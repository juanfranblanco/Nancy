namespace Nancy.Authentication.Facebook
{
    public interface IApplicationAuthenticator
    {
        /// <summary>
        /// The application authentication, an implementation could be FormsAuthentication login in and redirect.
        Response UserLoggedInRedirectResponse(NancyContext context, long facebookId);
    }
}