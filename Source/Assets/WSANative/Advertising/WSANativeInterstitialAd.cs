////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if ENABLE_WINMD_SUPPORT
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
#endif

namespace CI.WSANative.Advertising
{
    public static class WSANativeInterstitialAd
    {
#if ENABLE_WINMD_SUPPORT
        private delegate void AdReadyCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId);
        private delegate void CancelledCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId);
        private delegate void CompletedCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId);
        private delegate void ErrorOccurredCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string errorMessage);

        [DllImport("__Internal")]
        private static extern void _InterstitialAdInitialise([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string appId,
            AdReadyCallbackDelegate adReadyCallbackDelegate, CancelledCallbackDelegate cancelledCallbackDelegate, 
            CompletedCallbackDelegate completedCallbackDelegate, ErrorOccurredCallbackDelegate errorOccurredCallbackDelegate);

        [DllImport("__Internal")]
        private static extern void _InterstitialAdRequest([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId);

        [DllImport("__Internal")]
        private static extern void _InterstitialAdShow([MarshalAs(UnmanagedType.LPWStr)]string adType, [MarshalAs(UnmanagedType.LPWStr)]string adUnitOrPlacementId);
#endif

        /// <summary>
        /// Raised when the interstitial ad is ready to be shown
        /// </summary>
        public static Action<WSAInterstitialAdType, string> AdReady
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user cancels the interstitial ad before it is considered complete (AdDuplex does not raise this event)
        /// </summary>
        public static Action<WSAInterstitialAdType, string> Cancelled
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad has been closed and the experience is considered complete
        /// </summary>
        public static Action<WSAInterstitialAdType, string> Completed
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad encounters an operational error
        /// </summary>
        public static Action<WSAInterstitialAdType, string> ErrorOccurred
        {
            get; set;
        }

        /// <summary>
        /// Initialise the interstitial ad for the specified provider
        /// </summary>
		/// <param name="adType">The ad network to initialise</param>
        /// <param name="appId">Your apps id</param>
        public static void Initialise(WSAInterstitialAdType adType, string appId)
        {
#if ENABLE_WINMD_SUPPORT
            _InterstitialAdInitialise(adType.ToString(), appId, AdReadyCallback, CancelledCallback, CompletedCallback, ErrorOccurredCallback);
#endif
        }

        /// <summary>
        /// Requests an interstitial ad from the server.
        /// The AdReady event will fire when the request completes successfully.
        /// Only needs to be called once for Vungle ads (ads will be automatically fetched from then on)
        /// </summary>
        /// <param name="adType">The type of ad to request</param>
        /// <param name="adUnitOrPlacementId">The adUnit or placement id</param>
        public static void RequestAd(WSAInterstitialAdType adType, string adUnitOrPlacementId)
        {
#if ENABLE_WINMD_SUPPORT
            _InterstitialAdRequest(adType.ToString(), adUnitOrPlacementId);
#endif
        }

        /// <summary>
        /// Shows the interstitial ad if it is ready.
        /// It is recommended that you have at least a 60 second gap between ads
        /// </summary>
        /// <param name="adType">The type of ad to show</param>
        /// <param name="placement">The adUnit or placement id</param>
        public static void ShowAd(WSAInterstitialAdType adType, string adUnitOrPlacementId)
        {
#if ENABLE_WINMD_SUPPORT
            _InterstitialAdShow(adType.ToString(), adUnitOrPlacementId);
#endif
        }

#if ENABLE_WINMD_SUPPORT
        [MonoPInvokeCallback(typeof(AdReadyCallbackDelegate))]
        private static void AdReadyCallback(string adType, string adUnitOrPlacementId)
        {
            if (AdReady != null)
            {
                AdReady(GetAdType(adType), adUnitOrPlacementId);
            }
        }

        [MonoPInvokeCallback(typeof(CancelledCallbackDelegate))]
        private static void CancelledCallback(string adType, string adUnitOrPlacementId)
        {
            if (Cancelled != null)
            {
                Cancelled(GetAdType(adType), adUnitOrPlacementId);
            }
        }

        [MonoPInvokeCallback(typeof(CompletedCallbackDelegate))]
        private static void CompletedCallback(string adType, string adUnitOrPlacementId)
        {
            if (Completed != null)
            {
                Completed(GetAdType(adType), adUnitOrPlacementId);
            }
        }

        [MonoPInvokeCallback(typeof(ErrorOccurredCallbackDelegate))]
        private static void ErrorOccurredCallback(string adType, string errorMessage)
        {
            if (ErrorOccurred != null)
            {
                ErrorOccurred(GetAdType(adType), errorMessage);
            }
        }

        private static WSAInterstitialAdType GetAdType(string adType)
        {     
            if(adType == WSAInterstitialAdType.AdDuplex.ToString())
            {
                return WSAInterstitialAdType.AdDuplex;
            }
            else
            {
                return WSAInterstitialAdType.Vungle;
            }
        }
#endif
    }
}