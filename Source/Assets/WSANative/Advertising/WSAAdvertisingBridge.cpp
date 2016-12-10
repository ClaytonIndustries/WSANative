#include "WSAAdvertisingBridge.h"

// Banner Ad Start

void (*_BannerAdInitialiseAction)(AdCallbackWithAdType,AdCallbackWithAdType);

extern "C" void __stdcall _BannerAdInitialise(AdCallbackWithAdType refreshed, AdCallbackWithAdType error)
{
	_BannerAdInitialiseAction(refreshed, error);
}

void (*_BannerAdCreateAction)(wchar_t*,wchar_t*,wchar_t*,int,int,wchar_t*,wchar_t*);

extern "C" void __stdcall _BannerAdCreate(wchar_t* adType, wchar_t* appId, wchar_t* adUnitId, int width, int height, 
	wchar_t* verticalPlacement, wchar_t* horizontalPlacement)
{
	_BannerAdCreateAction(adType, appId, adUnitId, width, height, verticalPlacement, horizontalPlacement);
}

void (*_BannerAdSetVisibilityAction)(wchar_t*,bool);

extern "C" void __stdcall _BannerAdSetVisibility(wchar_t* adType, bool visible)
{
	_BannerAdSetVisibilityAction(adType, visible);
}

void (*_BannerAdDestroyAction)(wchar_t*);

extern "C" void __stdcall _BannerAdDestroy(wchar_t* adType)
{
	_BannerAdDestroyAction(adType);
}

// Banner Ad End

// Interstitial Ad Start

void (*_InterstitialAdInitialiseAction)(AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdType);

extern "C" void __stdcall _InterstitialAdInitialise(AdCallbackWithAdType ready, AdCallbackWithAdType cancelled, AdCallbackWithAdType completed, AdCallbackWithAdType error)
{
	_InterstitialAdInitialiseAction(ready, cancelled, completed, error);
}

void (*_InterstitialAdRequestAction)(wchar_t*,wchar_t*,wchar_t*);

extern "C" void __stdcall _InterstitialAdRequest(wchar_t* adType, wchar_t* appId, wchar_t* adUnitId)
{
	_InterstitialAdRequestAction(adType, appId, adUnitId);
}

void (*_InterstitialAdShowAction)(wchar_t*);

extern "C" void __stdcall _InterstitialAdShow(wchar_t* adType)
{
	_InterstitialAdShowAction(adType);
}

// Interstitial Ad End