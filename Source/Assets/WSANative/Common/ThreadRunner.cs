////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.Common
{
    public static class ThreadRunner
    {
        /// <summary>
        /// Run the the supplied action on the UI thread
        /// </summary>
        /// <param name="action">The action to run</param>
        /// <param name="waitUntilDone">Should we block until the action completes</param>
        public static void RunOnUIThread(Action action, bool waitUntilDone = false)
        {
            if (UnityEngine.WSA.Application.RunningOnUIThread())
            {
                action();
            }
            else
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    action();
                }, waitUntilDone);
            }
        }

        /// <summary>
        /// Run the the supplied action on the app thread
        /// </summary>
        /// <param name="action">The action to run</param>
        /// <param name="waitUntilDone">Should we block until the action completes</param>
        public static void RunOnAppThread(Action action, bool waitUntilDone = false)
        {
            if (UnityEngine.WSA.Application.RunningOnAppThread())
            {
                action();
            }
            else
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    action();
                }, waitUntilDone);
            }
        }
    }
}