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
using Windows.UI.Xaml.Controls;

namespace CI.WSANative.Facebook.Core
{
    public sealed class SendDialog : FacebookDialogBase
    {
        public SendDialog(int screenWidth, int screenHeight)
            : base(screenWidth, screenHeight)
        {
        }

        public void Show(string uri, Dictionary<string, string> parameters, string responseUri, Grid parent, Action closed)
        {
            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.AbsolutePath == responseUri)
                {
                    Close(closed, parent);
                }
            };

            _closeButton.Click += (s, e) =>
            {
                Close(closed, parent);
            };

            Show(uri, parameters, parent);
        }

        private void Close(Action closed, Grid parent)
        {
            parent.Children.Remove(this);

            if (closed != null)
            {
                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    closed();
                }, false);
            }
        }
    }
}
#endif