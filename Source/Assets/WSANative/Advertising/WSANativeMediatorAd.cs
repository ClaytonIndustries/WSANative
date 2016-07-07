﻿////////////////////////////////////////////////////////////////////////////////
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
        /// Raised when the ad encounters an error in operations
        /// </summary>
        public static Action AdSdkError
        {
            get; set;
        }

        /// <summary>
        /// Raised when a new ad is received
        /// </summary>
        public static Action AdMediatorFilled
        {
            get; set;
        }

        /// <summary>
        /// Raised when the ad mediator control encounters an error
        /// </summary>
        public static Action AdMediatorError
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user interacts with an ad
        /// </summary>
        public static Action AdSdkEvent
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
        public static Action Destroy;

        /// <summary>
        /// Create a banner ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your ad unit id</param>
        /// <param name="width">Width of the ad</param>
        /// <param name="height">Height of the ad</param>
        /// <param name="placement">Where should the ad be placed</param>
        public static void CreatAd(int width, int height, WSAAdPlacement placement)
        {
#if NETFX_CORE
            if (Create != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Create(new WSAMediatorAdSettings() { Width = width, Height = height, Placement = placement });
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