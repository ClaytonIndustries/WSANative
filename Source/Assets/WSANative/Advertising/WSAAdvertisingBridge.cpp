#include "WSAAdvertisingBridge.h"

// Banner Ad Start

void (*_BannerAdInitialiseAction)(AdCallbackWithAdType,AdCallbackWithAdTypeAndErrorMessage);

extern "C" void __stdcall _BannerAdInitialise(AdCallbackWithAdType refreshed, AdCallbackWithAdTypeAndErrorMessage error)
{
	_BannerAdInitialiseAction(refreshed, error);
}

void (*_BannerAdCreateAction)(wchar_t*,wchar_t*,wchar_t*,int,int,wchar_t*,wchar_t*);

extern "C" void __stdcall _BannerAdCreate(wchar_t* adType, wchar_t* appId, wchar_t* adUnitOrPlacementId, int width, int height,
	wchar_t* verticalPlacement, wchar_t* horizontalPlacement)
{
	_BannerAdCreateAction(adType, appId, adUnitOrPlacementId, width, height, verticalPlacement, horizontalPlacement);
}

void (*_BannerAdSetVisibilityAction)(wchar_t*,bool);

extern "C" void __stdcall _BannerAdSetVisibility(wchar_t* adType, bool visible)
{
	_BannerAdSetVisibilityAction(adType, visible);
}

void (*_BannerAdReconfigureAction)(wchar_t*,int,int,wchar_t*,wchar_t*);

extern "C" void __stdcall _BannerAdReconfigure(wchar_t* adType, int width, int height, wchar_t* verticalPlacement, wchar_t* horizontalPlacement)
{
	_BannerAdReconfigureAction(adType, width, height, verticalPlacement, horizontalPlacement);
}

void (*_BannerAdDestroyAction)(wchar_t*);

extern "C" void __stdcall _BannerAdDestroy(wchar_t* adType)
{
	_BannerAdDestroyAction(adType);
}

// Banner Ad End

// Interstitial Ad Start

void (*_InterstitialAdInitialiseAction)(wchar_t*,wchar_t*,AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdType,AdCallbackWithAdTypeAndErrorMessage);

extern "C" void __stdcall _InterstitialAdInitialise(wchar_t* adType, wchar_t* appId, AdCallbackWithAdType ready, AdCallbackWithAdType cancelled, AdCallbackWithAdType completed, AdCallbackWithAdTypeAndErrorMessage error)
{
	_InterstitialAdInitialiseAction(adType, appId, ready, cancelled, completed, error);
}

void (*_InterstitialAdRequestAction)(wchar_t*,wchar_t*);

extern "C" void __stdcall _InterstitialAdRequest(wchar_t* adType, wchar_t* adUnitOrPlacementId)
{
	_InterstitialAdRequestAction(adType, adUnitOrPlacementId);
}

void (*_InterstitialAdShowAction)(wchar_t*, wchar_t*);

extern "C" void __stdcall _InterstitialAdShow(wchar_t* adType, wchar_t* adUnitOrPlacementId)
{
	_InterstitialAdShowAction(adType, adUnitOrPlacementId);
}

// Interstitial Ad End