
namespace CI.WSANative.Facebook.Core
{
    public static class WSAFacebookConstants
    {
        public const string ApiVersionNumber = "v2.7";

        public static string GraphApiUri { get { return string.Format("https://graph.facebook.com/{0}/", ApiVersionNumber); } }
        public static string FeedDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/feed", ApiVersionNumber); } }
        public static string RequestDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/apprequests", ApiVersionNumber); } }
        public static string SendDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/send", ApiVersionNumber); } }

        public const string WebRedirectUri = "http://www.facebook.com/connect/login_success.html";
        public const string FeedDialogResponseUri = "/dialog/return/close";
        public const string RequestDialogResponseUri = "/connect/login_success.html";
        public const string SendDialogResponseUri = "/connect/login_success.html";
        public const string LoginDialogResponseUri = "/connect/login_success.html";
    }
}