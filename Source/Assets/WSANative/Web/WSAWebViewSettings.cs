using System;
using CI.WSANative.Common;

namespace CI.WSANative.Web
{
    public class WSAWebViewSettings
    {
        /// <summary>
        /// Width of the webview in pixels
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the webview in pixels
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// X offset from the horizontal placement (0 for no offset)
        /// </summary>
        public int OffsetX { get; set; }
        /// <summary>
        /// Y offset from the vertical placement (0 for no offset)
        /// </summary>
        public int OffsetY { get; set; }
        /// <summary>
        /// Horizontal position of the webview
        /// </summary>
        public WSAHorizontalPlacement HorizontalPlacement { get; set; }
        /// <summary>
        /// Vertical position of the webview
        /// </summary>
        public WSAVerticalPlacement VerticalPlacement { get; set; }
        /// <summary>
        /// Initial url to navigate to
        /// </summary>
        public Uri Uri { get; set; }
    }
}