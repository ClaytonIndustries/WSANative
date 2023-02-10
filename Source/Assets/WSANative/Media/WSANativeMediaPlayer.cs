////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using CI.WSANative.Common;

#if ENABLE_WINMD_SUPPORT
using Windows.Media.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif

namespace CI.WSANative.Media
{
    public static class WSANativeMediaPlayer
    {
        /// <summary>
        /// Raised when media is opened
        /// </summary>
        public static Action MediaOpened
        {
            get; set;
        }

        /// <summary>
        /// Raised when media playback has finished
        /// </summary>
        public static Action MediaEnded
        {
            get; set;
        }

        /// <summary>
        /// Raised when an error is encountered
        /// </summary>
        public static Action<string> MediaFailed
        {
            get; set;
        }

        /// <summary>
        /// Gets the duration of the media
        /// </summary>
        public static TimeSpan Duration
        {
            get
            {
                var duration = TimeSpan.Zero;

#if ENABLE_WINMD_SUPPORT
                if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mediaPlayer != null)
                {
                    ThreadRunner.RunOnUIThread(() =>
                    {
                        duration = _mediaPlayer.MediaPlayer.PlaybackSession.NaturalDuration;
                    }, true);
                }
#endif

                return duration;
            }
        }


#if ENABLE_WINMD_SUPPORT
        private static MediaPlayerElement _mediaPlayer;
#endif

        /// <summary>
        /// Create and show a media player
        /// </summary>
        /// <param name="settings">Settings to configure the media player</param>
        public static void Create(WSAMediaPlayerSettings settings)
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mediaPlayer == null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mediaPlayer = new MediaPlayerElement()
                    {
                        HorizontalAlignment = (HorizontalAlignment)settings.HorizontalPlacement,
                        VerticalAlignment = (VerticalAlignment)settings.VerticalPlacement,
                        Width = settings.HorizontalPlacement == WSAHorizontalPlacement.Stretch ? double.NaN : settings.Width,
                        Height = settings.VerticalPlacement == WSAVerticalPlacement.Stretch ? double.NaN : settings.Height,
                        Margin = settings.Margin != null ? new Thickness(settings.Margin.Left, settings.Margin.Top, settings.Margin.Right, settings.Margin.Bottom) : new Thickness(),
                        AreTransportControlsEnabled = settings.AreTransportControlsEnabled,
                        AutoPlay = settings.AutoPlay,
                        IsFullWindow = settings.IsFullWindow,
                        Source = MediaSource.CreateFromUri(settings.Uri)
                    };
 
                    _mediaPlayer.MediaPlayer.MediaOpened += (s, e) => { if (MediaOpened != null) { MediaOpened(); } };
                    _mediaPlayer.MediaPlayer.MediaEnded += (s, e) => { if (MediaEnded != null) { MediaEnded(); } };
                    _mediaPlayer.MediaPlayer.MediaFailed += (s, e) => { if (MediaFailed != null) { MediaFailed(e.ErrorMessage); } };

                    WSANativeCore.DxSwapChainPanel.Children.Add(_mediaPlayer);
                }, true);
            }
#endif
        }

        /// <summary>
        /// Starts playback on the media player
        /// </summary>
        public static void Play()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mediaPlayer != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mediaPlayer.MediaPlayer.Play();
                }, true);
            }
#endif
        }

        /// <summary>
        /// Pauses playback on the media player
        /// </summary>
        public static void Pause()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mediaPlayer != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mediaPlayer.MediaPlayer.Pause();
                }, true);
            }
#endif
        }

        /// <summary>
        /// Set the playback position
        /// </summary>
        /// <param name="position"></param>
        public static void SetPosition(TimeSpan position)
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mediaPlayer != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mediaPlayer.MediaPlayer.PlaybackSession.Position = position;
                }, true);
            }
#endif
        }

        /// <summary>
        /// Destroy the media player if it is currently open
        /// </summary>
        public static void Destroy()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mediaPlayer != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    WSANativeCore.DxSwapChainPanel.Children.Remove(_mediaPlayer);
                    _mediaPlayer.MediaPlayer.Dispose();
                    _mediaPlayer = null;
                }, true);
            }
#endif
        }
    }
}