using Microsoft.Advertising.WinRT.UI;
using CI.WSANative.Advertising;

private void ConfigureInterstitalAd()
{
	InterstitialAd interstitialAd = new InterstitialAd();
	interstitialAd.AdReady += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.AdReady); };
	interstitialAd.ErrorOccurred += (s, e) => { WSANativeInterstitialAd.RaiseActionOnAppThread(WSANativeInterstitialAd.ErrorOcurred); };
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
		if(adControl == null)
		{
			adControl = new AdControl();
			adControl.ApplicationId = bannerAdSettings.AppId;
			adControl.AdUnitId = bannerAdSettings.AdUnitId;
			adControl.Height = bannerAdSettings.Height;
			adControl.VerticalAlignment = bannerAdSettings.Placement == WSAAdPlacement.Top ? VerticalAlignment.Top : VerticalAlignment.Bottom;
			adControl.HorizontalAlignment = HorizontalAlignment.Center;
			adControl.Width = bannerAdSettings.Width;
			adControl.IsAutoRefreshEnabled = true;
			
			adControl.AdRefreshed += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.AdRefreshed); };
			adControl.IsEngagedChanged += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.IsEngagedChanged); };
			adControl.ErrorOccurred += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred); };
			
			GetSwapChainPanel().Children.Add(adControl);
		}
	};
	
	WSANativeBannerAd.Destroy += () =>
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