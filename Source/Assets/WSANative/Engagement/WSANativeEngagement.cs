////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace CI.WSANative.Engagement
{
    public static class WSANativeEngagement
    {
        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<Dictionary<string, string>> _ShowFeedbackHub;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Func<bool> _IsFeedbackHubSupported;

        /// <summary>
        /// Returns whether the users device supports the feedback hub (i.e they are runnings Windows 10 build 10.0.14271 or later)
        /// </summary>
        /// <returns>Is the feedback hub supported</returns>
        public static bool IsFeedbackHubSupported()
        {
#if NETFX_CORE
            return _IsFeedbackHubSupported();
#else
            return false;
#endif
        }

        /// <summary>
        /// Shows the feedback hub if it is available
        /// </summary>
        /// <param name="feedbackProperties">Metadata that you want to associate with the feedback (does not need to be specified)</param>
        public static void ShowFeedbackHub(Dictionary<string, string> feedbackProperties = null)
        {
#if NETFX_CORE
            if (_ShowFeedbackHub != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _ShowFeedbackHub(feedbackProperties);
                }, false);
            }
#endif
        }
    }
}