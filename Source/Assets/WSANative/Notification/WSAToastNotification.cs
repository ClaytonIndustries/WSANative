using System;

namespace CI.WSANative.Notification
{
    public class WSAToastNotification
    {
        /// <summary>
        /// Title for the notification
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Text for the notification
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Optional tag for the notification - notifications with the same tag will overwrite each other in the action centre
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Optional image for the notification - specified as the location in the built uwp solution e.g ms-appx:///Assets/Square150x150Logo.png
        /// </summary>
        public Uri Image { get; set; }
    }
}