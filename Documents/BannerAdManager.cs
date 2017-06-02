//Uncomment the ones you want to enable
//#define BAM_AD_DUPLEX_ENABLED
//#define BAM_MICROSOFT_ENABLED

#if BAM_AD_DUPLEX_ENABLED
//Uncomment the correct using
//using AdDuplexAd = AdDuplex;                                    //(Windows 10)
//using AdDuplexAd = AdDuplex.Universal.Controls.Win.XAML;        //(Windows 8.1 Desktop)
//using AdDuplexAd = AdDuplex.Universal.Controls.WinPhone.XAML;   //(Windows 8.1 Mobile)
#endif
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UnityPlayer;

namespace CI.WSANative.Advertising
{
    public static class BannerAdManager
    {
        public static void Initialise(Grid grid)
        {
#if BAM_AD_DUPLEX_ENABLED
            ConfigureAdDuplexBannerAd(grid);
#endif
#if BAM_MICROSOFT_ENABLED
            ConfigureMicrosoftBannerAd(grid);
#endif
        }

#if BAM_AD_DUPLEX_ENABLED
        private static void ConfigureAdDuplexBannerAd(Grid grid)
        {
            AdDuplexAd.AdControl adControl = null;
            WSANativeBannerAd.Create += (bannerAdSettings) =>
            {
                if (bannerAdSettings.AdType == WSABannerAdType.AdDuplex && adControl == null)
                {
                    adControl = new AdDuplexAd.AdControl();
                    adControl.AppKey = bannerAdSettings.AppId;
                    adControl.AdUnitId = bannerAdSettings.AdUnitId;
                    adControl.Width = bannerAdSettings.Width;
                    adControl.Height = bannerAdSettings.Height;
                    adControl.RefreshInterval = 30;
                    adControl.VerticalAlignment = bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top
                        : bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
                    adControl.HorizontalAlignment = bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
                        : bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;
                    adControl.AdLoaded += (s, e) => { RaiseActionOnAppThread(WSANativeBannerAd.AdRefreshed, WSABannerAdType.AdDuplex); };
                    adControl.AdCovered += (s, e) => { RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.AdDuplex, "Ad Covered"); };
                    adControl.AdLoadingError += (s, e) => { RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.AdDuplex, e.Error.Message); };
                    adControl.NoAd += (s, e) => { RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.AdDuplex, e.Message); };
                    grid.Children.Add(adControl);
                }
            };
            WSANativeBannerAd.SetVisiblity += (adType, visible) =>
            {
                if (adType == WSABannerAdType.AdDuplex && adControl != null)
                {
                    adControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                }
            };
			WSANativeBannerAd.Reconfigure += (bannerAdSettings) =>
			{
				if(bannerAdSettings.AdType == WSABannerAdType.AdDuplex && adControl != null)
				{
					adControl.Width = bannerAdSettings.Width;
                    adControl.Height = bannerAdSettings.Height;
					adControl.VerticalAlignment = bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top
                        : bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
                    adControl.HorizontalAlignment = bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
                        : bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;
				}
			};
            WSANativeBannerAd.Destroy += (adType) =>
            {
                if (adType == WSABannerAdType.AdDuplex && adControl != null)
                {
                    foreach (UIElement child in grid.Children)
                    {
                        if (child == adControl)
                        {
                            grid.Children.Remove(child);
                            adControl = null;
                            break;
                        }
                    }
                }
            };
        }
#endif
#if BAM_MICROSOFT_ENABLED
        private static void ConfigureMicrosoftBannerAd(Grid grid)
        {
            Microsoft.Advertising.WinRT.UI.AdControl adControl = null;
            WSANativeBannerAd.Create += (bannerAdSettings) =>
            {
                if (bannerAdSettings.AdType == WSABannerAdType.Microsoft && adControl == null)
                {
                    adControl = new Microsoft.Advertising.WinRT.UI.AdControl();
                    adControl.ApplicationId = bannerAdSettings.AppId;
                    adControl.AdUnitId = bannerAdSettings.AdUnitId;
                    adControl.Width = bannerAdSettings.Width;
                    adControl.Height = bannerAdSettings.Height;
                    adControl.IsAutoRefreshEnabled = true;
                    adControl.VerticalAlignment = bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top
                        : bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
                    adControl.HorizontalAlignment = bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
                        : bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;
                    adControl.AdRefreshed += (s, e) => { RaiseActionOnAppThread(WSANativeBannerAd.AdRefreshed, WSABannerAdType.Microsoft); };
                    adControl.ErrorOccurred += (s, e) => { RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.Microsoft, e.ErrorMessage); };
                    grid.Children.Add(adControl);
                }
            };
            WSANativeBannerAd.SetVisiblity += (adType, visible) =>
            {
                if (adType == WSABannerAdType.Microsoft && adControl != null)
                {
                    adControl.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                }
            };
			WSANativeBannerAd.Reconfigure += (bannerAdSettings) =>
			{
				if(bannerAdSettings.AdType == WSABannerAdType.Microsoft && adControl != null)
				{
					adControl.Width = bannerAdSettings.Width;
                    adControl.Height = bannerAdSettings.Height;
					adControl.VerticalAlignment = bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top
                        : bannerAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;
                    adControl.HorizontalAlignment = bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
                        : bannerAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;
				}
			};
            WSANativeBannerAd.Destroy += (adType) =>
            {
                if (adType == WSABannerAdType.Microsoft && adControl != null)
                {
                    foreach (UIElement child in grid.Children)
                    {
                        if (child == adControl)
                        {
                            grid.Children.Remove(child);
                            adControl = null;
                            break;
                        }
                    }
                }
            };
        }
#endif

        private static void RaiseActionOnAppThread(Action<WSABannerAdType> action, WSABannerAdType adType)
        {
            if(action != null)
            {
                AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    action(adType);
                }, false);
            }
        }

        private static void RaiseActionOnAppThread(Action<WSABannerAdType, string> action, WSABannerAdType adType, string errorMessage)
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