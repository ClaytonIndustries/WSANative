//Uncomment the ones you want to enable
//#define IAM_AD_DUPLEX_ENABLED
//#define IAM_MICROSOFT_ENABLED
//#define IAM_VUNGLE_ENABLED

using System;
using System.Threading.Tasks;
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
                            interstitialAd.AdLoaded += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.AdDuplex); };
                            interstitialAd.AdClosed += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.AdDuplex); };
                            interstitialAd.AdLoadingError += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.AdDuplex, e.Error.Message); };
                            interstitialAd.NoAd += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.AdDuplex, e.Message); };
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
	        interstitialAd.AdReady += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.Microsoft); };
	        interstitialAd.ErrorOccurred += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.Microsoft, e.ErrorMessage); };
	        interstitialAd.Completed += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.Microsoft); };
	        interstitialAd.Cancelled += (s, e) => { RaiseActionOnAppThread(WSANativeInterstitialAd.Cancelled, WSAInterstitialAdType.Microsoft); };
	        WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
	        {
		        if (adType == WSAInterstitialAdType.Microsoft)
		        {
                    AppCallbacks.Instance.InvokeOnUIThread(() =>
                    {
                        interstitialAd.RequestAd(Microsoft.Advertising.WinRT.UI.AdType.Video, appId, adUnitId);
                    }, false);
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
					        RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.Vungle);
				        }
			        };
			        interstitialAd.OnVideoView += (s, e) =>
			        {
				        if (e.IsCompletedView)
				        {
					        RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.Vungle);
				        }
				        else
				        {
					        RaiseActionOnAppThread(WSANativeInterstitialAd.Cancelled, WSAInterstitialAdType.Vungle);
				        }
			        };
			        interstitialAd.Diagnostic += (s, e) =>
			        {
				        if(e.Level == VungleSDK.DiagnosticLogLevel.Error || e.Level == VungleSDK.DiagnosticLogLevel.Fatal)
				        {
					        RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.Vungle, e.Message);
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

        private static void RaiseActionOnAppThread(Action<WSAInterstitialAdType> action, WSAInterstitialAdType adType)
        {
            if (action != null)
            {
                AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    action(adType);
                }, false);
            }
        }

        private static void RaiseActionOnAppThread(Action<WSAInterstitialAdType, string> action, WSAInterstitialAdType adType, string errorMessage)
        {
            if (action != null)
            {
                AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    action(adType, errorMessage);
                }, false);
            }
        }
    }
}