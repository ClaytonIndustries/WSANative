////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
//  
// .NET Implementation
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

////////////////////////////////////////////////////////////////////////////////
//  
// IL2CPP Implementation
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
        private delegate void AdRefreshedCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);
        private delegate void ErrorOccurredCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);

        [DllImport("__Internal")]
        private static extern void _BannerAdInitialise(AdRefreshedCallbackDelegate adRefreshedCallback, ErrorOccurredCallbackDelegate errorOccurredCallback);

        [DllImport("__Internal")]
        private static extern void _BannerAdCreate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string appId, 
            [MarshalAs(UnmanagedType.LPWStr)]string adUnitId, int width, int height, 
            [MarshalAs(UnmanagedType.LPWStr)]string verticalPlacement, [MarshalAs(UnmanagedType.LPWStr)]string horizontalPlacement);

        [DllImport("__Internal")]
        private static extern void _BannerAdSetVisibility([MarshalAs(UnmanagedType.LPWStr)]string adType, bool visible);

        [DllImport("__Internal")]
        private static extern void _BannerAdDestroy([MarshalAs(UnmanagedType.LPWStr)]string adType);

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
        public static Action<WSABannerAdType> ErrorOccurred
        {
            get; set;
        }

        private static string _msAppId;
        private static string _msAdUnitId;

        private static string _adDuplexAppId;
        private static string _adDuplexAppUnitId;

        /// <summary>
        /// Initialise the banner ad for the specified provider
        /// </summary>
        /// <param name="adType">The ad network to initialise</param>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your apps ad unit id</param>
        public static void Initialise(WSABannerAdType adType, string appId, string adUnitId)
        {
            switch (adType)
            {
                case WSABannerAdType.AdDuplex:
                    _adDuplexAppId = appId;
                    _adDuplexAppUnitId = adUnitId;
                    break;
                case WSABannerAdType.Microsoft:
                    _msAppId = appId;
                    _msAdUnitId = adUnitId;
                    break;
            }

            _BannerAdInitialise(AdRefreshedCallback, ErrorOccurredCallback);
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
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                if (adType == WSABannerAdType.AdDuplex)
                {
                    _BannerAdCreate(adType.ToString(), _adDuplexAppId, _adDuplexAppUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                }
                else if(adType == WSABannerAdType.Microsoft)
                {
                    _BannerAdCreate(adType.ToString(), _msAppId, _msAdUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                }
            }, false);
        }

        /// <summary>
        /// Show or hide the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        /// <param name="visible">Should the ad be visible</param>
        public static void SetAdVisibility(WSABannerAdType adType, bool visible)
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _BannerAdSetVisibility(adType.ToString(), visible);
            }, false);
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        public static void DestroyAd(WSABannerAdType adType)
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _BannerAdDestroy(adType.ToString());
            }, false);
        }


        [MonoPInvokeCallback(typeof(AdRefreshedCallbackDelegate))]
        private static void AdRefreshedCallback(string adType)
        {
            if (AdRefreshed != null)
            {
                AdRefreshed(GetAdType(adType));
            }
        }

        [MonoPInvokeCallback(typeof(ErrorOccurredCallbackDelegate))]
        private static void ErrorOccurredCallback(string adType)
        {
            if(ErrorOccurred != null)
            {
                ErrorOccurred(GetAdType(adType));
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
    }
}
#endif