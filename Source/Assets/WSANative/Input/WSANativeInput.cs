////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.Input
{
    public static class WSANativeInput
    {
        /// <summary>
        /// Raised when a pointer is pressed
        /// </summary>
        public static Action<WSAPointerProperties> PointerPressed { get; set; }

        /// <summary>
        /// Raised when a pointer is released
        /// </summary>
        public static Action<WSAPointerProperties> PointerReleased { get; set; }
    }
}