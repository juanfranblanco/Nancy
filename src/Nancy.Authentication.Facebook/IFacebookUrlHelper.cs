namespace Nancy.Authentication.Facebook
{
    public interface IFacebookUrlHelper
    {
        /// <summary>
        /// Returns the Oath absolute url
        /// </summary>
        string GetOathAbsoluteUrl();

        /// <summary>
        /// Returns the request absolute url using the configured base path
        /// </summary>
        string GetRequestAbsoluteUrl(NancyContext context);

        string ExpandPath(string path);
    }
}