////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.Storage.Streams;
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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var uri = new Uri("ms-windows-store://review/?ProductId=" + appId);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }, false);
#endif
        }

        /// <summary>
        /// Allows the user to send an email with the specified address, body and attachment - will launch the default email client
        /// </summary>
        /// <param name="to">Prepopulate the to field</param>
        /// <param name="subject">Prepopulate the subject field</param>
        /// <param name="messageBody">Prepopulate the email body</param>
        /// <param name="attachmentFilename">Filename of the attachment - optional</param>
        /// <param name="attachmentData">Data for the attachment - optional</param>
        public static void ComposeEmail(string to, string subject, string messageBody, string attachmentFilename = null, byte[] attachmentData = null)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                EmailMessage emailMessage = new EmailMessage()
                {
                    Subject = subject,
                    Body = messageBody
                };

                if (!string.IsNullOrWhiteSpace(attachmentFilename) && attachmentData != null)
                {
                    InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();

                    await memoryStream.WriteAsync(attachmentData.AsBuffer());

                    IRandomAccessStreamReference stream = RandomAccessStreamReference.CreateFromStream(memoryStream);

                    EmailAttachment emailAttachment = new EmailAttachment(attachmentFilename, stream);

                    emailMessage.Attachments.Add(emailAttachment);
                }

                emailMessage.To.Add(new EmailRecipient(to));

                await EmailManager.ShowComposeNewEmailAsync(emailMessage);
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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
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
    }
}