////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_IL2CPP
using System;
using System.Runtime.InteropServices;
using AOT;

namespace CI.WSANative.Advertising
{
    public static class WSANativeBannerAd
    {
        private delegate void AdRefreshedCallbackDelegate();
        private delegate void ErrorOccurredCallbackDelegate();

        [DllImport("__Internal")]
        private static extern void _BannerAdCreate([MarshalAs(UnmanagedType.LPWStr)]string appId, [MarshalAs(UnmanagedType.LPWStr)]string adUnitId,
            int width, int height, [MarshalAs(UnmanagedType.LPWStr)]string verticalPlacement, [MarshalAs(UnmanagedType.LPWStr)]string horizontalPlacement,
            AdRefreshedCallbackDelegate adRefreshedCallback, ErrorOccurredCallbackDelegate errorOccurredCallback);

        [DllImport("__Internal")]
        private static extern void _BannerAdSetVisibility(bool visible);

        [DllImport("__Internal")]
        private static extern void _BannerAdDestroy();

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
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _BannerAdCreate(appId, adUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString(), AdRefreshedCallback, ErrorOccurredCallback);
            }, false);
        }

        /// <summary>
        /// Show or hide the banner ad
        /// </summary>
        /// <param name="visible">Should the ad be visible</param>
        public static void SetAdVisibility(bool visible)
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _BannerAdSetVisibility(visible);
            }, false);
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        public static void DestroyAd()
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _BannerAdDestroy();
            }, false);
        }


        [MonoPInvokeCallback(typeof(AdRefreshedCallbackDelegate))]
        private static void AdRefreshedCallback()
        {
            if (AdRefreshed != null)
            {
                AdRefreshed();
            }
        }

        [MonoPInvokeCallback(typeof(ErrorOccurredCallbackDelegate))]
        private static void ErrorOccurredCallback()
        {
            if(ErrorOccurred != null)
            {
                ErrorOccurred();
            }
        }
    }
}
#endif