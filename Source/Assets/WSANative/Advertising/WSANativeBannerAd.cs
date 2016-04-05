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
    public static class WSANativeBannerAd
    {
        /// <summary>
        /// Raised when a new ad is received
        /// </summary>
        public static Action AdRefreshed
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user interacts with an ad
        /// </summary>
        public static Action IsEngagedChanged
        {
            get; set;
        }

        /// <summary>
        /// Raised when the ad encounters an error in operations
        /// </summary>
        public static Action ErrorOccurred
        {
            get; set;
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSABannerAdSettings> Create;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action Destroy;

        /// <summary>
        /// Create a banner ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your ad unit id</param>
        /// <param name="width">Width of the ad</param>
        /// <param name="height">Height of the ad</param>
        /// <param name="placement">Where should the ad be placed</param>
        public static void CreatAd(string appId, string adUnitId, int width, int height, WSAAdPlacement placement)
        {
            if (Create != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Create(new WSABannerAdSettings() { AppId = appId, AdUnitId = adUnitId, Width = width, Height = height, Placement = placement });
                }, false);
            }
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        public static void DestroyAd()
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                if (Destroy != null)
                {
                    Destroy();
                }
            }, false);
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        /// <param name="action"></param>
        public static void RaiseActionOnAppThread(Action action)
        {
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                if (action != null)
                {
                    action();
                }
            }, false);
        }
    }
}