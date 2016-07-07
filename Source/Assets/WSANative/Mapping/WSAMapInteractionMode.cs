////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Mapping
{
    public enum WSAMapInteractionMode
    {
        /// <summary>
        /// Map UI controls and touch input are disabled
        /// </summary>
        Disabled,
        /// <summary>
        /// Map responds to touch and mouse input only
        /// </summary>
        GestureOnly,
        /// <summary>
        /// Map responds to touch and mouse input as well as showing UI controls
        /// </summary>
        GestureAndControl
    }
}