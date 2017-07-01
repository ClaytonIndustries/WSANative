#pragma once

#define IAM_AD_DUPLEX_ENABLED 0
#define IAM_MICROSOFT_ENABLED 0
#define IAM_VUNGLE_ENABLED 0

#if IAM_AD_DUPLEX_ENABLED
using namespace AdDuplex;
using namespace AdDuplex::Common::Models;
using namespace AdDuplex::Interstitials::Models;
#endif
#if IAM_MICROSOFT_ENABLED
using namespace Microsoft::Advertising::WinRT::UI;
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
#if IAM_MICROSOFT_ENABLED
	static Microsoft::Advertising::WinRT::UI::InterstitialAd^ _microsoftInterstitialAd;
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

const wchar_t* InterstitialAdManager::AD_TYPE_AD_DUPLEX = L"AdDuplex";
const wchar_t* InterstitialAdManager::AD_TYPE_MICROSOFT = L"Microsoft";
const wchar_t* InterstitialAdManager::AD_TYPE_VUNGLE = L"Vungle";

AdCallbackWithAdType InterstitialAdManager::_Ready;
AdCallbackWithAdType InterstitialAdManager::_Cancelled;
AdCallbackWithAdType InterstitialAdManager::_Completed;
AdCallbackWithAdTypeAndErrorMessage InterstitialAdManager::_Error;

#if IAM_AD_DUPLEX_ENABLED
AdDuplex::InterstitialAd^ InterstitialAdManager::_adDuplexInterstitialAd;
#endif
#if IAM_MICROSOFT_ENABLED
Microsoft::Advertising::WinRT::UI::InterstitialAd^ InterstitialAdManager::_microsoftInterstitialAd;
#endif
#if IAM_VUNGLE_ENABLED
VungleAd^ InterstitialAdManager::_vungleInterstitialAd;
#endif

inline void InterstitialAdManager::Initialise()
{
	_InterstitialAdInitialiseAction = [](AdCallbackWithAdType ready, AdCallbackWithAdType cancelled, AdCallbackWithAdType completed, AdCallbackWithAdTypeAndErrorMessage error)
	{
		_Ready = ready;
		_Cancelled = cancelled;
		_Completed = completed;
		_Error = error;
	};
	_InterstitialAdRequestAction = [](wchar_t* adType, wchar_t* adVariant, wchar_t* appId, wchar_t* adUnitId)
	{
#if IAM_AD_DUPLEX_ENABLED
		if (IsAdType(adType, AD_TYPE_AD_DUPLEX))
		{	
			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([appId, adUnitId]()
			{
				if(_adDuplexInterstitialAd == nullptr)
				{
					AdDuplexClient::Initialize(ref new Platform::String(appId));
					_adDuplexInterstitialAd = ref new AdDuplex::InterstitialAd(ref new Platform::String(adUnitId));
					_adDuplexInterstitialAd->AdLoaded += ref new Windows::Foundation::EventHandler<InterstitialAdLoadedEventArgs^>([](Object^ s, InterstitialAdLoadedEventArgs^ e) { _Ready(AD_TYPE_AD_DUPLEX); });
					_adDuplexInterstitialAd->AdClosed += ref new Windows::Foundation::EventHandler<InterstitialAdLoadedEventArgs^>([](Object^ s, InterstitialAdLoadedEventArgs^ e) { _Completed(AD_TYPE_AD_DUPLEX); });
					_adDuplexInterstitialAd->AdLoadingError += ref new Windows::Foundation::EventHandler<AdLoadingErrorEventArgs^>([](Object^ s, AdLoadingErrorEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, L"Ad Loading Error"); });
					_adDuplexInterstitialAd->NoAd += ref new Windows::Foundation::EventHandler<NoAdEventArgs^>([](Object^ s, NoAdEventArgs^ e) { _Error(AD_TYPE_AD_DUPLEX, e->Message->Data()); });
				}
			
				_adDuplexInterstitialAd->LoadAdAsync();
			}), false);	
		}
#endif
#if IAM_MICROSOFT_ENABLED
		if (IsAdType(adType, AD_TYPE_MICROSOFT))
		{
			if (_microsoftInterstitialAd == nullptr)
			{
				_microsoftInterstitialAd = ref new Microsoft::Advertising::WinRT::UI::InterstitialAd();
				_microsoftInterstitialAd->AdReady += ref new Windows::Foundation::EventHandler<Object^>([](Object^ s, Object^ e) { _Ready(AD_TYPE_MICROSOFT); });
				_microsoftInterstitialAd->Cancelled += ref new Windows::Foundation::EventHandler<Object^>([](Object^ s, Object^ e) { _Cancelled(AD_TYPE_MICROSOFT); });
				_microsoftInterstitialAd->Completed += ref new Windows::Foundation::EventHandler<Object^>([](Object^ s, Object^ e) { _Completed(AD_TYPE_MICROSOFT); });
				_microsoftInterstitialAd->ErrorOccurred += ref new Windows::Foundation::EventHandler<AdErrorEventArgs^>([](Object^ s, AdErrorEventArgs^ e) { _Error(AD_TYPE_MICROSOFT, e->ErrorMessage->Data()); });
			}

			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([appId, adUnitId, adVariant]()
			{
				_microsoftInterstitialAd->RequestAd(IsAdVariantDisplay(adVariant) ? AdType::Display : AdType::Video, ref new Platform::String(appId), ref new Platform::String(adUnitId));
			}), false);	
		}
#endif
#if IAM_VUNGLE_ENABLED
		if (IsAdType(adType, AD_TYPE_VUNGLE))
		{
			if (_vungleInterstitialAd == nullptr)
			{
				_vungleInterstitialAd = AdFactory::GetInstance(ref new Platform::String(appId));
				_vungleInterstitialAd->OnAdPlayableChanged += ref new Windows::Foundation::EventHandler<AdPlayableEventArgs^>([](Object^ s, AdPlayableEventArgs^ e)
				{
					if (e->AdPlayable)
					{
						_Ready(AD_TYPE_VUNGLE);
					}
				});
				_vungleInterstitialAd->OnVideoView += ref new Windows::Foundation::EventHandler<AdViewEventArgs^>([](Object^ s, AdViewEventArgs^ e)
				{
					if (e->IsCompletedView)
					{
						_Completed(AD_TYPE_VUNGLE);
					}
					else
					{
						_Cancelled(AD_TYPE_VUNGLE);
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
		}
#endif
	};
	_InterstitialAdShowAction = [](wchar_t* adType)
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
#if IAM_MICROSOFT_ENABLED
		if (IsAdType(adType, AD_TYPE_MICROSOFT) && _microsoftInterstitialAd != nullptr && _microsoftInterstitialAd->State == InterstitialAdState::Ready)
		{
			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([]()
			{
				_microsoftInterstitialAd->Show();
			}), false);
		}
#endif
#if IAM_VUNGLE_ENABLED
		if (IsAdType(adType, AD_TYPE_VUNGLE) && _vungleInterstitialAd != nullptr && _vungleInterstitialAd->AdPlayable)
		{
			_vungleInterstitialAd->PlayAdAsync(ref new AdConfig());
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