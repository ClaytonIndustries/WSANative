////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using System.Net.Http;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
#endif

using System;

namespace CI.WSANative.Notification
{
    public static class WSANativeNotification
    {
        /// <summary>
        /// Shows a toast notfication
        /// </summary>
        /// <param name="title">Title for the toast notification</param>
        /// <param name="text">Text to show on the toast notification</param>
        /// <param name="tag">Optional tag, toast notifications with the same tag will overwrite each other in the action centre 
        /// (null or empty if not needed - has no effect on Windows 8.1)</param>
        /// <param name="image">Image to show on the toast notification - specified as the location in the built uwp solution e.g ms-appx:///Assets/Square150x150Logo.png 
        /// (optional and will not show on Windows Phone 8.1)</param>
        public static void ShowToastNotification(string title, string text, string tag, Uri image = null)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            XmlDocument toastXml = null;

            if (image != null)
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

                XmlNodeList imageElements = toastXml.GetElementsByTagName("image");

                if (imageElements != null && imageElements.Length >= 1)
                {
                    ((XmlElement)imageElements[0]).SetAttribute("src", image.OriginalString);
                }
            }
            else
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            }

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            if (stringElements != null && stringElements.Length >= 2)
            {
                stringElements[0].AppendChild(toastXml.CreateTextNode(title));
                stringElements[1].AppendChild(toastXml.CreateTextNode(text));

                ToastNotification toast = new ToastNotification(toastXml);
#if UNITY_WSA_10_0
                if(!string.IsNullOrEmpty(tag))
                {
                    toast.Tag = tag;
                }
#endif
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
#endif
        }

        /// <summary>
        /// Shows a toast notification at a specific date and time
        /// </summary>
        /// <param name="title">Title for the toast notification</param>
        /// <param name="text">Text to show on the toast notification</param>
        /// <param name="deliveryTime">The date and time that the toast notification should be displayed </param>
        /// <param name="tag">Optional tag, toast notifications with the same tag will overwrite each other in the action centre 
        /// (null or empty if not needed - has no effect on Windows 8.1)</param>
        /// <param name="image">Image to show on the toast notification - specified as the location in the built uwp solution e.g ms-appx:///Assets/Square150x150Logo.png 
        /// (optional and will not show on Windows Phone 8.1)</param>
        public static void ShowScheduledToastNotification(string title, string text, DateTime deliveryTime, string tag, Uri image = null)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            XmlDocument toastXml = null;

            if (image != null)
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

                XmlNodeList imageElements = toastXml.GetElementsByTagName("image");

                if (imageElements != null && imageElements.Length >= 1)
                {
                    ((XmlElement)imageElements[0]).SetAttribute("src", image.OriginalString);
                }
            }
            else
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            }

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            if (stringElements != null && stringElements.Length >= 2)
            {
                stringElements[0].AppendChild(toastXml.CreateTextNode(title));
                stringElements[1].AppendChild(toastXml.CreateTextNode(text));

                ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, new DateTimeOffset(deliveryTime));
#if UNITY_WSA_10_0
                if(!string.IsNullOrEmpty(tag))
                {
                    toast.Tag = tag;
                }
#endif
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            }
#endif
        }

        /// <summary>
        /// Removes a toast notification from the action centre (Has no effect on Windows 8.1)
        /// </summary>
        /// <param name="tag">The tag assigned to the toast notification</param>
        public static void RemoveToastNotification(string tag)
        {
#if (NETFX_CORE && UNITY_WSA_10_0) || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            ToastNotificationManager.History.Remove(tag);
#endif
        }

        /// <summary>
        /// Attempts to create a push notification channel - response will be null if it fails
        /// </summary>
        /// <param name="response">The push notification channel</param>
        public static void CreatePushNotificationChannel(Action<WSAPushNotificationChannel> response)
        {
#if NETFX_CORE
            CreatePushNotificationChannelAsync(response);
#endif
        }

#if NETFX_CORE
        private static async void CreatePushNotificationChannelAsync(Action<WSAPushNotificationChannel> response)
        {
            PushNotificationChannel channel = null;

            try
            {
                channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            }
            catch
            {
            }

            if(channel != null)
            {
                response(new WSAPushNotificationChannel(channel));
            }
            else
            {
                response(null);
            }
        }
#endif

        /// <summary>
        /// Register this instance to received push notifications from your server, it is recommended that your server be using HTTPS and that information is sent in a secure manner.
        /// The channelUri is posted with content type x-www-form-urlencoded in the form { key:ChannelUri, value:channelUri }
        /// </summary>
        /// <param name="serverUrl">The url on your server that you want to post to</param>
        /// <param name="channelUri">The uri received from CreatePushNotificationChannel</param>
        /// <param name="authorisation">An optional code to authenticate this app when it hits your server. Specify empty string to ignore otherwise the Authorization header will be added</param>
        /// <param name="response">Indicates whether the request was successful along with any text response the server sends</param>
        public static void SendPushNotificationUriToServer(string serverUrl, string channelUri, string authorisation, Action<bool, string> response)
        {
#if NETFX_CORE
            SendPushNotificationUriToServerAsync(serverUrl, channelUri, authorisation, response);
#endif
        }

#if NETFX_CORE
        private static async void SendPushNotificationUriToServerAsync(string serverUrl, string channelUri, string authorisation, Action<bool, string> response)
        {
            string result = string.Empty;
            bool isSuccess = false;
            
            using(HttpClient client = new HttpClient())
            {
                if(!string.IsNullOrEmpty(authorisation))
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", authorisation);
                }

                try
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("ChannelUri", channelUri)
                    });

                    HttpResponseMessage responseMessage = await client.PostAsync(serverUrl, content);
        
                    if(responseMessage.IsSuccessStatusCode)
                    {
                        result = await responseMessage.Content.ReadAsStringAsync();
                        isSuccess = true;
                    } 
                }
                catch
                {
                }

                if(response != null)
                {
                    response(isSuccess, result);
                }
            }
        }
#endif
    }
}