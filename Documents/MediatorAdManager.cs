using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#if UNITY_WSA_8_1
//Uncomment the correct using
//using Microsoft.AdMediator.Windows81;			//(Windows 8.1 Desktop)
//using Microsoft.AdMediator.WindowsPhone81;	//(Windows 8.1 Mobile)
#elif UNITY_WSA_10_0
using Microsoft.Advertising.WinRT.UI;
#endif

namespace CI.WSANative.Advertising
{
    public static class MediatorAdManager
    {
#if UNITY_WSA_8_1
        public static void Initialise(SwapChainPanel dxSwapChainPannel) 
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
                    adMediatorControl.AdSdkError += (s, e) => { RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred, e.ErrorDescription); };
                    adMediatorControl.AdMediatorFilled += (s, e) => { RaiseActionOnAppThread(WSANativeMediatorAd.AdRefreshed); };
                    adMediatorControl.AdMediatorError += (s, e) => { RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred, e.Error.Message); };
                    dxSwapChainPannel.Children.Add(adMediatorControl);
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
                    foreach (UIElement child in dxSwapChainPannel.Children)
                    {
                        if (child == adMediatorControl)
                        {
                            dxSwapChainPannel.Children.Remove(child);
                            adMediatorControl = null;
                            break;
                        }
                    }
                }
            };
        }
