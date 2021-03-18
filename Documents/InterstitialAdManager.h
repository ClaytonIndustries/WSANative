#pragma once

#define IAM_AD_DUPLEX_ENABLED 0
#define IAM_VUNGLE_ENABLED 0

#if IAM_AD_DUPLEX_ENABLED
using namespace AdDuplex;
using namespace AdDuplex::Common::Models;
using namespace AdDuplex::Interstitials::Models;
#endif
#if IAM_VUNGLE_ENABLED
using namespace VungleSDK;
#endif
using namespace UnityPlayer;

ref class InterstitialAdManager
{
public:
	static void Initialise();
private:
#if IAM_AD_DUPLEX_ENABLED
	static AdDuplex::InterstitialAd^ _adDuplexInterstitialAd;
#endif
#if IAM_VUNGLE_ENABLED
	static VungleAd^ _vungleInterstitialAd;
#endif
	static const wchar_t* AD_TYPE_AD_DUPLEX;
	static const wchar_t* AD_TYPE_MICROSOFT;
	static const wchar_t* AD_TYPE_VUNGLE;
	static AdCallbackWithAdType _Ready;
	static AdCallbackWithAdType _Cancelled;
	static AdCallbackWithAdType _Completed;
	static AdCallbackWithAdTypeAndErrorMessage _Error;
	static bool IsAdType(const wchar_t*, const wchar_t*);
	static bool IsAdVariantDisplay(const wchar_t*);
};

#include "pch.h"
#include "WSAAdvertisingBridge.h"
#include <iterator>
using namespace std;

const wchar_t* InterstitialAdManager::AD_TYPE_AD_DUPLEX = L"AdDuplex";
const wchar_t* InterstitialAdManager::AD_TYPE_VUNGLE = L"Vungle";

AdCallbackWithAdType InterstitialAdManager::_Ready;
AdCallbackWithAdType InterstitialAdManager::_Cancelled;
AdCallbackWithAdType InterstitialAdManager::_Completed;
AdCallbackWithAdTypeAndErrorMessage InterstitialAdManager::_Error;

#if IAM_AD_DUPLEX_ENABLED
AdDuplex::InterstitialAd^ InterstitialAdManager::_adDuplexInterstitialAd;
#endif
#if IAM_VUNGLE_ENABLED
VungleAd^ InterstitialAdManager::_vungleInterstitialAd;
#endif

inline void InterstitialAdManager::Initialise()
{
	_InterstitialAdInitialiseAction = [](wchar_t* adType, wchar_t* appId, AdCallbackWithAdType ready, AdCallbackWithAdType cancelled, AdCallbackWithAdType completed, AdCallbackWithAdTypeAndErrorMessage error)
	{
		_Ready = ready;
		_Cancelled = cancelled;
		_Completed = completed;
		_Error = error;

#if IAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX) && _adDuplexInterstitialAd == nullptr)
		{
			AdDuplexClient::Initialize(ref new Platform::String(appId));
		}
#endif
#if IAM_VUNGLE_ENABLED
		if (IsAdType(adType, AD_TYPE_VUNGLE) && _vungleInterstitialAd == nullptr)
		{
			_vungleInterstitialAd = AdFactory::GetInstance(ref new Platform::String(appId), ref new Platform::Array<Platform::String^>(0));
			_vungleInterstitialAd->OnAdPlayableChanged += ref new Windows::Foundation::EventHandler<AdPlayableEventArgs^>([](Object^ s, AdPlayableEventArgs^ e)
			{
				if (e->AdPlayable)
				{
					_Ready(AD_TYPE_VUNGLE, e->Placement->Data());
				}
			});
			_vungleInterstitialAd->OnAdEnd += ref new Windows::Foundation::EventHandler<AdEndEventArgs^>([](Object^ s, AdEndEventArgs^ e)
			{
				if (e->IsCompletedView || e->CallToActionClicked)
				{
					_Completed(AD_TYPE_VUNGLE, e->Placement->Data());
				}
				else
				{
					_Cancelled(AD_TYPE_VUNGLE, e->Placement->Data());
				}
			});
			_vungleInterstitialAd->Diagnostic += ref new Windows::Foundation::EventHandler<DiagnosticLogEvent^>([](Object^ s, DiagnosticLogEvent^ e)
			{
				if (e->Level == DiagnosticLogLevel::Error || e->Level == DiagnosticLogLevel::Fatal)
				{
					_Error(AD_TYPE_VUNGLE, e->Message->Data());
				}
			});
		}
#endif
	};
	// Lets just have request ad and check if it has been initialised are not
	_InterstitialAdRequestAction = [](wchar_t* adType, wchar_t* adUnitOrPlacementId)
	{
#if IAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX))
		{	
			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([adUnitOrPlacementId]()
			{
				if(_adDuplexInterstitialAd == nullptr)
				{
					_adDuplexInterstitialAd = ref new AdDuplex::InterstitialAd(ref new Platform::String(adUnitOrPlacementId));
					_adDuplexInterstitialAd->AdLoaded += ref new Windows::Foundation::EventHandler<InterstitialAdLoadedEventArgs^>([adUnitOrPlacementId](Object^ s, InterstitialAdLoadedEventArgs^ e) { _Ready(AD_TYPE_AD_DUPLEX, adUnitOrPlacementId); });
					_adDuplexInterstitialAd->AdClosed += ref new Windows::Foundation::EventHandler<InterstitialAdLoadedEventArgs^>([adUnitOrPlacementId](Object^ s, InterstitialAdLoadedEventArgs^ e) { _Completed(AD_TYPE_AD_DUPLEX, adUnitOrPlacementId); });
					_adDuplexInterstitialAd->AdLoadingError += ref new Windows::Foundation::EventHandler<AdLoadingErrorEventArgs^>([](Object^ s, AdLoadingErrorEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, L"Ad Loading Error"); });
					_adDuplexInterstitialAd->NoAd += ref new Windows::Foundation::EventHandler<NoAdEventArgs^>([](Object^ s, NoAdEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, e->Message->Data()); });
				}
			
				_adDuplexInterstitialAd->LoadAdAsync();
			}), false);	
		}
#endif
#if IAM_VUNGLE_ENABLED
		if (IsAdType(adType, AD_TYPE_VUNGLE))
		{
			if (_vungleInterstitialAd != nullptr)
			{
				_vungleInterstitialAd->LoadAd(ref new Platform::String(adUnitOrPlacementId));
			}
		}
#endif
	};
	_InterstitialAdShowAction = [](wchar_t* adType, wchar_t* adUnitOrPlacementId)
	{
#if IAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX) && _adDuplexInterstitialAd != nullptr)
		{
			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([]()
			{
				_adDuplexInterstitialAd->ShowAdAsync();
			}), false);
		}
#endif
#if IAM_VUNGLE_ENABLED
		if (IsAdType(adType, AD_TYPE_VUNGLE) && _vungleInterstitialAd != nullptr && _vungleInterstitialAd->IsAdPlayable(ref new Platform::String(adUnitOrPlacementId)))
		{
			_vungleInterstitialAd->PlayAdAsync(ref new AdConfig(), ref new Platform::String(adUnitOrPlacementId));
		}
#endif
	};
}

inline bool InterstitialAdManager::IsAdType(const wchar_t* actual, const wchar_t* expected)
{
	return wcscmp(actual, expected) == 0;
}

inline bool InterstitialAdManager::IsAdVariantDisplay(const wchar_t* actual)
{
	return wcscmp(actual, L"Display") == 0;
}