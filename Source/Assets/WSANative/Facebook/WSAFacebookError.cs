////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

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