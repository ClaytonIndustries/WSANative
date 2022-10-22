using System;

namespace CI.WSANative.Facebook.Models
{
    public class WSAFacebookLoginResult
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
    }
}