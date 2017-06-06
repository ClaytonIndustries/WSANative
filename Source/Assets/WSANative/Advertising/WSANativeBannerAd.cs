////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////
//  
// IL2CPP Implementation
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_IL2CPP && UNITY_WSA_10_0
using System;
using System.Runtime.InteropServices;
using AOT;

namespace CI.WSANative.Advertising
{
    public static class WSANativeBannerAd
    {
#if UNITY_EDITOR
        private static bool _unityEditor = true;
#else
        private static bool _unityEditor = false;
#endif

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

        private static string _msAppId;
        private static string _msAdUnitId;

        private static string _adDuplexAppId;
        private static string _adDuplexAdUnitId;

        /// <summary>
        /// Initialise the banner ad for the specified provider
        /// </summary>
        /// <param name="adType">The ad network to initialise</param>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your apps ad unit id</param>
        public static void Initialise(WSABannerAdType adType, string appId, string adUnitId)
        {
            if (!_unityEditor)
            {
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
            }
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
            if (!_unityEditor)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    if (adType == WSABannerAdType.AdDuplex)
                    {
                        _BannerAdCreate(adType.ToString(), _adDuplexAppId, _adDuplexAdUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                    }
                    else if (adType == WSABannerAdType.Microsoft)
                    {
                        _BannerAdCreate(adType.ToString(), _msAppId, _msAdUnitId, width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                    }
                }, false);
            }
        }

        /// <summary>
        /// Show or hide the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        /// <param name="visible">Should the ad be visible</param>
        public static void SetAdVisibility(WSABannerAdType adType, bool visible)
        {
            if (!_unityEditor)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _BannerAdSetVisibility(adType.ToString(), visible);
                }, false);
            }
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
            if (!_unityEditor)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _BannerAdReconfigure(adType.ToString(), width, height, verticalPlacement.ToString(), horizontalPlacement.ToString());
                }, false);
            }
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        public static void DestroyAd(WSABannerAdType adType)
        {
            if (!_unityEditor)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _BannerAdDestroy(adType.ToString());
                }, false);
            }
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
    }
}

#else

////////////////////////////////////////////////////////////////////////////////
//  
// .NET Implementation
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

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSABannerAdSettings> Create;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSABannerAdType, bool> SetVisiblity;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSABannerAdSettings> Reconfigure;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSABannerAdType> Destroy;

#pragma warning disable 0414
        private static string _adDuplexAppId;
        private static string _adDuplexAdUnitId;

        private static string _msAppId;
        private static string _msAdUnitId;
#pragma warning restore 0414

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
                    _adDuplexAdUnitId = adUnitId;
                    break;
                case WSABannerAdType.Microsoft:
                    _msAppId = appId;
                    _msAdUnitId = adUnitId;
                    break;
            }
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
#if NETFX_CORE
            if (Create != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    if (adType == WSABannerAdType.AdDuplex)
                    {
                        Create(new WSABannerAdSettings()
                        {
                            AdType = adType,
                            AppId = _adDuplexAppId,
                            AdUnitId = _adDuplexAdUnitId,
                            Width = width,
                            Height = height,
                            VerticalPlacement = verticalPlacement,
                            HorizontalPlacement = horizontalPlacement
                        });
                    }
                    else if (adType == WSABannerAdType.Microsoft)
                    {
                        Create(new WSABannerAdSettings()
                        {
                            AdType = adType,
                            AppId = _msAppId,
                            AdUnitId = _msAdUnitId,
                            Width = width,
                            Height = height,
                            VerticalPlacement = verticalPlacement,
                            HorizontalPlacement = horizontalPlacement
                        });
                    }
                }, false);
            }
#endif
        }

        /// <summary>
        /// Show or hide the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        /// <param name="visible">Should the ad be visible</param>
        public static void SetAdVisibility(WSABannerAdType adType, bool visible)
        {
#if NETFX_CORE
            if (SetVisiblity != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    SetVisiblity(adType, visible);
                }, false);
            }
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
#if NETFX_CORE
            if (Reconfigure != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Reconfigure(new WSABannerAdSettings()
                    {
                        AdType = adType,
                        Width = width,
                        Height = height,
                        VerticalPlacement = verticalPlacement,
                        HorizontalPlacement = horizontalPlacement
                    });
                }, false);
            }
#endif
        }

        /// <summary>
        /// Destroy the banner ad
        /// </summary>
        /// <param name="adType">The ad type</param>
        public static void DestroyAd(WSABannerAdType adType)
        {
#if NETFX_CORE
            if (Destroy != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Destroy(adType);
                }, false);
            }
#endif
        }
    }
}
#endif