using System;
using CI.WSANative.Common;

namespace CI.WSANative.Media
{
    public class WSAMediaPlayerSettings
    {
        /// <summary>
        /// Width of the media player in pixels - ignored if HorizontalPlacement is set to Stretch
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the media player in pixels - ignored if VerticalPlacement is set to Stretch
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The outer margin around the media player
        /// </summary>
        public WSAMargin Margin { get; set; }
        /// <summary>
        /// Horizontal position of the media player
        /// </summary>
        public WSAHorizontalPlacement HorizontalPlacement { get; set; }
        /// <summary>
        /// Vertical position of the media player
        /// </summary>
        public WSAVerticalPlacement VerticalPlacement { get; set; }
        /// <summary>
        /// Should the standard transport controls be shown
        /// </summary>
        public bool AreTransportControlsEnabled { get; set; }
        /// <summary>
        /// Uri of the media file - supports either a web url or an embedded file (ms-appx:///)
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// Should the media auto play
        /// </summary>
        public bool AutoPlay { get; set; }
        /// <summary>
        /// Should the media player render in full window mode
        /// </summary>
        public bool IsFullWindow { get; set; }
    }
}