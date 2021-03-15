////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using Windows.Networking.PushNotifications;
#endif

using System;

namespace CI.WSANative.Notification
{
    public class WSAPushNotificationChannel
    {
#if ENABLE_WINMD_SUPPORT
        private PushNotificationChannel _pushNotificationChannel;

        public WSAPushNotificationChannel(PushNotificationChannel pushNotificationChannel)
        {
            _pushNotificationChannel = pushNotificationChannel;
            _pushNotificationChannel.PushNotificationReceived += (s,e) => { if(PushNotificationReceived != null) { PushNotificationReceived(); } };
        }
#endif

        /// <summary>
        /// Raised when a push notification is received on this channel
        /// </summary>
        public Action PushNotificationReceived;

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) to which an app server sends a push notification, this should be sent to your server and stored
        /// </summary>
        public string Uri
        {
            get
            {
#if ENABLE_WINMD_SUPPORT
                return _pushNotificationChannel.Uri;
#else
                return string.Empty;
#endif
            }
        }

        /// <summary>
        /// Gets the time at which the channel expires. Notifications sent to this channel after this time are rejected. Channels automatically expire after 30 days
        /// </summary>
        public DateTimeOffset ExpirationTime
        {
            get
            {
#if ENABLE_WINMD_SUPPORT
                return _pushNotificationChannel.ExpirationTime;
#else
                return DateTimeOffset.MinValue;
#endif
            }
        }

        /// <summary>
        /// Explicitly invalidates this channel. Any notifications pushed to this channel after this method is called are not delivered
        /// </summary>
        public void Close()
        {
#if ENABLE_WINMD_SUPPORT
            _pushNotificationChannel.Close();
#endif
        }
    }
}