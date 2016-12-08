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
    public static class WSANativeMediatorAd
    {
        /// <summary>
        /// Raised when a new ad is received
        /// </summary>
        public static Action AdRefreshed
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
        public static Action<WSAMediatorAdSettings> Create;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<bool> SetVisiblity;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action Destroy;

        /// <summary>
        /// Create a mediator ad (Microsoft and AdDuplex) - Use WSANativeBannerAd if you only require Microsoft
        /// </summary>
        /// <param name="wAppid">Your Windows app id (null or empty for Windows 8.1)</param>
        /// <param name="wAdUnitId">Your Windows ad unit id (null or empty for Windows 8.1)</param>
        /// <param name="adDuplexAppKey">Your AdDuplex app key (null or empty for Windows 8.1)</param>
        /// <param name="adDuplexAdUnitId">Your AdDuplex ad unit id (null or empty for Windows 8.1)</param>
        /// <param name="adDuplexWeight">Percentage chance of an AdDuplex ad being shown - e.g 50 for equal split with Windows (0 for Windows 8.1)</param>
        /// <param name="width">Width of the ad</param>
        /// <param name="height">Height of the ad</param>
        /// <param name="verticalPlacement">>Where should the ad be placed vertically</param>
        /// <param name="horizontalPlacement">Where should the ad be placed horizontally</param>
        public static void CreatAd(string wAppid, string wAdUnitId, string adDuplexAppKey, string adDuplexAdUnitId, int adDuplexWeight, int width, int height, 
            WSAAdVerticalPlacement verticalPlacement, WSAAdHorizontalPlacement horizontalPlacement)
        {
#if NETFX_CORE
            if (Create != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Create(new WSAMediatorAdSettings() { WAppId = wAppid, WAdUnitId = wAdUnitId, AdDuplexAppKey = adDuplexAppKey, AdDuplexAdUnitId = adDuplexAdUnitId, 
                        AdDuplexWeight = adDuplexWeight, Width = width, Height = height, VerticalPlacement = verticalPlacement, HorizontalPlacement = horizontalPlacement });
                }, false);
            }
#endif
        }

        /// <summary>
        /// Show or hide the medaitor ad
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
        /// Destroy the mediator ad
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