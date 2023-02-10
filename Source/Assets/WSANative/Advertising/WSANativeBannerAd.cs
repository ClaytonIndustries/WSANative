////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using CI.WSANative.Common;

#if ENABLE_WINMD_SUPPORT
using System.Runtime.InteropServices;
using AOT;
#endif

namespace CI.WSANative.Advertising
{
    public static class WSANativeBannerAd
    {
#if ENABLE_WINMD_SUPPORT
        private delegate void AdRefreshedCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId);
        private delegate void ErrorOccurredCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string errorMessage);

        [DllImport("__Internal")]
        private static extern void _BannerAdInitialise(AdRefreshedCallbackDelegate adRefreshedCallback, ErrorOccurredCallbackDelegate errorOccurredCallback);

        [DllImport("__Internal")]
        private static extern void _BannerAdCreate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string appId, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId, 
            int width, int height, [MarshalAs(UnmanagedType.LPWStr)]string verticalPlacement, [MarshalAs(UnmanagedType.LPWStr)]string horizontalPlacement);

        [DllImport("__Internal")]
        private static extern void _BannerAdSetVisibility([MarshalAs(UnmanagedType.LPWStr)]string adType, bool visible);

        [DllImport("__Internal")]
        private static extern void _BannerAdReconfigure([MarshalAs(UnmanagedType.LPWStr)]string adType, int width, int height, 
            [MarshalAs(UnmanagedType.LPWStr)]string verticalPlacement, [MarshalAs(UnmanagedType.LPWStr)]string horizontalPlacement);

        [DllImport("__Internal")]
        private static extern void _BannerAdDestroy([MarshalAs(UnmanagedType.LPWStr)]string adType);
#endif

        /// <summary>
        /// Raised when a new ad is received
        /// </summary>
        public static Action<WSABannerAdType, string> AdRefreshed
        {
            get; set;
        }

        /// <summary>
        /// Raised when the ad encounters an error in operations
        /// </summary>
        public static Action<WSABannerAdType, string> ErrorOccurred
        {
            get; set;
        }

#if ENABLE_WINMD_SUPPORT
        private static string _adDuplexAppId;
        private static string _adDuplexAdUnitId;
#endif

        /// <summary>
        /// Initialise the banner ad for the specified provider
        /// </summary>
        /// <param name="adType">The ad network to initialise</param>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitOrPlacementId">The adUnit or placement id</param>
        public static void Initialise(WSABannerAdType adType, string appId, string adUnitOrPlacementId)
        {
#if ENABLE_WINMD_SUPPORT
            switch (adType)
            {
                case WSABannerAdType.AdDuplex:
                    _adDuplexAppId = appId;
                    _adDuplexAdUnitId = adUnitOrPlacementId;
                    break;
            }

            _BannerAdInitialise(AdRefreshedCallback, ErrorOccurredCallback);
#endif
        }

        /// <summary>
        /// Create a banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        /// <param name="width">Width of the ad</param>
        /// <param name="height">Height of the ad</param>
        /// <param name="verticalPlacement">Where should the ad be placed vertically - Stretch is not supported</param>
        /// <param name="horizontalPlacement">Where should the ad be placed horizontally - Stretch is not supported</param>
        public static void CreatAd(WSABannerAdType adType, int width, int height, WSAVerticalPlacement verticalPlacement, WSAHorizontalPlacement horizontalPlacement)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                if (adType == WSABannerAdType.AdDuplex)
                {
                    _BannerAdCreate(adType.ToString(), _adDuplexAppId, _adDuplexAdUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                }
            });
#endif
        }

        /// <summary>
        /// Show or hide the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        /// <param name="visible">Should the ad be visible</param>
        public static void SetAdVisibility(WSABannerAdType adType, bool visible)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                _BannerAdSetVisibility(adType.ToString(), visible);
            });
#endif
        }

        /// <summary>
        /// Reconfigure an existing ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        /// <param name="width">Width of the ad</param>
        /// <param name="height">Height of the ad</param>
        /// <param name="verticalPlacement">Where should the ad be placed vertically - Stretch is not supported</param>
        /// <param name="horizontalPlacement">Where should the ad be placed horizontally - Stretch is not supported</param>
        public static void ReconfigureAd(WSABannerAdType adType, int width, int height, WSAVerticalPlacement verticalPlacement, WSAHorizontalPlacement horizontalPlacement)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                _BannerAdReconfigure(adType.ToString(), width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
            });
#endif
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        public static void DestroyAd(WSABannerAdType adType)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                _BannerAdDestroy(adType.ToString());
            });
#endif
        }

#if ENABLE_WINMD_SUPPORT
        [MonoPInvokeCallback(typeof(AdRefreshedCallbackDelegate))]
        private static void AdRefreshedCallback(string adType, string adUnitOrPlacementId)
        {
            if (AdRefreshed != null)
            {
                AdRefreshed(GetAdType(adType), adUnitOrPlacementId);
            }
        }

        [MonoPInvokeCallback(typeof(ErrorOccurredCallbackDelegate))]
        private static void ErrorOccurredCallback(string adType, string errorMessage)
        {
            if(ErrorOccurred != null)
            {
                ErrorOccurred(GetAdType(adType), errorMessage);
            }
        }

        private static WSABannerAdType GetAdType(string adType)
        {
            if (adType == WSABannerAdType.AdDuplex.ToString())
            {
                return WSABannerAdType.AdDuplex;
            }
            else
            {
                return WSABannerAdType.AdDuplex;
            }
        }
#endif
    }
}