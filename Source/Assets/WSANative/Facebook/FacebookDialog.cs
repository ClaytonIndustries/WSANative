////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookDialog : UserControl
    {
        public Action Closed { get; set; }

        private readonly WebView _iFrame;
        private readonly Button _closeButton;

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
                BorderThickness = new Thickness(0, 1, 0, 0),
                Foreground = new SolidColorBrush(Colors.Black)
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

        public void Show(string feedUri, Dictionary<string,string> parameters, string responseUri, Grid parent)
        {
            feedUri = parameters.Aggregate(feedUri, (total, current) => total = AddParameter(total, current.Key, current.Value));

            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.AbsolutePath == responseUri)
                {
                    Close(parent);
                }
            };

            _closeButton.Click += (s, e) =>
            {
                Close(parent);
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

        private void Close(Grid parent)
        {
            parent.Children.Remove(this);

            if(Closed != null)
            {
                Closed();
            }
        }
    }
}
#endif