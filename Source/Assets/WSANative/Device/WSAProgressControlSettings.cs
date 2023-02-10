////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using CI.WSANative.Common;
using UnityEngine;

namespace CI.WSANative.Device
{
    public class WSAProgressControlSettings
    {
        /// <summary>
        /// Width of the control in pixels - ignored if HorizontalPlacement is set to Stretch
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the control in pixels - ignored if VerticalPlacement is set to Stretch and does not apply to the progress bar
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// X offset from the horizontal placement (default is 0)
        /// </summary>
        public int OffsetX { get; set; }
        /// <summary>
        /// Y offset from the vertical placement (default is 0)
        /// </summary>
        public int OffsetY { get; set; }
        /// <summary>
        /// Horizontal position of the control
        /// </summary>
        public WSAHorizontalPlacement HorizontalPlacement { get; set; }
        /// <summary>
        /// Vertical position of the control
        /// </summary>
        public WSAVerticalPlacement VerticalPlacement { get; set; }
        /// <summary>
        /// Colour of the control
        /// </summary>
        public Color32 Colour { get; set; }
    }
}