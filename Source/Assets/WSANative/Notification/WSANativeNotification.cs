////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using Windows.Data.Xml.Dom;
using System.Net.Http;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
#endif

using System;
using System.Collections.Generic;

namespace CI.WSANative.Notification
{
    public static class WSANativeNotification
    {
        /// <summary>
        /// Shows a toast notfication
        /// </summary>
        /// <param name="notification">The notification to show</param>
        public static void ShowToastNotification(WSAToastNotification notification)
        {
#if ENABLE_WINMD_SUPPORT
            XmlDocument toastXml = null;

            if (notification.Image != null)
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

                XmlNodeList imageElements = toastXml.GetElementsByTagName("image");

                if (imageElements != null && imageElements.Length >= 1)
                {
                    ((XmlElement)imageElements[0]).SetAttribute("src", notification.Image.OriginalString);
                }
            }
            else
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            }

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            if (stringElements != null && stringElements.Length >= 2)
            {
                stringElements[0].AppendChild(toastXml.CreateTextNode(notification.Title));
                stringElements[1].AppendChild(toastXml.CreateTextNode(notification.Text));

                ToastNotification toast = new ToastNotification(toastXml);

                if(!string.IsNullOrEmpty(notification.Tag))
                {
                    toast.Tag = notification.Tag;
                }

               ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
#endif
        }

        /// <summary>
        /// Shows a toast notification at a specific date and time
        /// </summary>
        /// <param name="notification">The notification to schedule</param>
        public static void ShowScheduledToastNotification(WSAScheduledToastNotification notification)
        {
#if ENABLE_WINMD_SUPPORT
            XmlDocument toastXml = null;

            if (notification.Image != null)
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

                XmlNodeList imageElements = toastXml.GetElementsByTagName("image");

                if (imageElements != null && imageElements.Length >= 1)
                {
                    ((XmlElement)imageElements[0]).SetAttribute("src", notification.Image.OriginalString);
                }
            }
            else
            {
                toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            }

            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            if (stringElements != null && stringElements.Length >= 2)
            {
                stringElements[0].AppendChild(toastXml.CreateTextNode(notification.Title));
                stringElements[1].AppendChild(toastXml.CreateTextNode(notification.Text));

                ScheduledToastNotification toast = new ScheduledToastNotification(toastXml, new DateTimeOffset(notification.DeliveryTime));
                toast.Id = notification.Id;

                if(!string.IsNullOrEmpty(notification.Tag))
                {
                    toast.Tag = notification.Tag;
                }

                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            }
#endif
        }

        /// <summary>
        /// Gets a list of the scheduled toast notifications - (only id, tag and deliveryTime will be returned for each notification)
        /// </summary>
        /// <returns>Scheduled toast notifications</returns>
        public static List<WSAScheduledToastNotification> GetScheduledToastNotifications()
        {
            var notifications = new List<WSAScheduledToastNotification>();

#if ENABLE_WINMD_SUPPORT
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            for (var i = 0; i < scheduled.Count; i++)
            {
                notifications.Add(new WSAScheduledToastNotification()
                {
                    Id = scheduled[i].Id,
                    Tag = scheduled[i].Tag,
                    DeliveryTime = scheduled[i].DeliveryTime.UtcDateTime
                });
            }
#endif

            return notifications;
        }

        /// <summary>
        /// Remove a scheduled toast notification
        /// </summary>
        /// <param name="id">The id of the notification</param>
        public static void RemoveScheduledToastNotification(string id)
        {
#if ENABLE_WINMD_SUPPORT
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            for (var i = 0; i < scheduled.Count; i++)
            {
                if (scheduled[i].Id == id)
                {
                    notifier.RemoveFromSchedule(scheduled[i]);
                }
            }
#endif
        }

        /// <summary>
        /// Removes a toast notification from the action centre
        /// </summary>
        /// <param name="tag">The tag assigned to the toast notification</param>
        public static void RemoveToastNotification(string tag)
        {
#if ENABLE_WINMD_SUPPORT
            ToastNotificationManager.History.Remove(tag);
#endif
        }

        /// <summary>
        /// Attempts to create a push notification channel - response will be null if it fails
        /// </summary>
        /// <param name="response">The push notification channel</param>
        public static void CreatePushNotificationChannel(Action<WSAPushNotificationChannel> response)
        {
#if ENABLE_WINMD_SUPPORT
            CreatePushNotificationChannelAsync(response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
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
#if ENABLE_WINMD_SUPPORT
            SendPushNotificationUriToServerAsync(serverUrl, channelUri, authorisation, response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
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