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
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
#endif

namespace CI.WSANative.Input
{
    public static class WSANativeInput
    {
        /// <summary>
        /// Raised when a pointer is pressed
        /// </summary>
        public static Action<WSAPointerProperties> PointerPressed { get; set; }

        /// <summary>
        /// Raised when a pointer is released
        /// </summary>
        public static Action<WSAPointerProperties> PointerReleased { get; set; }

#if ENABLE_WINMD_SUPPORT
        private static bool _isInitialised;
#endif

        public static void Initialise()
        {
#if ENABLE_WINMD_SUPPORT
            if (!_isInitialised)
            { 
                ThreadRunner.RunOnUIThread(() =>
                {
                    CoreWindow.GetForCurrentThread().PointerPressed += (s, e) =>
                    {
                        if (WSANativeInput.PointerPressed != null)
                        {
                            PointerPointProperties pointerProperties = e.CurrentPoint.Properties;
                            ThreadRunner.RunOnUIThread(() =>
                            {
                                PointerPressed(new WSAPointerProperties()
                                {
                                    InputType = e.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Touch ? WSAInputType.Touch :
                                        e.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Pen ? WSAInputType.Pen : WSAInputType.Mouse,
                                    IsLeftButtonPressed = pointerProperties.IsLeftButtonPressed,
                                    IsRightButtonPressed = pointerProperties.IsRightButtonPressed,
                                    IsEraser = pointerProperties.IsEraser,
                                    IsInverted = pointerProperties.IsInverted
                                });
                            });
                        }
                    };
                    CoreWindow.GetForCurrentThread().PointerReleased += (s, e) =>
                    {
                        if (WSANativeInput.PointerReleased != null)
                        {
                            PointerPointProperties pointerProperties = e.CurrentPoint.Properties;
                            ThreadRunner.RunOnUIThread(() =>
                            {
                                PointerReleased(new WSAPointerProperties()
                                {
                                    InputType = e.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Touch ? WSAInputType.Touch :
                                        e.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Pen ? WSAInputType.Pen : WSAInputType.Mouse,
                                    IsLeftButtonPressed = pointerProperties.IsLeftButtonPressed,
                                    IsRightButtonPressed = pointerProperties.IsRightButtonPressed,
                                    IsEraser = pointerProperties.IsEraser,
                                    IsInverted = pointerProperties.IsInverted
                                });
                            });
                        }
                    };
                    _isInitialised = true;
                });
            }
#endif
        }
    }
}