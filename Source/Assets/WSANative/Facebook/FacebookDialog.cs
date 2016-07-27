////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookDialog : UserControl
    {
        private readonly WebView _iFrame;
        private readonly Button _closeButton;

        private const string _responseAbsolutePath = "/dialog/return/close";

        public FacebookDialog(int screenWidth, int screenHeight)
        {
            _iFrame = new WebView();
            _iFrame.SetValue(Grid.RowProperty, 0);

            _closeButton = new Button()
            {
                Content = "Close",
                Height = 40,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(0, 1, 0, 0)
            };

            _closeButton.SetValue(Grid.RowProperty, 1);

            int horizontalMargin = (screenWidth / 100) * 2;
            int verticalMargin = (screenHeight / 100) * 5;

            Margin = new Thickness(horizontalMargin, verticalMargin, horizontalMargin, verticalMargin);
            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            Grid container = new Grid()
            {
                Background = new SolidColorBrush(Colors.White)
            };

            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            container.Children.Add(_iFrame);
            container.Children.Add(_closeButton);

            Content = container;
        }

        public void InitialiseFeed(string feedUri, string link, string picture, string source, string name, string caption, string description, Grid parent)
        {
            feedUri = AddParameter(feedUri, "link", link);
            feedUri = AddParameter(feedUri, "picture", picture);
            feedUri = AddParameter(feedUri, "source", source);
            feedUri = AddParameter(feedUri, "name", name);
            feedUri = AddParameter(feedUri, "caption", caption);
            feedUri = AddParameter(feedUri, "description", description);

            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.AbsolutePath == _responseAbsolutePath)
                {
                    parent.Children.Remove(this);
                }
            };

            _iFrame.FrameNavigationCompleted += (s, e) =>
            {
                if(!e.IsSuccess)
                {
                    parent.Children.Remove(this);
                }
            };

            _closeButton.Click += (s, e) =>
            {
                parent.Children.Remove(this);
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