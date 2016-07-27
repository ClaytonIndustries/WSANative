////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookDialog : UserControl
    {
        private readonly WebView _iFrame;

        public FacebookDialog(int width, int height)
        {
            _iFrame = new WebView();

            Height = height;
            Width = width;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;

            AddChild(_iFrame);
        }

        public void InitialiseFeed(string feedBaseUri, string link, string picture, string source, string name, string caption, string description, string redirectUri, Grid parent)
        {
            // Escape all params

            Uri fullFeedUri = new Uri(string.Format("{0}&link={1}", feedBaseUri, Uri.EscapeUriString(link)));

            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.ToString() == redirectUri)
                {
                    parent.Children.Remove(this);
                }
            };

            _iFrame.Navigate(fullFeedUri);

            parent.Children.Add(this);
        }
    }
}
#endif