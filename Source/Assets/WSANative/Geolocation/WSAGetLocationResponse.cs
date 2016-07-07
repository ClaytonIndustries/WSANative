////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Geolocation
{
    public class WSAGetLocationResponse
    {
        /// <summary>
        /// Was the users location found (if this is false check AccessStatus to determine if location services are disabled)
        /// </summary>
        public bool Success
        {
            get; set;
        }

        /// <summary>
        /// Has permission to access location services been granted
        /// </summary>
        public WSAGeolocationAccessStatus AccessStatus
        {
            get; set;
        }

        /// <summary>
        /// The position containing lat / long
        /// </summary>
        public WSAGeoPosition GeoPosition
        {
            get; set;
        }
    }
}