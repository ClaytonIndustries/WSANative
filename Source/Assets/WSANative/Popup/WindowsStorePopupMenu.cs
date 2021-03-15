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
using Windows.Foundation;
using Windows.UI.Popups;
using CI.WSANative.Common;

namespace CI.WSANative.Dialogs
{
    public class WindowsStorePopupMenu
    {
        private Action<WSADialogResult> OnComplete;

        public WindowsStorePopupMenu(Action<WSADialogResult> callback)
        {
            OnComplete = callback;
        }

        public void ShowPopupMenu(double xPos, double yPos, List<WSADialogCommand> commands, WSAPopupMenuPlacement placement)
        {
            ThreadRunner.RunOnUIThread(async () =>
            {
                var menu = new PopupMenu();
                if (commands != null)
                {
                    for (int i = 0; i < commands.Count; i++)
                    {
                        menu.Commands.Add(new UICommand(commands[i].ButtonName, new UICommandInvokedHandler(this.CommandInvokedHandler)));
                    }
                }

                await menu.ShowForSelectionAsync(new Rect(new Point(xPos, yPos), new Point(xPos, yPos)), GetPlacement(placement));
            });
        }

        private Placement GetPlacement(WSAPopupMenuPlacement placement)
        {
            switch(placement)
            {
                case WSAPopupMenuPlacement.Above:
                    return Placement.Above;
                case WSAPopupMenuPlacement.Below:
                    return Placement.Below;
                case WSAPopupMenuPlacement.Left:
                    return Placement.Left;
                case WSAPopupMenuPlacement.Right:
                    return Placement.Right;
            }

            return Placement.Above;
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