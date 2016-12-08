#pragma once

using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;
using namespace Microsoft::Advertising::WinRT::UI;

ref class BannerAdManager
{
public:
	static void Initialise(Grid^);
private:
	static AdControl^ _adControl;
	static Grid^ _grid;
	static VerticalAlignment GetVerticalAlignment(wchar_t*);
	static HorizontalAlignment GetHorizontalAlignment(wchar_t*);
};

#include "pch.h"
#define GENERATED_PROJECT 1
#include "..\Il2CppOutputProject\il2cppOutput\WSAAdvertisingBridge.h"

AdControl^ BannerAdManager::_adControl;
Grid^ BannerAdManager::_grid;

inline void BannerAdManager::Initialise(Grid^ grid)
{
	_grid = grid;

	_BannerAdCreateAction = [](wchar_t* appId, wchar_t* adUnitId, int width, int height, wchar_t* verticalPlacement, wchar_t* horizontalPlacement, AdCallback refreshed, AdCallback error)
	{
		if (_adControl == nullptr)
		{
			_adControl = ref new AdControl();
			_adControl->ApplicationId = ref new Platform::String(appId);
			_adControl->AdUnitId = ref new Platform::String(adUnitId);
			_adControl->Width = width;
			_adControl->Height = height;
			_adControl->IsAutoRefreshEnabled = true;
			_adControl->VerticalAlignment = GetVerticalAlignment(verticalPlacement);
			_adControl->HorizontalAlignment = GetHorizontalAlignment(horizontalPlacement);

			_adControl->AdRefreshed += ref new Windows::Foundation::EventHandler<RoutedEventArgs^>([refreshed](Object^ s, RoutedEventArgs^ e) { refreshed(); });
			_adControl->ErrorOccurred += ref new Windows::Foundation::EventHandler<AdErrorEventArgs^>([error](Object^ s, AdErrorEventArgs^ e) { error(); });
			_grid->Children->Append(_adControl);
		}
	};
	_BannerAdSetVisibilityAction = [](bool visible)
	{
		if (_adControl != nullptr)
		{
			_adControl->Visibility = visible ? Visibility::Visible : Visibility::Collapsed;
		}
	};
	_BannerAdDestroyAction = []()
	{
		if (_adControl != nullptr)
		{
			for (unsigned int i = 0; i < _grid->Children->Size; i++)
			{
				if (_grid->Children->GetAt(i) == _adControl)
				{
					_grid->Children->RemoveAt(i);
					_adControl = nullptr;
					break;
				}
			}
		}
	};
}

inline VerticalAlignment BannerAdManager::GetVerticalAlignment(wchar_t* alignment)
{
	if (wcscmp(alignment, L"Top") == 0)
	{
		return VerticalAlignment::Top;
	}
	else if (wcscmp(alignment, L"Bottom") == 0)
	{
		return VerticalAlignment::Bottom;
	}
	else
	{
		return VerticalAlignment::Center;
	}
}

inline HorizontalAlignment BannerAdManager::GetHorizontalAlignment(wchar_t* alignment)
{
	if (wcscmp(alignment, L"Left") == 0)
	{
		return HorizontalAlignment::Left;
	}
	else if (wcscmp(alignment, L"Right") == 0)
	{
		return HorizontalAlignment::Right;
	}
	else
	{
		return HorizontalAlignment::Center;
	}
}