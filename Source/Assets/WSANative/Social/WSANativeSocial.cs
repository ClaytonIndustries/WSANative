#if NETFX_CORE
using System;
#endif

namespace CI.WSANative.Social
{
    public static class WSANativeSocial
    {
        /// <summary>
        /// Shows the store description page for you app
        /// </summary>
        /// <param name="appId">Your apps id - get from Windows Dev Center under Your App -> App Management -> App Identity -> use the the last part of URL for Windows 10</param>
        public static void ShowAppStoreDescriptionPage(string appId)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var uri = new Uri("ms-windows-store://pdp/?ProductId=" + appId);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }, false);
#endif
        }

        /// <summary>
        /// Shows the store review page for you app
        /// </summary>
        /// <param name="appId">Your apps id - get from Windows Dev Center under Your App -> App Management -> App Identity -> use the the last part of URL for Windows 10</param>
        public static void ShowAppStoreReviewPage(string appId)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var uri = new Uri("ms-windows-store://review/?ProductId=" + appId);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }, false);
#endif
        }

        /// <summary>
        /// Allows the user to send an email to the specified address - will launch the default email client
        /// </summary>
        /// <param name="emailAddress">Prepopulate the to field with this address</param>
        public static void ComposeEmail(string emailAddress)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var uri = new Uri("mailto:" + emailAddress);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }, false);
#endif
        }
    }
}