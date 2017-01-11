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

#if ENABLE_IL2CPP && NETFX_CORE
using System;
using System.Runtime.InteropServices;
using AOT;

namespace CI.WSANative.Advertising
{
    public static class WSANativeInterstitialAd
    {
        private delegate void AdReadyCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);
        private delegate void CancelledCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);
        private delegate void CompletedCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);
        private delegate void ErrorOccurredCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType);

        [DllImport("__Internal")]
        private static extern void _InterstitialAdInitialise(AdReadyCallbackDelegate adReadyCallbackDelegate, CancelledCallbackDelegate cancelledCallbackDelegate, 
            CompletedCallbackDelegate completedCallbackDelegate, ErrorOccurredCallbackDelegate errorOccurredCallbackDelegate);

        [DllImport("__Internal")]
        private static extern void _InterstitialAdRequest([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string appId,
            [MarshalAs(UnmanagedType.LPWStr)]string adUnitId);

        [DllImport("__Internal")]
        private static extern void _InterstitialAdShow([MarshalAs(UnmanagedType.LPWStr)]string adType);

        /// <summary>
        /// Raised when the interstitial ad is ready to be shown
        /// </summary>
        public static Action<WSAInterstitialAdType> AdReady
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user cancels the interstitial ad before it is considered complete
        /// </summary>
        public static Action<WSAInterstitialAdType> Cancelled
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad has been closed and the experience is considered complete
        /// </summary>
        public static Action<WSAInterstitialAdType> Completed
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad encounters an operational error
        /// </summary>
        public static Action<WSAInterstitialAdType> ErrorOccurred
        {
            get; set;
        }

        private static string _adDuplexAppId;
        private static string _adDuplexAdUnitId;

        private static string _msAppId;
        private static string _msAdUnitId;

        private static string _vungleAppId;

        /// <summary>
        /// Initialise the interstitial ad for the specified provider
        /// </summary>
		/// <param name="adType">The ad network to initialise</param>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your apps ad unit id (null or empty for Vungle)</param>
        public static void Initialise(WSAInterstitialAdType adType, string appId, string adUnitId)
        {
            switch (adType)
            {
                case WSAInterstitialAdType.AdDuplex:
                    _adDuplexAppId = appId;
                    _adDuplexAdUnitId = adUnitId;
                    break;
                case WSAInterstitialAdType.Microsoft:
                    _msAppId = appId;
                    _msAdUnitId = adUnitId;
                    break;
                case WSAInterstitialAdType.Vungle:
                    _vungleAppId = appId;
                    break;
            }

            _InterstitialAdInitialise(AdReadyCallback, CancelledCallback, CompletedCallback, ErrorOccurredCallback);
        }

        /// <summary>
        /// Requests an interstitial ad from the server.
        /// The AdReady event will fire when the request completes successfully.
        /// Only needs to be called once for Vungle ads (ads will be automatically fetched from then on)
        /// </summary>
        /// <param name="adType">The type of ad to request</param>
        public static void RequestAd(WSAInterstitialAdType adType)
        {
            switch (adType)
            {
                case WSAInterstitialAdType.AdDuplex:
                    _InterstitialAdRequest(adType.ToString(), _adDuplexAppId, _adDuplexAdUnitId);
                    break;
                case WSAInterstitialAdType.Microsoft:
                    _InterstitialAdRequest(adType.ToString(), _msAppId, _msAdUnitId);
                    break;
                case WSAInterstitialAdType.Vungle:
                    _InterstitialAdRequest(adType.ToString(), _vungleAppId, string.Empty);
                    break;
            }
        }

        /// <summary>
        /// Shows the interstitial ad if it is ready.
        /// It is recommended that you have at least a 60 second gap between ads
        /// </summary>
        /// <param name="adType">The type of ad to show</param>
        public static void ShowAd(WSAInterstitialAdType adType)
        {
            _InterstitialAdShow(adType.ToString());
        }

        [MonoPInvokeCallback(typeof(AdReadyCallbackDelegate))]
        private static void AdReadyCallback(string adType)
        {
            if (AdReady != null)
            {
                AdReady(GetAdType(adType));
            }
        }

        [MonoPInvokeCallback(typeof(CancelledCallbackDelegate))]
        private static void CancelledCallback(string adType)
        {
            if (Cancelled != null)
            {
                Cancelled(GetAdType(adType));
            }
        }

        [MonoPInvokeCallback(typeof(CompletedCallbackDelegate))]
        private static void CompletedCallback(string adType)
        {
            if (Completed != null)
            {
                Completed(GetAdType(adType));
            }
        }

        [MonoPInvokeCallback(typeof(ErrorOccurredCallbackDelegate))]
        private static void ErrorOccurredCallback(string adType)
        {
            if (ErrorOccurred != null)
            {
                ErrorOccurred(GetAdType(adType));
            }
        }

        private static WSAInterstitialAdType GetAdType(string adType)
        {     
            if(adType == WSAInterstitialAdType.AdDuplex.ToString())
            {
                return WSAInterstitialAdType.AdDuplex;
            }
            else if(adType == WSAInterstitialAdType.Microsoft.ToString())
            {
                return WSAInterstitialAdType.Microsoft;
            }
            else
            {
                return WSAInterstitialAdType.Vungle;
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
    public static class WSANativeInterstitialAd
    {
        /// <summary>
        /// Raised when the interstitial ad is ready to be shown
        /// </summary>
        public static Action<WSAInterstitialAdType> AdReady
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user cancels the interstitial ad before it is considered complete
        /// </summary>
        public static Action<WSAInterstitialAdType> Cancelled
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad has been closed and the experience is considered complete
        /// </summary>
        public static Action<WSAInterstitialAdType> Completed
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad encounters an operational error
        /// </summary>
        public static Action<WSAInterstitialAdType> ErrorOccurred
        {
            get; set;
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSAInterstitialAdType, string, string> _Request;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSAInterstitialAdType> _Show;

        private static string _adDuplexAppId;
        private static string _adDuplexAdUnitId;

        private static string _msAppId;
        private static string _msAdUnitId;

        private static string _vungleAppId;

        /// <summary>
        /// Initialise the interstitial ad for the specified provider
        /// </summary>
		/// <param name="adType">The type of ad to request</param>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your apps ad unit id (null or empty for Vungle)</param>
        public static void Initialise(WSAInterstitialAdType adType, string appId, string adUnitId)
        {
            switch (adType)
            {
                case WSAInterstitialAdType.AdDuplex:
                    _adDuplexAppId = appId;
                    _adDuplexAdUnitId = adUnitId;
                    break;
                case WSAInterstitialAdType.Microsoft:
                    _msAppId = appId;
                    _msAdUnitId = adUnitId;
                    break;
                case WSAInterstitialAdType.Vungle:
                    _vungleAppId = appId;
                    break;
            }
        }

        /// <summary>
        /// Requests an interstitial ad from the server.
        /// The AdReady event will fire when the request completes successfully.
        /// Only needs to be called once for Vungle ads (ads will be automatically fetched from then on)
        /// </summary>
        /// <param name="adType">The type of ad to request</param>
        public static void RequestAd(WSAInterstitialAdType adType)
        {
            if (_Request != null)
            {
                switch (adType)
                {
                    case WSAInterstitialAdType.AdDuplex:
                        _Request(adType, _adDuplexAppId, _adDuplexAdUnitId);
                        break;
                    case WSAInterstitialAdType.Microsoft:
                        _Request(adType, _msAppId, _msAdUnitId);
                        break;
                    case WSAInterstitialAdType.Vungle:
                        _Request(adType, _vungleAppId, string.Empty);
                        break;
                }
            }
        }

        /// <summary>
        /// Shows the interstitial ad if it is ready.
        /// It is recommended that you have at least a 60 second gap between ads
        /// </summary>
        /// <param name="adType">The type of ad to show</param>
        public static void ShowAd(WSAInterstitialAdType adType)
        {
            if (_Show != null)
            {
                _Show(adType);
            }
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        /// <param name="action"></param>
        /// <param name="adType"></param>
        public static void RaiseActionOnAppThread(Action<WSAInterstitialAdType> action, WSAInterstitialAdType adType)
        {
#if NETFX_CORE
            if (action != null)
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    action(adType);
                }, false);
            }
#endif
        }
    }
}
#endif