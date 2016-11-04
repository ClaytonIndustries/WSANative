////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if (NETFX_CORE && UNITY_WSA_10_0)
using Windows.Devices.Geolocation;
#endif

using System;

namespace CI.WSANative.Geolocation
{
    /// <summary>
    /// Windows 10 universal only
    /// </summary>
    public static class WSANativeGeolocation
    {
        /// <summary>
        /// Attempts to get the users current position (if this is the first time the users permission will be sought)
        /// </summary>
        /// <param name="desiredAccuracyInMeters">Desired accuracy in meters</param>
        /// <param name="response">A response indicating whether the action was successful and if so containing the users position</param>
        public static void GetUsersLocation(int desiredAccuracyInMeters, Action<WSAGetLocationResponse> response)
        {
#if (NETFX_CORE && UNITY_WSA_10_0)
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();

                UnityEngine.WSA.Application.InvokeOnAppThread(async () =>
                {
                    switch (accessStatus)
                    {
                        case GeolocationAccessStatus.Allowed:
                            Geolocator geolocator = new Geolocator() { DesiredAccuracyInMeters = (uint)desiredAccuracyInMeters };
                            try
                            {
                                Geoposition position = await geolocator.GetGeopositionAsync();
                                RaiseCallback(true, WSAGeolocationAccessStatus.Allowed, 
                                    new WSAGeoPosition() { Latitude = position.Coordinate.Point.Position.Latitude, Longitude = position.Coordinate.Point.Position.Longitude }, response);
                            }
                            catch
                            {
                                RaiseCallback(false, WSAGeolocationAccessStatus.Allowed, null, response);
                            }
                            break;
                        case GeolocationAccessStatus.Denied:
                            RaiseCallback(false, WSAGeolocationAccessStatus.Denied, null, response);
                            break;
                        default:
                            RaiseCallback(false, WSAGeolocationAccessStatus.Unspecified, null, response);
                            break;
                    }
                }, false);
            }, false);
#endif
        }

#if (NETFX_CORE && UNITY_WSA_10_0)
        private static void RaiseCallback(bool success, WSAGeolocationAccessStatus accessStatus, WSAGeoPosition geoPosition, Action<WSAGetLocationResponse> response)
        {
            if (response != null)
            {
                response(new WSAGetLocationResponse() { Success = success, AccessStatus = accessStatus, GeoPosition = geoPosition });
            }
        }
#endif
    }
}