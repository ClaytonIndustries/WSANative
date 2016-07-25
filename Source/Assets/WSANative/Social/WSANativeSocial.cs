////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using Windows.ApplicationModel.DataTransfer;
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

        /// <summary>
        /// Launch the native share dialog to share content using another app
        /// </summary>
        /// <typeparam name="T">The type of content to share - currently supports string to share text, byte[] to share a picture and Uri to share a link</typeparam>
        /// <param name="title">Title to display on the dialog</param>
        /// <param name="description">Description to display on the dialog</param>
        /// <param name="content">Content to share</param>
        public static void Share<T>(string title, string description, T content)
        {
#if NETFX_CORE
            DataTransferManager.GetForCurrentView().DataRequested += (s, a) =>
            {
                DataRequest request = a.Request;

                request.Data.Properties.Title = title;
                request.Data.Properties.Description = description;

                if(T is string)
                {
                    request.Data.SetText(content);
                }
                else if(T is Uri)
                {
                    request.Data.SetWebLink(content);
                }
                else if(T is byte[])
                {
                    InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
                    await memoryStream.WriteAsync(content.AsBuffer());
                    RandomAccessStreamReference randomAccessStream = RandomAccessStreamReference.CreateFromStream(memoryStream.GetInputStreamAt(0));

                    request.Data.SetBitmap(randomAccessStream);
                }
            };

            DataTransferManager.ShowShareUI();
#endif
        }
    }
}