////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Twitter
{
    public class WSATwitterLoginResult
    {
        public bool Success { get; set; }
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }
    }
}