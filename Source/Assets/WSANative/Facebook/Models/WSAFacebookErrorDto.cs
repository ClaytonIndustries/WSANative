////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////


using System;

namespace CI.WSANative.Facebook.Models
{
    [Serializable]
    public class WSAFacebookErrorDto
    {
        public WSAFacebookErrorDataDto error;
    }

    [Serializable]
    public class WSAFacebookErrorDataDto
    {
        public string message;
        public string type;
        public string code;
    }
}