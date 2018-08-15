////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookLogin : UserControl
    {
        private readonly WebView _iFrame;
        private readonly Button _closeButton;		
		private readonly TaskCompletionSource<string> _taskCompletionSource;

        public FacebookLogin(int screenWidth, int screenHeight)
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
			
			_taskCompletionSource = new TaskCompletionSource<string>();

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

        Uri _uri = null;

        public async Task<string> Show(string requestUri, string responseUri, Grid parent)
        {
            _iFrame.NavigationStarting += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine(e.Uri.AbsoluteUri);

                _uri = e.Uri;

                if (e.Uri.AbsolutePath == responseUri)
                {
                    Match match = Regex.Match(e.Uri.Fragment, "access_token=(.+)&");

                    Close(parent, match.Groups.Count >= 2 ? match.Groups[1].Value : string.Empty);
                }
            };

            _closeButton.Click += (s, e) =>
            {
                Close(parent, string.Empty);
            };

            _iFrame.Navigate(new Uri(requestUri));

            parent.Children.Add(this);

            return await _taskCompletionSource.Task;
        }

        private void Close(Grid parent, string result)
        {
            parent.Children.Remove(this);
			
			_taskCompletionSource.SetResult(result);
        }
    }
}
#endif