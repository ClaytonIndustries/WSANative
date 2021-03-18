#pragma once

#define BAM_AD_DUPLEX_ENABLED 0

#if BAM_AD_DUPLEX_ENABLED
using namespace AdDuplex::Common::Models;
using namespace AdDuplex::Banners::Core;
using namespace AdDuplex::Banners::Models;
#endif
using namespace Windows::UI::Xaml;
using namespace Windows::UI::Xaml::Controls;

ref class BannerAdManager
{
public:
	static void Initialise(Grid^);
private:
#if BAM_AD_DUPLEX_ENABLED
	static AdDuplex::AdControl^ _adDuplexAdControl;
#endif
	static const wchar_t* AD_TYPE_AD_DUPLEX;
	static const wchar_t* AD_TYPE_MICROSOFT;
	static Grid^ _grid;
	static AdCallbackWithAdType _Refreshed;
	static AdCallbackWithAdTypeAndErrorMessage _Error;
	static VerticalAlignment GetVerticalAlignment(wchar_t*);
	static HorizontalAlignment GetHorizontalAlignment(wchar_t*);
	static bool IsAdType(const wchar_t*, const wchar_t*);
};

#include "pch.h"
#include "WSAAdvertisingBridge.h"

const wchar_t* BannerAdManager::AD_TYPE_AD_DUPLEX = L"AdDuplex";

Grid^ BannerAdManager::_grid;
AdCallbackWithAdType BannerAdManager::_Refreshed;
AdCallbackWithAdTypeAndErrorMessage BannerAdManager::_Error;

#if BAM_AD_DUPLEX_ENABLED
AdDuplex::AdControl^ BannerAdManager::_adDuplexAdControl;
#endif

inline void BannerAdManager::Initialise(Grid^ grid)
{
	_grid = grid;

	_BannerAdInitialiseAction = [](AdCallbackWithAdType refreshed, AdCallbackWithAdTypeAndErrorMessage error)
	{
		_Refreshed = refreshed;
		_Error = error;
	};
	_BannerAdCreateAction = [](wchar_t* adType, wchar_t* appId, wchar_t* adUnitOrPlacementId, int width, int height, wchar_t* verticalPlacement, wchar_t* horizontalPlacement)
	{
#if BAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX) && _adDuplexAdControl == nullptr)
		{
			_adDuplexAdControl = ref new AdDuplex::AdControl();
			_adDuplexAdControl->AppKey = ref new Platform::String(appId);
			_adDuplexAdControl->AdUnitId = ref new Platform::String(adUnitOrPlacementId);
			_adDuplexAdControl->Width = width;
			_adDuplexAdControl->Height = height;
			_adDuplexAdControl->RefreshInterval = 30;
			_adDuplexAdControl->VerticalAlignment = GetVerticalAlignment(verticalPlacement);
			_adDuplexAdControl->HorizontalAlignment = GetHorizontalAlignment(horizontalPlacement);
			_adDuplexAdControl->AdLoaded += ref new Windows::Foundation::EventHandler<BannerAdLoadedEventArgs^>([adUnitOrPlacementId](Object^ s, BannerAdLoadedEventArgs^ e) { _Refreshed(AD_TYPE_AD_DUPLEX, adUnitOrPlacementId); });
			_adDuplexAdControl->AdCovered += ref new Windows::Foundation::EventHandler<AdCoveredEventArgs^>([](Object^ s, AdCoveredEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, L"Ad Covered"); });
			_adDuplexAdControl->AdLoadingError += ref new Windows::Foundation::EventHandler<AdLoadingErrorEventArgs^>([](Object^ s, AdLoadingErrorEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, L"Ad Loading Error"); });
			_adDuplexAdControl->NoAd += ref new Windows::Foundation::EventHandler<NoAdEventArgs^>([](Object^ s, NoAdEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, e->Message->Data()); });
			_grid->Children->Append(_adDuplexAdControl);
		}
#endif
	};
	_BannerAdSetVisibilityAction = [](wchar_t* adType, bool visible)
	{
#if BAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX) && _adDuplexAdControl != nullptr)
		{
			_adDuplexAdControl->Visibility = visible ? Visibility::Visible : Visibility::Collapsed;
		}
#endif
	};
	_BannerAdReconfigureAction = [](wchar_t* adType, int width, int height, wchar_t* verticalPlacement, wchar_t* horizontalPlacement)
	{
#if BAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX) && _adDuplexAdControl != nullptr)
		{
			_adDuplexAdControl->Width = width;
			_adDuplexAdControl->Height = height;
			_adDuplexAdControl->VerticalAlignment = GetVerticalAlignment(verticalPlacement);
			_adDuplexAdControl->HorizontalAlignment = GetHorizontalAlignment(horizontalPlacement);		
		}
#endif
	};
	_BannerAdDestroyAction = [](wchar_t* adType)
	{
#if BAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX) && _adDuplexAdControl != nullptr)
		{
			for (unsigned int i = 0; i < _grid->Children->Size; i++)
			{
				if (_grid->Children->GetAt(i) == _adDuplexAdControl)
				{
					_grid->Children->RemoveAt(i);
					_adDuplexAdControl = nullptr;
					break;
				}
			}
		}
#endif
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

inline bool BannerAdManager::IsAdType(const wchar_t* actual, const wchar_t* expected)
{
	return wcscmp(actual, expected) == 0;
}