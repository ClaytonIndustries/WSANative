
namespace CI.WSANative.Facebook.Core
{
    public static class WSAFacebookConstants
    {
        public const string ApiVersionNumber = "v15.0";

        public static string LoginApiUri { get { return string.Format("https://www.facebook.com/{0}/dialog/oauth", ApiVersionNumber); } }
        public static string GraphApiUri { get { return string.Format("https://graph.facebook.com/{0}/", ApiVersionNumber); } }
        public static string FeedDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/feed", ApiVersionNumber); } }
        public static string RequestDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/apprequests", ApiVersionNumber); } }
        public static string SendDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/send", ApiVersionNumber); } }

        public const string WebRedirectUri = "https://www.facebook.com/connect/login_success.html";
    }
}