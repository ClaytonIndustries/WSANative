////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using CI.WSANative.Common; 
#endif

using UnityEngine;

namespace CI.WSANative.Device
{
    public static class WSANativeProgressControl
    {
#if ENABLE_WINMD_SUPPORT
        private static ProgressBar _progressBar;
        private static ProgressRing _progressRing;
#endif

        /// <summary>
        /// Create an indeterminate progress bar
        /// </summary>
        /// <param name="width">The width of the progress bar</param>
        /// <param name="width">The colour of the progress bar</param>
        public static void CreateProgressBar(int width, Color32 colour)
        {

#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                if(WSANativeCore.IsDxSwapChainPanelConfigured() && _progressBar == null)
                {
                    _progressBar = new ProgressBar()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        IsIndeterminate = true,
                        Width = width,
                        Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(colour.a, colour.r, colour.g, colour.b))
                    };

                    WSANativeCore.DxSwapChainPanel.Children.Add(_progressBar);
                }
            }, true);
#endif
        }

        /// <summary>
        /// Destroy the progress bar if it is currently active
        /// </summary>
        public static void DestroyProgressBar()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _progressBar != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    WSANativeCore.DxSwapChainPanel.Children.Remove(_progressBar);
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
        /// <param name="height">The colour of the progress ring</param>
        public static void CreateProgressRing(int width, int height, Color32 colour)
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _progressRing == null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _progressRing = new ProgressRing()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Width = width,
                        Height = height,
                        IsActive = true,
                        Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(colour.a, colour.r, colour.g, colour.b))
                    };

                    WSANativeCore.DxSwapChainPanel.Children.Add(_progressRing);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Destroy the progress ring if it is currently active
        /// </summary>
        public static void DestroyProgressRing()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _progressRing != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    WSANativeCore.DxSwapChainPanel.Children.Remove(_progressRing);
                    _progressRing = null;
                }, true);
            }
#endif
        }
    }
}