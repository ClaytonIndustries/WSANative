using System;

namespace CI.WSANative.Common
{
    public static class ThreadRunner
    {
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