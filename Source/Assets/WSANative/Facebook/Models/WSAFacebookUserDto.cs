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
    public class WSAFacebookUserDto
    {
        public string id;
        public string name;
        public string first_name;
        public WSAFacebookPictureDto picture;
        public string email;
    }

    [Serializable]
    public class WSAFacebookPictureDto
    {
        public WSAFacebookPictureDataDto data;
    }

    [Serializable]
    public class WSAFacebookPictureDataDto
    {
        public string url;
        public bool is_silhouette;
        public int width;
        public int height;
    }
}