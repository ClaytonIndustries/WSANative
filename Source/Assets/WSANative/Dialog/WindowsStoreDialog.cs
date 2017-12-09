////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace CI.WSANative.Dialogs
{
    public class WindowsStoreDialog
    {
        private Action<WSADialogResult> OnComplete;

        public WindowsStoreDialog()
        {
        }

        public WindowsStoreDialog(Action<WSADialogResult> callback)
        {
            OnComplete = callback;
        }

        public void ShowDialog(string title, string message)
        {
             UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
             {
                var messageDialog = new MessageDialog(message, title);
                messageDialog.Commands.Add(new UICommand("Ok"));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }, false);
        }

        public void ShowDialogWithOptions(string title, string message)
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var messageDialog = new MessageDialog(message, title);
                messageDialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }, false);
        }

        public void ShowDialogWithOptions(string title, string message, List<WSADialogCommand> commands, int defaultCommandIndex, int cancelCommandIndex)
        {
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                var messageDialog = new MessageDialog(message, title);
                if (commands != null)
                {
                    for (int i = 0; i < commands.Count; i++)
                    {
                        messageDialog.Commands.Add(new UICommand(commands[i].ButtonName, new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    }
                }
                messageDialog.DefaultCommandIndex = (uint)defaultCommandIndex;
                messageDialog.CancelCommandIndex = (uint)cancelCommandIndex;
                await messageDialog.ShowAsync();
            }, false);
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            UnityEngine.WSA.Application.InvokeOnAppThread(() =>
            {
                if (OnComplete != null)
                {
                    OnComplete(new WSADialogResult(command.Label));
                }
            }, false);
        }
    }
}
#endif