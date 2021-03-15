////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if ENABLE_WINMD_SUPPORT
using System.Runtime.InteropServices;
using AOT;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Advertising
{
    public static class WSANativeBannerAd
    {
#if ENABLE_WINMD_SUPPORT
        private delegate void AdRefreshedCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);
        private delegate void ErrorOccurredCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string errorMessage);

        [DllImport("__Internal")]
        private static extern void _BannerAdInitialise(AdRefreshedCallbackDelegate adRefreshedCallback, ErrorOccurredCallbackDelegate errorOccurredCallback);

        [DllImport("__Internal")]
        private static extern void _BannerAdCreate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string appId, [MarshalAs(UnmanagedType.LPWStr)]string adUnitId, 
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
        public static Action<WSABannerAdType> AdRefreshed
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
        private static string _msAppId;
        private static string _msAdUnitId;

        private static string _adDuplexAppId;
        private static string _adDuplexAdUnitId;
#endif

        /// <summary>
        /// Initialise the banner ad for the specified provider
        /// </summary>
        /// <param name="adType">The ad network to initialise</param>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your apps ad unit id</param>
        public static void Initialise(WSABannerAdType adType, string appId, string adUnitId)
        {
#if ENABLE_WINMD_SUPPORT
            switch (adType)
            {
                case WSABannerAdType.AdDuplex:
                    _adDuplexAppId = appId;
                    _adDuplexAdUnitId = adUnitId;
                    break;
                case WSABannerAdType.Microsoft:
                    _msAppId = appId;
                    _msAdUnitId = adUnitId;
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
        /// <param name="verticalPlacement">Where should the ad be placed vertically</param>
        /// <param name="horizontalPlacement">Where should the ad be placed horizontally</param>
        public static void CreatAd(WSABannerAdType adType, int width, int height, WSAAdVerticalPlacement verticalPlacement, WSAAdHorizontalPlacement horizontalPlacement)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                if (adType == WSABannerAdType.AdDuplex)
                {
                    _BannerAdCreate(adType.ToString(), _adDuplexAppId, _adDuplexAdUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                }
                else if (adType == WSABannerAdType.Microsoft)
                {
                    _BannerAdCreate(adType.ToString(), _msAppId, _msAdUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
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
        /// <param name="verticalPlacement">Where should the ad be placed vertically</param>
        /// <param name="horizontalPlacement">Where should the ad be placed horizontally</param>
        public static void ReconfigureAd(WSABannerAdType adType, int width, int height, WSAAdVerticalPlacement verticalPlacement, WSAAdHorizontalPlacement horizontalPlacement)
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
        private static void AdRefreshedCallback(string adType)
        {
            if (AdRefreshed != null)
            {
                AdRefreshed(GetAdType(adType));
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
                return WSABannerAdType.Microsoft;
            }
        }
#endif
    }
}