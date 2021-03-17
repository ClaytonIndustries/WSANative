////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CI.WSANative.Twitter.Core
{
    public sealed class TwitterLogin : UserControl
    {
        private readonly WebView _iFrame;
        private readonly Button _closeButton;		
		private readonly TaskCompletionSource<string> _taskCompletionSource;

        public TwitterLogin(int screenWidth, int screenHeight)
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

            int horizontalMargin = (screenWidth - 600) / 2;
            int verticalMargin = (screenHeight - 700) / 2;

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

        public async Task<string> Show(string requestUri, string responseUri, Grid parent)
        {
            _iFrame.NavigationStarting += (s, e) =>
            {
                var path = String.Format("{0}{1}{2}{3}", e.Uri.Scheme, Uri.SchemeDelimiter, e.Uri.Authority, e.Uri.AbsolutePath);

                if (path == responseUri)
                {
                    string query = "";

                    if (!string.IsNullOrEmpty(e.Uri.Query))
                    {
                        query = e.Uri.Query.Substring(1);
                    }

                    Close(parent, query);
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