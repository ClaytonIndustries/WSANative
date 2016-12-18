//Uncomment the ones you want to enable
//#define BAM_AD_DUPLEX_ENABLED
//#define BAM_MICROSOFT_ENABLED

#if BAM_AD_DUPLEX_ENABLED
//Uncomment the correct using
//using AdDuplexAd = AdDuplex;                                    //(Windows 10)
//using AdDuplexAd = AdDuplex.Universal.Controls.Win.XAML;        //(Windows 8.1 Desktop)
//using AdDuplexAd = AdDuplex.Universal.Controls.WinPhone.XAML;   //(Windows 8.1 Mobile)
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                    adControl.AdLoaded += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.AdRefreshed, WSABannerAdType.AdDuplex); };
                    adControl.AdCovered += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.AdDuplex); };
                    adControl.AdLoadingError += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.AdDuplex); };
                    adControl.NoAd += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.AdDuplex); };
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
                    adControl.AdRefreshed += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.AdRefreshed, WSABannerAdType.Microsoft); };
                    adControl.ErrorOccurred += (s, e) => { WSANativeBannerAd.RaiseActionOnAppThread(WSANativeBannerAd.ErrorOccurred, WSABannerAdType.Microsoft); };
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
    }
}