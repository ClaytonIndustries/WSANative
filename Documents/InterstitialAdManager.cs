//Uncomment the ones you want to enable
//#define IAM_AD_DUPLEX_ENABLED
//#define IAM_MICROSOFT_ENABLED
//#define IAM_VUNGLE_ENABLED

using System;
using UnityPlayer;

namespace CI.WSANative.Advertising
{
    public static class InterstitialAdManager
    {
        public static void Initialise()
        {
#if IAM_AD_DUPLEX_ENABLED
            ConfigureAdDuplexInterstitalAd();
#endif
#if IAM_MICROSOFT_ENABLED
            ConfigureMicrosoftInterstitalAd();
#endif
#if IAM_VUNGLE_ENABLED
            ConfigureVungleInterstitalAd();
#endif
        }

#if IAM_AD_DUPLEX_ENABLED
        private static void ConfigureAdDuplexInterstitalAd()
        {
            AdDuplex.InterstitialAd interstitialAd = null;
            WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
            {
                if (adType == WSAInterstitialAdType.AdDuplex)
                {
                    AppCallbacks.Instance.InvokeOnUIThread(async () =>
                    {
                        if (interstitialAd == null)
                        {
                            AdDuplex.AdDuplexClient.Initialize(appId);
							await Task.Delay(500);
                            interstitialAd = new AdDuplex.InterstitialAd(adUnitId);
                            interstitialAd.AdLoaded += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.AdDuplex); };
                            interstitialAd.AdClosed += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.AdDuplex); };
                            interstitialAd.AdLoadingError += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.AdDuplex); };
                            interstitialAd.NoAd += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.AdDuplex); };
                        }

                        await interstitialAd.LoadAdAsync();
                    }, false);
                }
            };
            WSANativeInterstitialAd._Show += (adType) =>
            {
                if (adType == WSAInterstitialAdType.AdDuplex && interstitialAd != null)
                {
                    AppCallbacks.Instance.InvokeOnUIThread(async () =>
                    {
                        await interstitialAd.ShowAdAsync();
                    }, false);
                }
            };
        }
#endif
#if IAM_MICROSOFT_ENABLED
        private static void ConfigureMicrosoftInterstitalAd()
        {
            Microsoft.Advertising.WinRT.UI.InterstitialAd interstitialAd = new Microsoft.Advertising.WinRT.UI.InterstitialAd();
	        interstitialAd.AdReady += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.Microsoft); };
	        interstitialAd.ErrorOccurred += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.Microsoft); };
	        interstitialAd.Completed += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.Microsoft); };
	        interstitialAd.Cancelled += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Cancelled, WSAInterstitialAdType.Microsoft); };
	        WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
	        {
		        if (adType == WSAInterstitialAdType.Microsoft)
		        {
			        interstitialAd.RequestAd(Microsoft.Advertising.WinRT.UI.AdType.Video, appId, adUnitId);
		        }
	        };
	        WSANativeInterstitialAd._Show += (adType) =>
	        {
		        if (adType == WSAInterstitialAdType.Microsoft && interstitialAd != null && interstitialAd.State == Microsoft.Advertising.WinRT.UI.InterstitialAdState.Ready)
		        {
			        AppCallbacks.Instance.InvokeOnUIThread(() =>
			        {
				        interstitialAd.Show();
			        }, false);
		        }
	        };
        }
#endif
#if IAM_VUNGLE_ENABLED
        private static void ConfigureVungleInterstitalAd()
        {
            VungleSDK.VungleAd interstitialAd = null;
	        WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
	        {
		        if(adType == WSAInterstitialAdType.Vungle && interstitialAd == null)
		        {
			        interstitialAd = VungleSDK.AdFactory.GetInstance(appId);
			        interstitialAd.OnAdPlayableChanged += (s, e) =>
			        {
				        if (e.AdPlayable)
				        {
					        WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.Vungle);
				        }
			        };
			        interstitialAd.OnVideoView += (s, e) =>
			        {
				        if (e.IsCompletedView)
				        {
					        WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.Vungle);
				        }
				        else
				        {
					        WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Cancelled, WSAInterstitialAdType.Vungle);
				        }
			        };
			        interstitialAd.Diagnostic += (s, e) =>
			        {
				        if(e.Level == VungleSDK.DiagnosticLogLevel.Error || e.Level == VungleSDK.DiagnosticLogLevel.Fatal)
				        {
					        WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.Vungle);
				        }
			        };
		        }
	        };
	        WSANativeInterstitialAd._Show += (adType) =>
	        {
		        if (adType == WSAInterstitialAdType.Vungle && interstitialAd != null && interstitialAd.AdPlayable)
		        {
			        AppCallbacks.Instance.InvokeOnUIThread(async () =>
			        {
				        await interstitialAd.PlayAdAsync(new VungleSDK.AdConfig());
			        }, false);
		        }
	        };
        }
#endif
    }
}