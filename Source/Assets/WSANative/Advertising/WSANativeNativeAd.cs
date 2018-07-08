////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.Advertising
{
    public static class WSANativeNativeAd
    {
        /// <summary>
        /// Raised when a new ad is received
        /// </summary>
        public static Action<WSANativeAd> AdReady { get; set; }

        /// <summary>
        /// Raised when an error occurs
        /// </summary>
        public static Action<string> ErrorOccurred { get; set; }

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<string, string> _Request;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<int, int, int, int> _Position;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action _Destroy;

#pragma warning disable 0414
        private static string _appId = string.Empty;
        private static string _adUnitId = string.Empty;
#pragma warning restore 0414

        /// <summary>
        /// Initialise the native ad - this only needs to be called once
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your apps ad unit id</param>
        public static void Initialise(string appId, string adUnitId)
        {
            _appId = appId;
            _adUnitId = adUnitId;
        }

        /// <summary>
        /// Request a new ad - AdReady will be raised when the request completes successfully
        /// </summary>
        public static void RequestAd()
        {
#if NETFX_CORE
            if (_Request != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _Request(_appId, _adUnitId);
                }, false);
            }
#endif
        }

        /// <summary>
        /// Sets the position of the ad container - only call after an ad is available
        /// </summary>
        /// <param name="x">X position of the container</param>
        /// <param name="y">Y position of the container</param>
        /// <param name="width">Width of the container</param>
        /// <param name="height">Height of the container</param>
        public static void SetPosition(int x, int y, int width, int height)
        {
#if NETFX_CORE
            if (_Position != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _Position(x, y, width, height);
                }, false);
            }
#endif
        }

        /// <summary>
        /// Removes the ads container from the screen
        /// </summary>
        public static void DestroyAd()
        {
#if NETFX_CORE
            if (_Destroy != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _Destroy();
                }, false);
            }
#endif
        }
    }
}