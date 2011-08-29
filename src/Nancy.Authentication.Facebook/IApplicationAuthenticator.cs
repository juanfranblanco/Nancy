namespace Nancy.Authentication.Facebook
{
    public interface IApplicationAuthenticator
    {
        /// <summary>
        /// The application authentication, an implementation could be FormsAuthentication login in and redirect.
        Response UserLoggedInRedirectResponse(NancyContext context, long facebookId);

        bool IsAuthenticatedIntoTheApplication(NancyContext context);

        long? GetFacebookId(NancyContext context);

        void SetAsNotAuthenticated(NancyContext context);
    }
}