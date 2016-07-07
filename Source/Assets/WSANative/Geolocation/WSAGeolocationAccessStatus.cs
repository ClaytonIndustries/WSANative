////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Geolocation
{
    public enum WSAGeolocationAccessStatus
    {
        /// <summary>
        /// Permission to access location was not specified
        /// </summary>
        Unspecified,
        /// <summary>
        /// Permission to access location was granted
        /// </summary>
        Allowed,
        /// <summary>
        /// Permission to access location was denied
        /// </summary>
        Denied
    }
}