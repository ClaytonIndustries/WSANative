////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
#endif

using System;
using System.Collections.Generic;

namespace CI.WSANative.Social
{
    public static class WSANativeSocial
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
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                DataTransferManager.GetForCurrentView().DataRequested += (s, a) =>
                {
                    DataRequest request = a.Request;

                    request.Data.Properties.Title = title;
                    request.Data.Properties.Description = description;

                    if (content is string)
                    {
                        request.Data.SetText(content as string);
                    }
                    else if (content is Uri)
                    {
                        request.Data.SetWebLink(content as Uri);
                    }
                    else if (content is byte[])
                    {
                        InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
                        memoryStream.WriteAsync((content as byte[]).AsBuffer()).AsTask().Wait();
                        RandomAccessStreamReference randomAccessStream = RandomAccessStreamReference.CreateFromStream(memoryStream);

                        request.Data.Properties.Thumbnail = randomAccessStream;

                        request.Data.SetBitmap(randomAccessStream);
                    }
                };

                DataTransferManager.ShowShareUI();
            }, false);
#endif
        }

        /// <summary>
        /// Returns whether the users device supports the feedback hub (i.e they are runnings Windows 10 build 10.0.14271 or later)
        /// </summary>
        /// <returns>Is the feedback hub supported</returns>
        public static bool IsFeedbackHubSupported()
        {
#if UNITY_WSA_10_0
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
#if UNITY_WSA_10_0
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