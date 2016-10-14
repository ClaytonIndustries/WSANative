////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Input
{
    public class WSAPointerProperties
    {
        /// <summary>
        /// Which type of device created the input
        /// </summary>
        public WSAInputType InputType
        {
            get; set;
        }

        /// <summary>
        /// Is the left button of a mouse or other input device pressed
        /// </summary>
        public bool IsLeftButtonPressed
        {
            get; set;
        } 

        /// <summary>
        /// Is the right button of a mouse or other input device pressed
        /// </summary>
        public bool IsRightButtonPressed
        {
            get; set;
        }

        /// <summary>
        /// Is the input from a digitizer eraser
        /// </summary>
        public bool IsEraser
        {
            get; set;
        }

        /// <summary>
        /// Is the digitizer pen inverted
        /// </summary>
        public bool IsInverted
        {
            get; set;
        }
    }
}