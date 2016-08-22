using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Advertising.WinRT.UI;

namespace CI.WSANative.Advertising
{
    public class MediatorAdManager
    {
        public Action AdRefreshed { get; set; }
        public Action ErrorOccured { get; set; }

        private Grid _container;
        private SwapChainPanel _dxSwapChainPannel;
        private DispatcherTimer _adRefreshTimer;
        private Random _randomGenerator;
        private int _errorCountCurrentRefresh;
        private AdControl _microsoftBanner;
        private AdDuplex.AdControl _AdDuplexBanner;

        private int _adWidth;
        private int _adHeight;
        private string _wApplicationId;
        private string _wAdUnitId;
        private string _adDuplexAppKey;
        private string _adDuplexAdUnitId;
        private int _adDuplexWeight;

        private const int MAX_ERRORS_PER_REFRESH = 3;
        private const int AD_REFRESH_SECONDS = 35;

        public void Initialise(SwapChainPanel dxSwapChainPannel, int width, int height, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment,
            string wApplicationId, string wAdUnitId, string adDuplexAppKey, string adDuplexAdUnitId, int adDuplexWeight)
        {
            _dxSwapChainPannel = dxSwapChainPannel;
            _adWidth = width;
            _adHeight = height;
            _wApplicationId = wApplicationId;
            _wAdUnitId = wAdUnitId;
            _adDuplexAppKey = adDuplexAppKey;
            _adDuplexAdUnitId = adDuplexAdUnitId;
            _adDuplexWeight = adDuplexWeight;

            _container = new Grid();
            _container.HorizontalAlignment = horizontalAlignment;
            _container.VerticalAlignment = verticalAlignment;

            _dxSwapChainPannel.Children.Add(_container);

            _errorCountCurrentRefresh = 0;
            _randomGenerator = new Random();

            _adRefreshTimer = new DispatcherTimer();
            _adRefreshTimer.Interval = new TimeSpan(0, 0, AD_REFRESH_SECONDS);
            _adRefreshTimer.Tick += (s, e) => { RefreshBanner(); };
            _adRefreshTimer.Start();

            RefreshBanner();
        }

        public void SetVisiblity(bool visible)
        {
            if (_container != null)
            {
                _container.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Destroy()
        {
            if (_dxSwapChainPannel != null && _container != null)
            {
                foreach (UIElement child in _dxSwapChainPannel.Children)
                {
                    if (child == _container)
                    {
                        _dxSwapChainPannel.Children.Remove(child);
                        _adRefreshTimer.Stop();
                        _adRefreshTimer = null;
                        _microsoftBanner = null;
                        _AdDuplexBanner = null;
                        _container = null;
                        break;
                    }
                }
            }
        }

        private void RefreshBanner()
        {
            _errorCountCurrentRefresh = 0;

            int wight = _randomGenerator.Next(0, 100);

            if (wight < _adDuplexWeight)
            {
                ActivateAdDuplexBanner();
            }
            else
            {
                ActivateMicrosoftBanner();
            }
        }

        private void ActivateMicrosoftBanner()
        {
            if (_errorCountCurrentRefresh >= MAX_ERRORS_PER_REFRESH)
            {
                _container.Visibility = Visibility.Collapsed;
                return;
            }

            if (_AdDuplexBanner != null)
            {
                _AdDuplexBanner.Visibility = Visibility.Collapsed;
            }

            if (_microsoftBanner == null)
            {
                _microsoftBanner = new AdControl();
                _microsoftBanner.ApplicationId = _wApplicationId;
                _microsoftBanner.AdUnitId = _wAdUnitId;
                _microsoftBanner.Width = _adWidth;
                _microsoftBanner.Height = _adHeight;
                _microsoftBanner.IsAutoRefreshEnabled = false;

                _microsoftBanner.AdRefreshed += (s,e) => { RaiseEvent(AdRefreshed); };
                _microsoftBanner.ErrorOccurred += (s,e) => { _errorCountCurrentRefresh++; ActivateAdDuplexBanner(); RaiseEvent(ErrorOccured); };

                _container.Children.Add(_microsoftBanner);
            }

            _microsoftBanner.Visibility = Visibility.Visible;
            _microsoftBanner.Refresh();
        }

        private void ActivateAdDuplexBanner()
        {
            if (_errorCountCurrentRefresh >= MAX_ERRORS_PER_REFRESH)
            {
                _container.Visibility = Visibility.Collapsed;
                return;
            }

            if (_microsoftBanner != null)
            {
               _microsoftBanner.Visibility = Visibility.Collapsed;
            }

            if (_AdDuplexBanner == null)
            {
                _AdDuplexBanner = new AdDuplex.AdControl();
                _AdDuplexBanner.AppKey = _adDuplexAppKey;
                _AdDuplexBanner.AdUnitId = _adDuplexAdUnitId;
                _AdDuplexBanner.Width = _adWidth;
                _AdDuplexBanner.Height = _adHeight;
                _AdDuplexBanner.RefreshInterval = AD_REFRESH_SECONDS;

                _AdDuplexBanner.AdLoaded += (s,e) => { RaiseEvent(AdRefreshed); };
                _AdDuplexBanner.AdCovered += (s, e) => { _errorCountCurrentRefresh++; ActivateMicrosoftBanner(); RaiseEvent(ErrorOccured); };
                _AdDuplexBanner.AdLoadingError += (s, e) => { _errorCountCurrentRefresh++; ActivateMicrosoftBanner(); RaiseEvent(ErrorOccured); };
                _AdDuplexBanner.NoAd += (s, e) => { _errorCountCurrentRefresh++; ActivateMicrosoftBanner(); RaiseEvent(ErrorOccured); };

                _container.Children.Add(_AdDuplexBanner);
            }

            _AdDuplexBanner.Visibility = Visibility.Visible;
        }

        private void RaiseEvent(Action action)
        {
            if(action != null)
            {
                action();
            }
        }
    }
}