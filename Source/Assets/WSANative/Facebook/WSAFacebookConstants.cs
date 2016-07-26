
namespace CI.WSANative.Facebook.Core
{
    public static class WSAFacebookConstants
    {
        public const string ApiVersionNumber = "v2.7";

        public static string GraphApiUri { get { return string.Format("https://graph.facebook.com/{0}/", ApiVersionNumber); } }
        public static string FeedApiUri { get { return string.Format("https://www.facebook.com/{0}/dialog/feed", ApiVersionNumber); } }
        public static string WebRedirectUri { get { return "http://www.facebook.com/connect/login_success.html"; } }
    }
}