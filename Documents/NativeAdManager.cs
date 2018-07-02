using System;
using System.Linq;
using Microsoft.Advertising.WinRT.UI;
using UnityPlayer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CI.WSANative.Advertising
{
    public static class NativeAdManager
    {
        private static Grid _root;
        private static NativeAdsManagerV2 _nativeAdsManagerV2;
        private static NativeAdV2 _currentAd;
        private static StackPanel _container;
        private static bool _isRegistered;

        public static void Initialise(Grid grid)
        {
            _root = grid;

            _container = new StackPanel();

            WSANativeNativeAd._Request += (appId, adUnitId) =>
            {
                if (_nativeAdsManagerV2 == null)
                {
                    _nativeAdsManagerV2 = new NativeAdsManagerV2(appId, adUnitId);

                    _nativeAdsManagerV2.AdReady += (s, e) =>
                    {
                        _currentAd = e.NativeAd;

                        _isRegistered = false;

                        RaiseActionOnAppThread(WSANativeNativeAd.AdReady, MapNativeAdV2ToWSANativeAd(_currentAd));
                    };

                    _nativeAdsManagerV2.ErrorOccurred += (s, e) =>
                    {
                        RaiseActionOnAppThread(WSANativeNativeAd.ErrorOccurred, $"{e.ErrorCode} - {e.ErrorMessage}");
                    };
                }
                else
                {
                    _nativeAdsManagerV2.RequestAd();
                }
            };

            WSANativeNativeAd._Position += (x, y, width, height) =>
            {
                if (_nativeAdsManagerV2 != null && _currentAd != null)
                {
                    _container.Width = width;
                    _container.Height = height;
                    _container.Margin = new Thickness(x, y, 0, 0);

                    if (!_root.Children.Contains(_container))
                    {
                        _root.Children.Add(_container);
                    }

                    if (!_isRegistered)
                    {
                        _currentAd.RegisterAdContainer(_container);

                        _isRegistered = true;
                    }
                }
            };

            WSANativeNativeAd._Destroy += () =>
            {
                if (_nativeAdsManagerV2 != null && _currentAd != null)
                {
                    if (_root.Children.Contains(_container))
                    {
                        _root.Children.Remove(_container);
                    }

                    _currentAd = null;
                    _isRegistered = false;
                }
            };
        }

        private static WSANativeAd MapNativeAdV2ToWSANativeAd(NativeAdV2 nativeAdV2)
        {
            return new WSANativeAd()
            {
                CallToActionText = nativeAdV2.CallToActionText,
                Description = nativeAdV2.Description,
                Price = nativeAdV2.Price,
                PrivacyUrl = nativeAdV2.PrivacyUrl,
                Rating = nativeAdV2.Rating,
                SponsoredBy = nativeAdV2.SponsoredBy,
                Title = nativeAdV2.Title,
                AdditionalAssets = nativeAdV2.AdditionalAssets.ToDictionary(x => x.Key, y => y.Value),
                IconImage = new WSANativeAdImage()
                {
                    Height = nativeAdV2.IconImage.Height,
                    Width = nativeAdV2.IconImage.Width,
                    Url = nativeAdV2.IconImage.Url
                },
                MainImages = nativeAdV2.MainImages.Select(x => new WSANativeAdImage()
                {
                    Height = nativeAdV2.IconImage.Height,
                    Width = nativeAdV2.IconImage.Width,
                    Url = nativeAdV2.IconImage.Url
                }).ToList()
            };
        }

        private static void RaiseActionOnAppThread(Action<string> action, string errorMessage)
        {
            if (action != null)
            {
                AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    action(errorMessage);
                }, false);
            }
        }

        private static void RaiseActionOnAppThread(Action<WSANativeAd> action, WSANativeAd wsaNativeAd)
        {
            if (action != null)
            {
                AppCallbacks.Instance.InvokeOnAppThread(() =>
                {
                    action(wsaNativeAd);
                }, false);
            }
        }
    }
}