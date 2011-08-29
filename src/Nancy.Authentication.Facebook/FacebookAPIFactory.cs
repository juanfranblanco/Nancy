using Facebook;

namespace Nancy.Authentication.Facebook
{
    public class FacebookAPIFactory : IFacebookAPIFactory
    {
        /// <summary>
        /// Creates a new Facebook Client using the user accessCode
        /// </summary>
        /// <param name="accessToken">The user access token</param>
        public FacebookClient CreateNewFacebookClient(string accessToken)
        {
            return new FacebookClient(accessToken);
        }

        /// <summary>
        /// Creates a new Facebook OAuth Client
        /// </summary>
        public FacebookOAuthClient CreateNewFacebookOAuthClient()
        {
            return new FacebookOAuthClient(FacebookApplication.Current);
        }
    }
}