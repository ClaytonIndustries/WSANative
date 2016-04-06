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
        public static Action AdReady
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user cancels the interstitial ad before it is considered complete
        /// </summary>
        public static Action Cancelled
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad has been closed and the experience is considered complete
        /// </summary>
        public static Action Completed
        {
            get; set;
        }

        /// <summary>
        /// Raised when the interstitial ad encounters an operational error
        /// </summary>
        public static Action ErrorOccurred
        {
            get; set;
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<string, string> Request;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action Show;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action Close;

        private static string _appId;
        private static string _adUnitId;

        /// <summary>
        /// Initialise the interstitial ad
        /// </summary>
        /// <param name="appId">Your apps id</param>
        /// <param name="adUnitId">Your ad unit id</param>
        public static void Initialise(string appId, string adUnitId)
        {
            _appId = appId;
            _adUnitId = adUnitId;
        }

        /// <summary>
        /// Requests an interstitial ad from the server
        /// The AdReady event will fire when the request completes successfully
        /// </summary>
        public static void RequestAd()
        {
            if(Request != null)
            {
                Request(_appId, _adUnitId);
            }
        }

        /// <summary>
        /// Shows the interstitial ad if it is ready
        /// It is recommended that you have at least a 60 second gap between ads
        /// </summary>
        public static void ShowAd()
        {
            if(Show != null)
            {
                Show();
            }
        }

        /// <summary>
        /// Closes the interstitial ad that is showing
        /// </summary>
        public static void CloseAd()
        {
            if(Close != null)
            {
                Close();
            }
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        /// <param name="action"></param>
        public static void RaiseActionOnAppThread(Action action)
        {
#if NETFX_CORE
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