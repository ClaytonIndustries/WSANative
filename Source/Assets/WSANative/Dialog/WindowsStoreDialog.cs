////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Popups;
using CI.WSANative.Common;

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
             ThreadRunner.RunOnUIThread(async () =>
             {
                var messageDialog = new MessageDialog(message, title);
                messageDialog.Commands.Add(new UICommand("Ok"));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            });
        }

        public void ShowDialogWithOptions(string title, string message)
        {
            ThreadRunner.RunOnUIThread(async () =>
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
            ThreadRunner.RunOnUIThread(async () =>
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
            ThreadRunner.RunOnAppThread(() =>
            {
                if (OnComplete != null)
                {
                    OnComplete(new WSADialogResult(command.Label));
                }
            });
        }
    }
}
#endif