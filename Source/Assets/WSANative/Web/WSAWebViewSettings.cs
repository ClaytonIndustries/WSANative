using System;
using CI.WSANative.Common;

namespace CI.WSANative.Web
{
    public class WSAWebViewSettings
    {
        /// <summary>
        /// Width of the webview in pixels - ignored if HorizontalPlacement is set to Stretch
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the webview in pixels - ignored if VerticalPlacement is set to Stretch
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The outer margin around the webview
        /// </summary>
        public WSAMargin Margin { get; set; }
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