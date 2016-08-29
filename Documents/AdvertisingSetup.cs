using Microsoft.Advertising.WinRT.UI;
using CI.WSANative.Advertising;

private void ConfigureInterstitalAd()
{
	InterstitialAd interstitialAd = new InterstitialAd();
	interstitialAd.AdReady += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady); };
	interstitialAd.ErrorOccurred += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOccurred); };
	interstitialAd.Completed += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Completed); };
	interstitialAd.Cancelled += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.Cancelled); };
	WSANativeInterstitialAd.Request += (appId, addUnitId) =>
	{
		interstitialAd.RequestAd(AdType.Video, appId, addUnitId);
	};
	WSANativeInterstitialAd.Show += () =>
	{
		AppCallbacks.Instance.InvokeOnUIThread(() =>
		{
			if (interstitialAd.State == InterstitialAdState.Ready)
			{
				interstitialAd.Show();
			}
		}, false);
	};
	WSANativeInterstitialAd.Close += () =>
	{
		if (interstitialAd.State == InterstitialAdState.Showing)
		{
			interstitialAd.Close();
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
5) Copy the below function and add it just below the MainPage constructor
6) Call the function we just added from the bottom of the MainPage constructor
7) Open the apps manifest file and add the internet client capability (you must do this or the ads won't show)