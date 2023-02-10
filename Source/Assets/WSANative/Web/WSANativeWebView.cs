////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using CI.WSANative.Common;

#if ENABLE_WINMD_SUPPORT
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
        /// Raised when the web view has finished loading the current content or if navigation has failed
        /// </summary>
        public static Action<Uri, bool> NavigationCompleted
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

#if ENABLE_WINMD_SUPPORT
        private static WebView _webView;
#endif

        /// <summary>
        /// Create and show a web view
        /// </summary>
        /// <param name="settings">Settings to configure the webview</param>
        public static void Create(WSAWebViewSettings settings)
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _webView == null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _webView = new WebView()
                    {
                        HorizontalAlignment = (HorizontalAlignment)settings.HorizontalPlacement,
                        VerticalAlignment = (VerticalAlignment)settings.VerticalPlacement,
                        Width = settings.HorizontalPlacement == WSAHorizontalPlacement.Stretch ? double.NaN : settings.Width,
                        Height = settings.VerticalPlacement == WSAVerticalPlacement.Stretch ? double.NaN : settings.Height,
                        Margin = settings.Margin != null ? new Thickness(settings.Margin.Left, settings.Margin.Top, settings.Margin.Right, settings.Margin.Bottom) : new Thickness()
                    };

                    _webView.NavigationStarting += (s, e) => { if (NavigationStarting != null) { NavigationStarting(e.Uri); } };
                    _webView.UnviewableContentIdentified += (s, e) => { if (UnviewableContentIdentified != null) { UnviewableContentIdentified(e.Referrer, e.Uri); } };
                    _webView.DOMContentLoaded += (s, e) => { if (DOMContentLoaded != null) { DOMContentLoaded(e.Uri); } };
                    _webView.NavigationCompleted += (s, e) => { if (NavigationCompleted != null) { NavigationCompleted(e.Uri, e.IsSuccess); } };

                    _webView.Navigate(settings.Uri);

                    WSANativeCore.DxSwapChainPanel.Children.Add(_webView);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Destroy the web view if it is currently open
        /// </summary>
        public static void Destroy()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _webView != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    WSANativeCore.DxSwapChainPanel.Children.Remove(_webView);
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
#if ENABLE_WINMD_SUPPORT
            if (_webView != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _webView.Navigate(uri);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Executes the specified script function from the currently loaded HTML, with specific arguments
        /// </summary>
        /// <param name="scriptName">The name of the script function to invoke</param>
        /// <param name="arguments">A collection of strings that packages arguments to the script function</param>
        /// <param name="callback">Raised when the script finishes running and contains the string output of the script</param>
        public static void InvokeScript(string scriptName, IEnumerable<string> arguments, Action<string> callback)
        {
#if ENABLE_WINMD_SUPPORT
            if (_webView != null)
            {
                ThreadRunner.RunOnUIThread(async () =>
                {
                    var result = await _webView.InvokeScriptAsync(scriptName, arguments);
                    if (callback != null)
                    {
                        callback(result);
                    }
                }, false);
            }
#endif
        }
    }
}