
namespace CI.WSANative.Facebook
{
    public static class WSAFacebookConstants
    {
        public const string ApiVersionNumber = "v2.7";

        public static string GraphApiUri { get; }
        public static string FeedApiUri { get; }
        public static string WebRedirectUri { get; }

        static WSAFacebookConstants()
        {
            GraphApiUri = string.Format("https://graph.facebook.com/{0}/", ApiVersionNumber);
            FeedApiUri = string.Format("https://www.facebook.com/{0}/dialog/feed", ApiVersionNumber);
            WebRedirectUri = "http://www.facebook.com/login_success.html";
        }
    }
}