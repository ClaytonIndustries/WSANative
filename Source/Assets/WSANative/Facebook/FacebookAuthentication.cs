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
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookAuthentication : UserControl
    {
        private readonly WebView _iFrame;
        private readonly Button _closeButton;		
		private readonly TaskCompletionSource<bool> _taskCompletionSource;

        public FacebookAuthentication(int screenWidth, int screenHeight)
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
			
			_taskCompletionSource = new TaskCompletionSource<bool>();

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

        public async Task<bool> Show(string requestUri, string responseUri, Grid parent)
        {
            _iFrame.NavigationStarting += (s, e) =>
            {
                if (e.Uri.AbsolutePath == responseUri)
                {
					// Parse response here to determine if success
				
                    Close(parent, true);
                }
            };

            _closeButton.Click += (s, e) =>
            {
                Close(parent, false);
            };

            _iFrame.Navigate(new Uri(requestUri));

            parent.Children.Add(this);
			
			return _taskCompletionSource.Task;
        }

        private void Close(Grid parent, bool result)
        {
            parent.Children.Remove(this);
			
			_taskCompletionSource.SetResult(result);
        }
    }
}
#endif