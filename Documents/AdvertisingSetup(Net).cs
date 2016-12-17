using Microsoft.Advertising.WinRT.UI;
using CI.WSANative.Advertising;

private void ConfigureMicrosoftInterstitalAd()
{
	InterstitialAd interstitialAd = new InterstitialAd();
	interstitialAd.AdReady += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.Microsoft); };
	interstitialAd.ErrorOccurred += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.Microsoft); };
	interstitialAd.Completed += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.Microsoft); };
	interstitialAd.Cancelled += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Cancelled, WSAInterstitialAdType.Microsoft); };
	WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
	{
		if (adType == WSAInterstitialAdType.Microsoft)
		{
			interstitialAd.RequestAd(AdType.Video, appId, adUnitId);
		}
	};
	WSANativeInterstitialAd._Show += (adType) =>
	{
		if (adType == WSAInterstitialAdType.Microsoft && interstitialAd.State == InterstitialAdState.Ready)
		{
			AppCallbacks.Instance.InvokeOnUIThread(() =>
			{
				interstitialAd.Show();
			}, false);
		}
	};
}

private void ConfigureVungleInterstitalAd()
{
	VungleAd interstitialAd = null;
	WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
	{
		if(adType == WSAInterstitialAdType.Vungle && interstitialAd == null)
		{
			interstitialAd = AdFactory.GetInstance(appId);
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
				if(e.Level == DiagnosticLogLevel.Error || e.Level == DiagnosticLogLevel.Fatal)
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
				await interstitialAd.PlayAdAsync(new AdConfig());
			}, false);
		}
	};
}

private void ConfigureAdDuplexInterstitalAd()
{
	InterstitialAd interstitialAd = null;
	WSANativeInterstitialAd._Request += (adType, appId, adUnitId) =>
	{
		if (adType == WSAInterstitialAdType.AdDuplex)
		{
			AppCallbacks.Instance.InvokeOnUIThread(async () =>
			{ 
				if (interstitialAd == null)
				{
					AdDuplexClient.Initialize(appId);
					interstitialAd = new InterstitialAd(adUnitId);
					interstitialAd.AdLoaded += (s, e) =>  { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady, WSAInterstitialAdType.AdDuplex); };
					interstitialAd.AdClosed += (s, e) =>  { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Completed, WSAInterstitialAdType.AdDuplex); };
					interstitialAd.AdLoadingError += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.AdDuplex); };
					interstitialAd.NoAd += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred, WSAInterstitialAdType.AdDuplex); };
				}

				await interstitialAd.LoadAdAsync();
			}, false);
		}
	};
	WSANativeInterstitialAd._Show += (adType) =>
	{
		if (adType == WSAInterstitialAdType.AdDuplex)
		{
			AppCallbacks.Instance.InvokeOnUIThread(async () =>
			{
				await interstitialAd.ShowAdAsync();
			}, false);
		}
	};
}

private void ConfigureBannerAd()
{
    AdControl adControl = null;
    WSANativeBannerAd.Create += (bannerAdSettings) =>
    {
        if (adControl == null)
        {
            adControl = new AdControl();
            adControl.ApplicationId = bannerAdSettings.AppId;
            adControl.AdUnitId = bannerAdSettings.AdUnitId;
            adControl.Height = bannerAdSettings.Height;
            adControl.VerticalAlignment = bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top 
				: bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
            adControl.HorizontalAlignment = bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
				: bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            adControl.Width = bannerAdSettings.Width;
            adControl.IsAutoRefreshEnabled = true;
            adControl.AdRefreshed += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.AdRefreshed); };
            adControl.IsEngagedChanged += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.IsEngagedChanged); };
            adControl.ErrorOccurred += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred); };
            GetSwapChainPanel().Children.Add(adControl);
        }
    };
    WSANativeBannerAd.SetVisiblity += (visible) =>
    {
        if (adControl != null)
        {
            adControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    };
    WSANativeBannerAd.Destroy += () =>
    {
        if (adControl != null)
        {
            foreach (UIElement child in GetSwapChainPanel().Children)
            {
                if (child == adControl)
                {
                    GetSwapChainPanel().Children.Remove(child);
                    adControl = null;
                    break;
                }
            }
        }
    };
    WSANativeBannerAd.IsShowingAd += () =>
    {
        return adControl.HasAd;
    };
}

