////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace CI.WSANative.Dialogs
{
    public static class WSANativePopupMenu
    {
        /// <summary>
        /// Display a popup menu with a specified and options, a callback indicates the users choice - positioning may not work correctly on some platforms
        /// </summary>
        /// <param name="xPos">The xpos to center on in screen co-ordinates</param>
        /// <param name="yPos">The ypos to center on in screen co-ordinates</param>
        /// <param name="commands">Button names (max 6 for desktop) if more than max are added an exception will be thrown</param>
        /// <param name="placement">How the popup menu should be positioned</param>
        /// <param name="response">User response</param>
        public static void ShowPopupMenu(double xPos, double yPos, List<WSADialogCommand> commands, WSAPopupMenuPlacement placement, Action<WSADialogResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            new WindowsStorePopupMenu(response).ShowPopupMenu(xPos, yPos, commands, placement);
#endif
        }
    }
}