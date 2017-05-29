////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace CI.WSANative.Web
{
    public static class WSANativeWebView
    {
        /// <summary>
        /// Raised when the web view navigates to new content
        /// </summary>
        public static Action<Uri> NavigationStarting
        {
            get; set;
        }

        /// <summary>
        /// Raised when the web view attempts to download an unsupported file - indicates the uri of the page that contains the link to the unviewable 
        /// content and the uri of the content the WebView attempted to load.
        /// </summary>
        public static Action<Uri, Uri> UnviewableContentIdentified
        {
            get; set;
        }

        /// <summary>
        /// Raised when the web view has finished parsing the current HTML content
        /// </summary>
        public static Action<Uri> DOMContentLoaded
        {
            get; set;
        }

#if NETFX_CORE
        private static WebView _webView;
        private static Grid _dxSwapChainPanel;

        public static void Configure(Grid dxSwapChainPanel)
        {
            _dxSwapChainPanel = dxSwapChainPanel;
        }
#endif

        /// <summary>
        /// Create and show a web view
        /// </summary>
        /// <param name="x">X position of the web view (from the top left corner)</param>
        /// <param name="y">Y position of the web view (from the top left corner)</param>
        /// <param name="width">Width of the web view</param>
        /// <param name="height">Height of the web view</param>
        /// <param name="uri">Initial url to navigate to</param>
        public static void Create(int x, int y, int width, int height, Uri uri)
        {
#if NETFX_CORE
            if (_dxSwapChainPanel != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _webView = new WebView()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = width,
                        Height = height,
                        Margin = new Thickness(x, y, 0, 0)
                    };

                    _webView.NavigationStarting += (s, e) => { if (NavigationStarting != null) { NavigationStarting(e.Uri); } };
                    _webView.UnviewableContentIdentified += (s, e) => { if (UnviewableContentIdentified != null) { UnviewableContentIdentified(e.Referrer, e.Uri); } };
                    _webView.DOMContentLoaded += (s, e) => { if (DOMContentLoaded != null) { DOMContentLoaded(e.Uri); } };

                    _webView.Navigate(uri);

                    _dxSwapChainPanel.Children.Add(_webView);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Destroy the web view if it is currently open
        /// </summary>
        public static void Destroy()
        {
#if NETFX_CORE
            if (_dxSwapChainPanel != null && _webView != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _dxSwapChainPanel.Children.Remove(_webView);
                    _webView = null;
                }, true);
            }
#endif
        }

        /// <summary>
        /// Navigate to a url
        /// </summary>
        /// <param name="uri">The url to navigate to</param>
        public static void Navigate(Uri uri)
        {
#if NETFX_CORE
            if (_dxSwapChainPanel != null && _webView != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _webView.Navigate(uri);
                }, true);
            }
#endif
        }
    }
}