#elif UNITY_WSA_10_0
        private static Grid _container;
        private static DispatcherTimer _adRefreshTimer;
        private static Random _randomGenerator;
        private static int _errorCountCurrentRefresh;
        private static AdControl _microsoftBanner;
        private static AdDuplex.AdControl _adDuplexBanner;

        private const int MAX_ERRORS_PER_REFRESH = 3;
        private const int AD_REFRESH_SECONDS = 35;

        public static void Initialise(SwapChainPanel dxSwapChainPannel)
        {
            WSANativeMediatorAd.Create += (mediatorAdSettings) =>
            {
                VerticalAlignment verticalAlignment = mediatorAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Top ? VerticalAlignment.Top
                    : mediatorAdSettings.VerticalPlacement == WSAAdVerticalPlacement.Bottom ? VerticalAlignment.Bottom : VerticalAlignment.Center;

                HorizontalAlignment horizontalAlignment = mediatorAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Left ? HorizontalAlignment.Left
                    : mediatorAdSettings.HorizontalPlacement == WSAAdHorizontalPlacement.Right ? HorizontalAlignment.Right : HorizontalAlignment.Center;

                _container = new Grid();
                _container.HorizontalAlignment = horizontalAlignment;
                _container.VerticalAlignment = verticalAlignment;

                CreateMicrosoftAd(mediatorAdSettings.WAppId, mediatorAdSettings.WAdUnitId, mediatorAdSettings.Width, mediatorAdSettings.Height);
                CreateAdDuplexAd(mediatorAdSettings.AdDuplexAppKey, mediatorAdSettings.AdDuplexAdUnitId, mediatorAdSettings.Width, mediatorAdSettings.Height);

                dxSwapChainPannel.Children.Add(_container);

                _errorCountCurrentRefresh = 0;
                _randomGenerator = new Random();

                _adRefreshTimer = new DispatcherTimer();
                _adRefreshTimer.Interval = new TimeSpan(0, 0, AD_REFRESH_SECONDS);
                _adRefreshTimer.Tick += (s, e) => { RefreshBanner(mediatorAdSettings.AdDuplexWeight); };
                _adRefreshTimer.Start();

                RefreshBanner(mediatorAdSettings.AdDuplexWeight);
            };
            WSANativeMediatorAd.SetVisiblity += (visible) =>
            {
                if (_container != null)
                {
                    _container.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                }
            };
            WSANativeMediatorAd.Destroy += () =>
            {
                if (dxSwapChainPannel != null && _container != null)
                {
                    foreach (UIElement child in dxSwapChainPannel.Children)
                    {
                        if (child == _container)
                        {
                            dxSwapChainPannel.Children.Remove(child);
                            _adRefreshTimer.Stop();
                            _adRefreshTimer = null;
                            _microsoftBanner = null;
                            _adDuplexBanner = null;
                            _container = null;
                            break;
                        }
                    }
                }
            };
        }

        private static void CreateMicrosoftAd(string applicationId, string adUnitId, int width, int height)
        {
            _microsoftBanner = new AdControl();
            _microsoftBanner.ApplicationId = applicationId;
            _microsoftBanner.AdUnitId = adUnitId;
            _microsoftBanner.Width = width;
            _microsoftBanner.Height = height;
            _microsoftBanner.IsAutoRefreshEnabled = false;
            _microsoftBanner.Visibility = Visibility.Collapsed;

            _microsoftBanner.AdRefreshed += (s, e) => { RaiseActionOnAppThread(WSANativeMediatorAd.AdRefreshed); };
            _microsoftBanner.ErrorOccurred += (s, e) => { _errorCountCurrentRefresh++; ActivateAdDuplexBanner(); RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred, e.ErrorMessage); };

            _container.Children.Add(_microsoftBanner);
        }

        private static void CreateAdDuplexAd(string applicationId, string adUnitId, int width, int height)
        {
            _adDuplexBanner = new AdDuplex.AdControl();
            _adDuplexBanner.AppKey = applicationId;
            _adDuplexBanner.AdUnitId = adUnitId;
            _adDuplexBanner.Width = width;
            _adDuplexBanner.Height = height;
            _adDuplexBanner.RefreshInterval = AD_REFRESH_SECONDS;
            _adDuplexBanner.Visibility = Visibility.Collapsed;

            _adDuplexBanner.AdLoaded += (s, e) => { RaiseActionOnAppThread(WSANativeMediatorAd.AdRefreshed); };
            _adDuplexBanner.AdCovered += (s, e) => { _errorCountCurrentRefresh++; ActivateMicrosoftBanner(); RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred, "Ad Covered"); };
            _adDuplexBanner.AdLoadingError += (s, e) => { _errorCountCurrentRefresh++; ActivateMicrosoftBanner(); RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred, e.Error.Message); };
            _adDuplexBanner.NoAd += (s, e) => { _errorCountCurrentRefresh++; ActivateMicrosoftBanner(); RaiseActionOnAppThread(WSANativeMediatorAd.ErrorOccurred, e.Message); };

            _container.Children.Add(_adDuplexBanner);
        }

        private static void RefreshBanner(int adDuplexWeight)
        {
            _errorCountCurrentRefresh = 0;

            int wight = _randomGenerator.Next(0, 100);

            if (wight < adDuplexWeight)
            {
                ActivateAdDuplexBanner();
            }
            else
            {
                ActivateMicrosoftBanner();
            }
        }

        private static void ActivateMicrosoftBanner()
        {
            if (_errorCountCurrentRefresh >= MAX_ERRORS_PER_REFRESH)
            {
                _container.Visibility = Visibility.Collapsed;
                return;
            }

            _adDuplexBanner.Visibility = Visibility.Collapsed;

            _microsoftBanner.Visibility = Visibility.Visible;
            _microsoftBanner.Refresh();
        }

        private static void ActivateAdDuplexBanner()
        {
            if (_errorCountCurrentRefresh >= MAX_ERRORS_PER_REFRESH)
            {
                _container.Visibility = Visibility.Collapsed;
                return;
            }

            _microsoftBanner.Visibility = Visibility.Collapsed;

            _adDuplexBanner.Visibility = Visibility.Visible;
        }
#endif

        private static void RaiseActionOnAppThread(Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        private static void RaiseActionOnAppThread(Action<string> action, string errorMessage)
        {
            if (action != null)
            {
                action(errorMessage);
            }
        }
    }
}