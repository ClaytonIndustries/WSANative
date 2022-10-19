using System;

namespace CI.WSANative.Facebook.Core
{
    public class LoginResult
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiry { get; set; }
    }
}