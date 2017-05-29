////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace CI.WSANative.Device
{
    public static class WSANativeProgressControl
    {
#if NETFX_CORE
        private static ProgressBar _progressBar;
        private static ProgressRing _progressRing;
        private static Grid _dxSwapChainPanel;

        public static void Configure(Grid dxSwapChainPanel)
        {
            _dxSwapChainPanel = dxSwapChainPanel;
        }
#endif

        /// <summary>
        /// Create an indeterminate progress bar
        /// </summary>
        /// <param name="width">The width of the progress bar</param>
        public static void CreateProgressBar(int width)
        {
#if NETFX_CORE
            if(_dxSwapChainPanel != null && _progressBar == null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _progressBar = new ProgressBar()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        IsIndeterminate = true,
                        Width = width,
                    };

                    _dxSwapChainPanel.Children.Add(_progressBar);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Destroy the progress bar if is it currently active
        /// </summary>
        public static void DestroyProgressBar()
        {
#if NETFX_CORE
            if (_dxSwapChainPanel != null && _progressBar != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _dxSwapChainPanel.Children.Remove(_progressBar);
                    _progressBar = null;
                }, true);
            }
#endif
        }

        /// <summary>
        /// Create an indeterminate progress ring
        /// </summary>
        /// <param name="width">The width of the progress ring</param>
        /// <param name="height">The height of the progress ring</param>
        public static void CreateProgressRing(int width, int height)
        {
#if NETFX_CORE
            if (_dxSwapChainPanel != null && _progressRing == null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _progressRing = new ProgressRing()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = width,
                        Height = height,
                        IsActive = true
                    };

                    _dxSwapChainPanel.Children.Add(_progressRing);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Destroy the progress ring if is it currently active
        /// </summary>
        public static void DestroyProgressRing()
        {
#if NETFX_CORE
            if (_dxSwapChainPanel != null && _progressRing != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    _dxSwapChainPanel.Children.Remove(_progressRing);
                    _progressRing = null;
                }, true);
            }
#endif
        }
    }
}