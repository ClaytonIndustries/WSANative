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
        public static Action<string, string, WSAInterstitialAdType> _Request;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSAInterstitialAdType> _Show;

        private static string _msAppId;
        private static string _msAdUnitId;

        private static string _vungleAppId;

        /// <summary>
        /// Initialise the Microsoft interstitial ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your ad unit id</param>
        public static void InitialiseMicrosoft(string appId, string adUnitId)
        {
            _msAppId = appId;
            _msAdUnitId = adUnitId;
        }

        /// <summary>
        /// Initialise the Vungle interstitial ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        public static void InitialiseVungle(string appId)
        {
            _vungleAppId = appId;
        }

        /// <summary>
        /// Requests an interstitial ad from the server.
        /// The AdReady event will fire when the request completes successfully.
        /// Only needs to be called once for Vungle ads (ads will be automatically fetched after the first time)
        /// </summary>
        /// <param name="adType">The type of ad to request</param>
        public static void RequestAd(WSAInterstitialAdType adType)
        {
            if(_Request != null)
            {
                if (adType == WSAInterstitialAdType.Microsoft)
                {
                    _Request(_msAppId, _msAdUnitId, adType);
                }
                else
                {
                    _Request(_vungleAppId, string.Empty, adType);
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
            if(_Show != null)
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