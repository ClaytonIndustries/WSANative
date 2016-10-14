PointerPressed="DXSwapChainPanel_PointerPressed" PointerReleased="DXSwapChainPanel_PointerReleased"

private void DXSwapChainPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
{
	PointerPointProperties pointerProperties = e.GetCurrentPoint(DXSwapChainPanel).Properties;

	WSANativeInput.PointerPressed(new WSAPointerProperties()
	{
		InputType = e.Pointer.PointerDeviceType == PointerDeviceType.Touch ? WSAInputType.Touch : 
			e.Pointer.PointerDeviceType == PointerDeviceType.Pen ? WSAInputType.Pen : WSAInputType.Mouse,
		IsLeftButtonPressed = pointerProperties.IsLeftButtonPressed,
		IsRightButtonPressed = pointerProperties.IsRightButtonPressed,
		IsEraser = pointerProperties.IsEraser,
		IsInverted = pointerProperties.IsInverted
	});
}

private void DXSwapChainPanel_PointerReleased(object sender, PointerRoutedEventArgs e)
{
	PointerPointProperties pointerProperties = e.GetCurrentPoint(DXSwapChainPanel).Properties;

	WSANativeInput.PointerPressed(new WSAPointerProperties()
	{
		InputType = e.Pointer.PointerDeviceType == PointerDeviceType.Touch ? WSAInputType.Touch :
			e.Pointer.PointerDeviceType == PointerDeviceType.Pen ? WSAInputType.Pen : WSAInputType.Mouse,
		IsLeftButtonPressed = pointerProperties.IsLeftButtonPressed,
		IsRightButtonPressed = pointerProperties.IsRightButtonPressed,
		IsEraser = pointerProperties.IsEraser,
		IsInverted = pointerProperties.IsInverted
	});
}

using CI.WSANative.Input;
using Windows.Devices.Input;
using Windows.UI.Input;