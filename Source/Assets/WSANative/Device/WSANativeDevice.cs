////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if (ENABLE_WINMD_SUPPORT && UNITY_WSA_10_0)
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Lights;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

using System;
using CI.WSANative.Common;
using UnityEngine;

namespace CI.WSANative.Device
{
    public static class WSANativeDevice
    {
#if ENABLE_WINMD_SUPPORT
        private static Lamp _lamp;
        private static ProgressBar _progressBar;
        private static ProgressRing _progressRing;
#endif

        /// <summary>
        /// Turns on the flashlight if the device supports it
        /// </summary>
        public static void EnableFlashlight()
        {
#if ENABLE_WINMD_SUPPORT
            EnableFlashlightAsync();
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static async void EnableFlashlightAsync()
        {
            string selectorString = Lamp.GetDeviceSelector();

            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync(selectorString);

            DeviceInformation deviceInfo =
                devices.FirstOrDefault(di => di.EnclosureLocation != null && 
                    di.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);

            if(deviceInfo != null)
            {
                _lamp = await Lamp.FromIdAsync(deviceInfo.Id);

                _lamp.IsEnabled = true;
            }
        }
#endif

        /// <summary>
        /// Turns of the flashlight if it is on
        /// </summary>
        public static void DisableFlashlight()
        {
#if ENABLE_WINMD_SUPPORT
            if (_lamp != null)
            {
                _lamp.IsEnabled = false;
                _lamp.Dispose();
                _lamp = null;
            }
#endif
        }

        /// <summary>
        /// Captures the contents of the screen buffer and converts it to png - you should call this after "yield return new WaitForEndOfFrame();" to ensure that rendering is completed first
        /// </summary>
        /// <returns>An array containing the image data</returns>
        public static byte[] CaptureScreenshot()
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new UnityEngine.Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();
            return texture.EncodeToPNG();
        }

        /// <summary>
        /// Vibrates the device (if supported) for the specified number of seconds. Seconds can be in the range 0 to 5, any other value will throw an exception
        /// </summary>
        /// <param name="seconds">The number of seconds to vibrate for</param>
        public static void Vibrate(int seconds)
        {
#if ENABLE_WINMD_SUPPORT
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.Devices.Notification.VibrationDevice"))
            {
                Windows.Phone.Devices.Notification.VibrationDevice.GetDefault().Vibrate(TimeSpan.FromSeconds(seconds));
            }
#endif
        }

        /// <summary>
        /// Launches the camera capture UI allowing the user to take a picture
        /// </summary>
        /// <param name="imageWidth">Enforce that the image is this wide (specify 0 if the image can be any size)</param>
        /// <param name="imageHeight">Enforce that the image is this high (specify 0 if the image can be any size)</param>
        /// <param name="response">Byte array containing the raw image data or null if no image was captured</param>
        public static void CapturePicture(int imageWidth, int imageHeight, Action<byte[]> response)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                CameraCaptureUI captureUI = new CameraCaptureUI();
                captureUI.PhotoSettings.CroppedSizeInPixels = new Size(imageWidth, imageHeight);
                captureUI.PhotoSettings.AllowCropping = true;
                captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;
                captureUI.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;

                StorageFile file = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

                ThreadRunner.RunOnAppThread(async () =>
                {
                    if (response != null)
                    {
                        response(file != null ? await ReadStorageFile(file): null);
                    }
                }, true);
            });
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static async Task<byte[]> ReadStorageFile(StorageFile file)
        {
            IBuffer buffer = await FileIO.ReadBufferAsync(file);
            DataReader dataReader = DataReader.FromBuffer(buffer);
            byte[] bytes = new byte[buffer.Length];
            dataReader.ReadBytes(bytes);
            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            return bytes;
        }
#endif

        /// <summary>
        /// Create an indeterminate progress bar
        /// </summary>
        /// <param name="settings">Settings to configure the progress bar</param>
        public static void CreateProgressBar(WSAProgressControlSettings settings)
        {

#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                if(WSANativeCore.IsDxSwapChainPanelConfigured() && _progressBar == null)
                {
                    _progressBar = new ProgressBar()
                    {
                        HorizontalAlignment = (HorizontalAlignment)settings.HorizontalPlacement,
                        VerticalAlignment = (VerticalAlignment)settings.VerticalPlacement,
                        IsIndeterminate = true,
                        Width = settings.HorizontalPlacement == WSAHorizontalPlacement.Stretch ? double.NaN : settings.Width,
                        Margin = settings.Margin != null ? new Thickness(settings.Margin.Left, settings.Margin.Top, settings.Margin.Right, settings.Margin.Bottom) : new Thickness(),
                        Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(settings.Colour.a, settings.Colour.r, settings.Colour.g, settings.Colour.b))
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
        /// <param name="settings">Settings to configure the progress ring</param>
        public static void CreateProgressRing(WSAProgressControlSettings settings)
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _progressRing == null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _progressRing = new ProgressRing()
                    {
                        HorizontalAlignment = (HorizontalAlignment)settings.HorizontalPlacement,
                        VerticalAlignment = (VerticalAlignment)settings.VerticalPlacement,
                        IsActive = true,
                        Width = settings.HorizontalPlacement == WSAHorizontalPlacement.Stretch ? double.NaN : settings.Width,
                        Height = settings.VerticalPlacement == WSAVerticalPlacement.Stretch ? double.NaN : settings.Height,
                        Margin = settings.Margin != null ? new Thickness(settings.Margin.Left, settings.Margin.Top, settings.Margin.Right, settings.Margin.Bottom) : new Thickness(),
                        Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(settings.Colour.a, settings.Colour.r, settings.Colour.g, settings.Colour.b))
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