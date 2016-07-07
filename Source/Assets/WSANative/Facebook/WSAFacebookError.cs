
namespace CI.WSANative.Facebook
{
    public class WSAFacebookError
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public bool AccessTokenExpired { get; set; }
    }
}