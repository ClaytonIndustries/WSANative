using System;

namespace CI.WSANative.Notification
{
    public class WSAScheduledToastNotification : WSAToastNotification
    {
        /// <summary>
        /// Optional id for the notification - this can be used to remove it
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Time that the notification should be shown
        /// </summary>
        public DateTime DeliveryTime { get; set; }
    }
}