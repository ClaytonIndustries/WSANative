#pragma once

#define IAM_MICROSOFT_ENABLED 0
#define IAM_VUNGLE_ENABLED 0

#if IAM_MICROSOFT_ENABLED
using namespace Microsoft::Advertising::WinRT::UI;
#endif
#if IAM_VUNGLE_ENABLED
using namespace VungleSDK;
#endif
using namespace UnityPlayer;

ref class IntersitialAdManager
{
public:
	static void Initialise();
private:
#if IAM_MICROSOFT_ENABLED
	static InterstitialAd^ _microsoftInterstitialAd;
#endif
#if IAM_VUNGLE_ENABLED
	static VungleAd^ _vungleInterstitialAd;
#endif
	static const wchar_t* AD_TYPE_MICROSOFT;
	static const wchar_t* AD_TYPE_VUNGLE;
	static AdCallbackWithAdType _Ready;
	static AdCallbackWithAdType _Cancelled;
	static AdCallbackWithAdType _Completed;
	static AdCallbackWithAdType _Error;
	static bool IsAdType(const wchar_t*, const wchar_t*);
};

#include "pch.h"

const wchar_t* IntersitialAdManager::AD_TYPE_MICROSOFT = L"Microsoft";
const wchar_t* IntersitialAdManager::AD_TYPE_VUNGLE = L"Vungle";

AdCallbackWithAdType IntersitialAdManager::_Ready;
AdCallbackWithAdType IntersitialAdManager::_Cancelled;
AdCallbackWithAdType IntersitialAdManager::_Completed;
AdCallbackWithAdType IntersitialAdManager::_Error;

#if IAM_MICROSOFT_ENABLED
InterstitialAd^ IntersitialAdManager::_microsoftInterstitialAd;
#endif
#if IAM_VUNGLE_ENABLED
VungleAd^ IntersitialAdManager::_vungleInterstitialAd;
#endif

inline void IntersitialAdManager::Initialise()
{
	_InterstitialAdInitialiseAction = [](AdCallbackWithAdType ready, AdCallbackWithAdType cancelled, AdCallbackWithAdType completed, AdCallbackWithAdType error)
	{
		_Ready = ready;
		_Cancelled = cancelled;
		_Completed = completed;
		_Error = error;
	};
	_InterstitialAdRequestAction = [](wchar_t* adType, wchar_t* appId, wchar_t* adUnitId)
	{
#if IAM_MICROSOFT_ENABLED
		if (IsAdType(adType, AD_TYPE_MICROSOFT))
		{
			if (_microsoftInterstitialAd == nullptr)
			{
				_microsoftInterstitialAd = ref new InterstitialAd();
				_microsoftInterstitialAd->AdReady += ref new Windows::Foundation::EventHandler<Object^>([](Object^ s, Object^ e) { _Ready(AD_TYPE_MICROSOFT); });
				_microsoftInterstitialAd->Cancelled += ref new Windows::Foundation::EventHandler<Object^>([](Object^ s, Object^ e) { _Cancelled(AD_TYPE_MICROSOFT); });
				_microsoftInterstitialAd->Completed += ref new Windows::Foundation::EventHandler<Object^>([](Object^ s, Object^ e) { _Completed(AD_TYPE_MICROSOFT); });
				_microsoftInterstitialAd->ErrorOccurred += ref new Windows::Foundation::EventHandler<AdErrorEventArgs^>([](Object^ s, AdErrorEventArgs^ e) { _Error(AD_TYPE_MICROSOFT); });
			}

			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([appId, adUnitId]()
			{
				_microsoftInterstitialAd->RequestAd(AdType::Video, ref new Platform::String(appId), ref new Platform::String(adUnitId));
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
						_Error(AD_TYPE_VUNGLE);
					}
				});
			}
		}
#endif
	};
	_InterstitialAdShowAction = [](wchar_t* adType)
	{
#if IAM_MICROSOFT_ENABLED
		if (IsAdType(adType, AD_TYPE_MICROSOFT) && _microsoftInterstitialAd != nullptr)
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
			AppCallbacks::Instance->InvokeOnUIThread(ref new AppCallbackItem([]()
			{
				_vungleInterstitialAd->PlayAdAsync(ref new AdConfig());
			}), false);
		}
#endif
	};
}

inline bool IntersitialAdManager::IsAdType(const wchar_t* actual, const wchar_t* expected)
{
	return wcscmp(actual, expected) == 0;
}