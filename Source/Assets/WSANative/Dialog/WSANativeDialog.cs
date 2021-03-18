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
    public static class WSANativeDialog
    {
        /// <summary>
        /// Display an ok dialog with the specified title and message
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        public static void ShowDialog(string title, string message)
        {
#if ENABLE_WINMD_SUPPORT
            new WindowsStoreDialog().ShowDialog(title, message);
#endif
        }

        /// <summary>
        /// Display a yes no dialog with the specified title and message, a callback indicates the users choice
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="response">User response</param>
        public static void ShowDialogWithOptions(string title, string message, Action<WSADialogResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            new WindowsStoreDialog(response).ShowDialogWithOptions(title, message);
#endif
        }

        /// <summary>
        /// Display a dialog with the specified title, message and commands, a callback indicates the users choice
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="message">Dialog message</param>
        /// <param name="commands">Button names (max 3 for desktop) if more than max are added an exception will be thrown</param>
        /// <param name="defaultCommandIndex">The default button, zero based index based on the commands list</param>
        /// <param name="cancelCommandIndex">The default cancel button, zero based index based on the commands list</param>
        /// <param name="response">User response</param>
        public static void ShowDialogWithOptions(string title, string message, List<WSADialogCommand> commands, int defaultCommandIndex, int cancelCommandIndex, Action<WSADialogResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            new WindowsStoreDialog(response).ShowDialogWithOptions(title, message, commands, defaultCommandIndex, cancelCommandIndex);
#endif
        }
    }
}