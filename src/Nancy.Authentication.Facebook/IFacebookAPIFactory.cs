using Facebook;

namespace Nancy.Authentication.Facebook
{
    public interface IFacebookSDKObjectBuilder
    {
        /// <summary>
        /// Creates a new Facebook Client using the user accessCode
        /// </summary>
        /// <param name="accessCode">The user access code</param>
        FacebookClient CreateNewFacebookClient(string accessCode);

        /// <summary>
        /// Creates a new Facebook OAuth Client
        /// </summary>
        FacebookOAuthClient CreateNewFacebookOAuthClient();


       
    }
}