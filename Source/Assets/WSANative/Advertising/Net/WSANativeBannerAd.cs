////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_DOTNET
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
        public static Action<bool> SetVisiblity;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action Destroy;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Func<bool> IsShowingAd;

        /// <summary>
        /// Create a banner ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your ad unit id</param>
        /// <param name="width">Width of the ad</param>
        /// <param name="height">Height of the ad</param>
        /// <param name="verticalPlacement">Where should the ad be placed vertically</param>
        /// <param name="horizontalPlacement">Where should the ad be placed horizontally</param>
        public static void CreatAd(string appId, string adUnitId, int width, int height, WSAAdVerticalPlacement verticalPlacement, WSAAdHorizontalPlacement horizontalPlacement)
        {
#if NETFX_CORE
            if (Create != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Create(new WSABannerAdSettings() { AppId = appId, AdUnitId = adUnitId, Width = width, Height = height, VerticalPlacement = verticalPlacement, HorizontalPlacement = horizontalPlacement });
                }, false);
            }
#endif
        }

        /// <summary>
        /// Show or hide the banner ad
        /// </summary>
        /// <param name="visible">Should the ad be visible</param>
        public static void SetAdVisibility(bool visible)
        {
#if NETFX_CORE
            if (SetVisiblity != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    SetVisiblity(visible);
                }, false);
            }
#endif
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        public static void DestroyAd()
        {
#if NETFX_CORE
            if (Destroy != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Destroy();
                }, false);
            }
#endif
        }

        /// <summary>
        /// Is an ad currently being displayed
        /// </summary>
        /// <returns>Value indicating whether an ad is being shown</returns>
        public static bool HasAd()
        {
            if (IsShowingAd != null)
            {
                return IsShowingAd();
            }

            return false;
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        /// <param name="action"></param>
        public static void RaiseActionOnAppThread(Action action)
        {
#if NETFX_CORE
            if (action != null)
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    action();
                }, false);
            }
#endif
        }
    }
}
#endif