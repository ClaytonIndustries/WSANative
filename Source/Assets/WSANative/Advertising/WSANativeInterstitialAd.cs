////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#define ADS_DISABLED

using System;

#if NETFX_CORE && !ADS_DISABLED
using Microsoft.Advertising.WinRT.UI;
#endif

namespace CI.WSANative.Advertising
{
    public static class WSANativeInterstitialAd
    {
        /// <summary>
        /// Raised when the interstitial ad is ready to be shown
        /// </summary>
        public static Action AdReady;

        /// <summary>
        /// Raised when the user cancels the interstitial ad before it is considered complete
        /// </summary>
        public static Action Cancelled;

        /// <summary>
        /// Raised when the interstitial ad has been closed and the experience is considered complete
        /// </summary>
        public static Action Completed;

        /// <summary>
        /// Raised when the interstitial ad encounters an operational error
        /// </summary>
        public static Action ErrorOcurred;

#if NETFX_CORE && !ADS_DISABLED
        private static InterstitialAd _interstitialAd;
#endif

        private static string _appId;
        private static string _adUnitId;

        /// <summary>
        /// Initialise the interstitial ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your add unit id</param>
        public static void Init(string appId, string adUnitId)
        {
            _appId = appId;
            _adUnitId = adUnitId;

#if NETFX_CORE && !ADS_DISABLED
            _interstitialAd = new InterstitialAd();
            _interstitialAd.AdReady += delegate { RaiseActionOnAppThread(AdReady); };
            _interstitialAd.ErrorOccurred += delegate { RaiseActionOnAppThread(Cancelled); };
            _interstitialAd.Completed += delegate { RaiseActionOnAppThread(Completed); };
            _interstitialAd.Cancelled += delegate { RaiseActionOnAppThread(ErrorOcurred); };
#endif
        }

        /// <summary>
        /// Requests an interstitial ad from the server
        /// The AdReady event will fire when the request completes successfully
        /// </summary>
        public static void RequestAd()
        {
#if NETFX_CORE && !ADS_DISABLED
            if(_interstitialAd != null)
            {
                try
                {
                    UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                    {
                        _interstitialAd.RequestAd(AdType.Video, _appId, _adUnitId);
                    }, false);
                }
                catch
                {
                }
            }
#endif
        }

        /// <summary>
        /// Shows the interstitial ad if it is ready
        /// It is recommended that you have at least a 60 second gap between ads
        /// </summary>
        public static void Show()
        {
#if NETFX_CORE && !ADS_DISABLED
            if (_interstitialAd != null && _interstitialAd.State == InterstitialAdState.Ready)
            {
                try
                {
                    UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                    {
                        _interstitialAd.Show();
                    }, false);
                }
                catch
                {
                }
            }
#endif
        }

        /// <summary>
        /// Closes the interstitial ad that is showing
        /// </summary>
        public static void Close()
        {
#if NETFX_CORE && !ADS_DISABLED
            if (_interstitialAd != null && _interstitialAd.State == InterstitialAdState.Showing)
            {
                try
                {
                    UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                    {
                        _interstitialAd.Close();
                    }, false);
                }
                catch
                {
                }
            }
#endif
        }

        private static void RaiseActionOnAppThread(Action action)
        {
#if NETFX_CORE && !ADS_DISABLED
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                if (action != null)
                {
                    action();
                }
            }, false);
#endif
        }
    }
}