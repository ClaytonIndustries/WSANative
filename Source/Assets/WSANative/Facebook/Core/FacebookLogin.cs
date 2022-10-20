////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
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
		private readonly TaskCompletionSource<LoginResult> _taskCompletionSource;

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
			
			_taskCompletionSource = new TaskCompletionSource<LoginResult>();

            VerticalAlignment = VerticalAlignment.Stretch;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            int horizontalMargin = screenWidth / 10;
            int verticalMargin = screenHeight / 10;

            Grid container = new Grid()
            {
                Background = new SolidColorBrush(Colors.White)
            };

            container.Margin = new Thickness(horizontalMargin, verticalMargin, horizontalMargin, verticalMargin);
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            container.Children.Add(_iFrame);
            container.Children.Add(_closeButton);

            Grid background = new Grid()
            {
                Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0))
            };

            background.Children.Add(container);

            Content = background;
        }

        public async Task<LoginResult> Show(string requestUri, string responseUri, Grid parent)
        {
            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.AbsolutePath == responseUri)
                {
                    var accessTokenMatch = Regex.Match(e.Uri.Fragment, "access_token=(.+?)(&|$)");
                    var accessToken = accessTokenMatch.Groups.Count >= 2 ? accessTokenMatch.Groups[1].Value : string.Empty;
                    var accessTokenExpiryMatch = Regex.Match(e.Uri.Fragment, "expires_in=(.+?)(&|$)");
                    int.TryParse(accessTokenExpiryMatch.Groups.Count >= 2 ? accessTokenExpiryMatch.Groups[1].Value : "0", out int accessTokenExpiry);

                    var result = new LoginResult()
                    {
                        AccessToken = accessToken,
                        AccessTokenExpiry = DateTime.Now.AddSeconds(accessTokenExpiry)
                    };

                    Close(parent, result);
                }
            };

            _closeButton.Click += (s, e) =>
            {
                Close(parent, null);
            };

            _iFrame.Navigate(new Uri(requestUri));

            parent.Children.Add(this);

            return await _taskCompletionSource.Task;
        }

        private void Close(Grid parent, LoginResult result)
        {
            parent.Children.Remove(this);
			
			_taskCompletionSource.SetResult(result);
        }
    }
}
#endif