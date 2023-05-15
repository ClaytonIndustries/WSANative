
namespace CI.WSANative.Facebook.Core
{
    public static class WSAFacebookConstants
    {
        public const string ApiVersionNumber = "v15.0";

        public static string LoginApiUri { get { return string.Format("https://www.facebook.com/{0}/dialog/oauth", ApiVersionNumber); } }
        public static string GraphApiUri { get { return string.Format("https://graph.facebook.com/{0}/", ApiVersionNumber); } }
        public static string ShareDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/share", ApiVersionNumber); } }
        public static string RequestDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/apprequests", ApiVersionNumber); } }
        public static string SendDialogUri { get { return string.Format("https://www.facebook.com/{0}/dialog/send", ApiVersionNumber); } }
    }
}