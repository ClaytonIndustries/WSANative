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

        public void InitialiseFeed(string feedUri, string link, string picture, string source, string name, string caption, string description, string redirectUri, Grid parent)
        {
            feedUri = AddParameter(feedUri, "redirect_url", redirectUri);
            feedUri = AddParameter(feedUri, "link", link);
            feedUri = AddParameter(feedUri, "picture", picture);
            feedUri = AddParameter(feedUri, "source", source);
            feedUri = AddParameter(feedUri, "name", name);
            feedUri = AddParameter(feedUri, "caption", caption);
            feedUri = AddParameter(feedUri, "description", description);

            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.ToString() == redirectUri)
                {
                    parent.Children.Remove(this);
                }
            };

            _iFrame.Navigate(new Uri(feedUri));

            parent.Children.Add(this);
        }

        private string AddParameter(string currentUri, string parameterName, string parameterValue)
        {
            if(!string.IsNullOrEmpty(parameterValue))
            {
                return string.Format("{0}&{1}={2}", currentUri, parameterName, Uri.EscapeUriString(parameterValue));
            }

            return currentUri;
        }
    }
}
#endif