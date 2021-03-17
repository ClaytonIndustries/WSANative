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
using Windows.UI.Xaml.Controls;

namespace CI.WSANative.Twitter.Core
{
    public class TwitterWebIntent : TwitterDialogBase
    {
        public TwitterWebIntent(int screenWidth, int screenHeight)
            : base(screenWidth, screenHeight)
        {
        }

        public void Show(string uri, IDictionary<string, string> parameters, Grid parent, Action closed)
        {
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