private void ConfigureMediatorAd()
{
    AdMediatorControl adMediatorControl = null;
    WSANativeMediatorAd.Create += (mediatorAdSettings) =>
    {
        if (adMediatorControl == null)
        {
            adMediatorControl = new AdMediatorControl();
            adMediatorControl.Id = "AdMediator-Id-D1FDFDA7-EABB-474C-940C-ECA7FBCFF143";
            adMediatorControl.Height = mediatorAdSettings.Height;
            adMediatorControl.VerticalAlignment = mediatorAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top 
				: mediatorAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
            adMediatorControl.HorizontalAlignment = mediatorAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
				: mediatorAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            adMediatorControl.Width = mediatorAdSettings.Width;
            adMediatorControl.AdSdkError += (s, e) => { WSANativeMediatorAd.RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccured); };
            adMediatorControl.AdMediatorFilled += (s, e) => { WSANativeMediatorAd.RaiseActionOnAppThread(WSANativeMediatorAd.AdRefreshed); };
            adMediatorControl.AdMediatorError += (s, e) => { WSANativeMediatorAd.RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccured); };
            GetSwapChainPanel().Children.Add(adMediatorControl);
        }
    };
    WSANativeMediatorAd.SetVisiblity += (visible) =>
    {
        if (adMediatorControl != null)
        {
            adMediatorControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
    };
    WSANativeMediatorAd.Destroy += () =>
    {
        if (adMediatorControl != null)
        {
            foreach (UIElement child in GetSwapChainPanel().Children)
            {
                if (child == adMediatorControl)
                {
                    GetSwapChainPanel().Children.Remove(child);
                    adMediatorControl = null;
                    break;
                }
            }
        }
    };
}

private void ConfigureMediatorAd()
{
	MediatorAdManager manager = new MediatorAdManager();

	WSANativeMediatorAd.Create += (mediatorAdSettings) =>
	{
		VerticalAlignment vertialAlignment = mediatorAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top
			: mediatorAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;

		HorizontalAlignment horizontalAlignment = mediatorAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
			: mediatorAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;

		manager.Initialise(GetSwapChainPanel(), mediatorAdSettings.Width, mediatorAdSettings.Height, vertialAlignment, horizontalAlignment,
			mediatorAdSettings.WAppId, mediatorAdSettings.WAdUnitId, mediatorAdSettings.AdDuplexAppKey, mediatorAdSettings.AdDuplexAdUnitId, mediatorAdSettings.AdDuplexWeight);

		manager.AdRefreshed += () => { WSANativeMediatorAd.RaiseActionOnAppThread(WSANativeMediatorAd.AdRefreshed); };
		manager.ErrorOccured += () => { WSANativeMediatorAd.RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred); };
	};
	WSANativeMediatorAd.SetVisiblity += (visible) =>
	{
		manager.SetVisiblity(visible);
	};
	WSANativeMediatorAd.Destroy += () =>
	{
		manager.Destroy();
	};
}

1) Use Unity to build a windows store visual studio solution
2) Open the solution and add a reference to the ad sdk - use the one from here
3) Build the solution (you don't have to but it resolves any references that visual studio says are missing)
4) Open MainPage.xaml.cs and add the following using statements
using Microsoft.Advertising.WinRT.UI;
using CI.WSANative.Advertising;
using VungleSDK;
5) Copy the below function and add it just below the MainPage constructor
6) Call the function we just added from the bottom of the MainPage constructor
7) Open the apps manifest file and add the internet client capability (you must do this or the ads won't show)