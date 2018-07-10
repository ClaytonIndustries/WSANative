using System;
using System.Linq;

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using Windows.Services.Store;
#endif

namespace CI.WSANative.StoreNew
{
    public static class WSANativeStore
    {
        public static Action<WSAStoreAppLicense> OfflineLicensesChanged { get; set; }

        static WSANativeStore()
        {
            StoreContext.GetDefault().OfflineLicensesChanged += delegate
            {
                if (OfflineLicensesChanged != null)
                {
                    OfflineLicensesChanged(GetAppLicense());
                }
            };
        }

        public static WSAStoreAppLicense GetAppLicense()
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            var result = StoreContext.GetDefault().GetAppLicenseAsync().AsTask().Result;

            return new WSAStoreAppLicense()
            {
                AddOnLicenses = result.AddOnLicenses.ToDictionary(x => x.Key, y => new WSAStoreLicense()
                {
                    ExpirationDate = y.Value.ExpirationDate,
                    InAppOfferToken = y.Value.InAppOfferToken,
                    IsActive = y.Value.IsActive,
                    StoreId = y.Value.SkuStoreId
                }),
                ExpirationDate = result.ExpirationDate,
                IsActive = result.IsActive,
                IsTrial = result.IsTrial,
                StoreId = result.SkuStoreId,
                TrialTimeRemaining = result.TrialTimeRemaining,
                TrialUniqueId = result.TrialUniqueId
            };
#endif
        }

        public static void GetAppInfo(Action<WSAStoreProductResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            GetAppInfoAsync(response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void GetAppInfoAsync(Action<WSAStoreProductResult> response)
        {
            StoreProductResult result = await StoreContext.GetDefault().GetStoreProductForCurrentAppAsync();

            WSAStoreProductResult wsaStoreProductResult = new WSAStoreProductResult()
            {
                Product = new WSAStoreProduct()
                {
                    Description = result.Product.Description,
                    FormattedPrice = result.Product.Price.FormattedPrice,
                    Images = result.Product.Images.Select(x => x.Uri).ToList(),
                    InAppOfferToken = result.Product.InAppOfferToken,
                    IsInUserCollection = result.Product.IsInUserCollection,
                    Language = result.Product.Language,
                    LinkUri = result.Product.LinkUri,
                    ProductKind = result.Product.ProductKind,
                    StoreId = result.Product.StoreId,
                    Title = result.Product.Title,
                    Videos = result.Product.Videos.Select(x => x.Uri).ToList()
                },
                Error = result.ExtendedError
            };

            if (response != null)
            {
                response(wsaStoreProductResult);
            }
        }
#endif

        public static void GetAddOns(Action<WSAStoreProductQueryResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            GetAddOnsAsync(response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void GetAddOnsAsync(Action<WSAStoreProductQueryResult> response)
        {
            string[] productKinds = { "Durable", "Consumable", "UnmanagedConsumable" };

            StoreProductQueryResult result = await StoreContext.GetDefault().GetAssociatedStoreProductsAsync(productKinds.ToList());

            WSAStoreProductQueryResult wsaStoreProductQuery = new WSAStoreProductQueryResult()
            {
                Products = result.Products.ToDictionary(x => x.Key, y => new WSAStoreProduct()
                {
                    Description = y.Value.Description,
                    FormattedPrice = y.Value.Price.FormattedPrice,
                    Images = y.Value.Images.Select(x => x.Uri).ToList(),
                    InAppOfferToken = y.Value.InAppOfferToken,
                    IsInUserCollection = y.Value.IsInUserCollection,
                    Language = y.Value.Language,
                    LinkUri = y.Value.LinkUri,
                    ProductKind = y.Value.ProductKind,
                    StoreId = y.Value.StoreId,
                    Title = y.Value.Title,
                    Videos = y.Value.Videos.Select(x => x.Uri).ToList()
                }),
                Error = result.ExtendedError
            };

            if (response != null)
            {
                response(wsaStoreProductQuery);
            }
        }
#endif

        public static void PurchaseAddOn(string storeId, Action<WSAStorePurchaseResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                StorePurchaseResult result = await StoreContext.GetDefault().RequestPurchaseAsync(storeId);

                WSAStorePurchaseResult wsaStorePurchaseResult = new WSAStorePurchaseResult()
                {
                    Error = result.ExtendedError,
                    Status = (WSAStorePurchaseStatus)result.Status
                };

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(wsaStorePurchaseResult);
                    }
                }, true);
            }, false);
#endif
        }

        public static void ConsumeAddOn(string storeId, int quantity, Action<WSAStoreConsumableResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            ConsumeAddOnAsync(storeId, quantity, response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void ConsumeAddOnAsync(string storeId, int quantity, Action<WSAStoreConsumableResult> response)
        {
            StoreConsumableResult result = await StoreContext.GetDefault().ReportConsumableFulfillmentAsync(storeId, (uint)quantity, Guid.NewGuid());

            WSAStoreConsumableResult wsaStoreConsumableResult = new WSAStoreConsumableResult()
            {
                BalanceRemaining = (int)result.BalanceRemaining,
                Status = (WSAStoreConsumableStatus)result.Status,
                TrackingId = result.TrackingId,
                Error = result.ExtendedError
            };

            if (response != null)
            {
                response(wsaStoreConsumableResult);
            }
        }
#endif

        public static void GetRemainingBalanceForConsumableAddOn(string storeId, Action<WSAStoreConsumableResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            GetRemainingBalanceForConsumableAddOnAsync(storeId, response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void GetRemainingBalanceForConsumableAddOnAsync(string storeId, Action<WSAStoreConsumableResult> response)
        {
            StoreConsumableResult result = await StoreContext.GetDefault().GetConsumableBalanceRemainingAsync(storeId);

            WSAStoreConsumableResult wsaStoreConsumableResult = new WSAStoreConsumableResult()
            {
                BalanceRemaining = (int)result.BalanceRemaining,
                Status = (WSAStoreConsumableStatus)result.Status,
                TrackingId = result.TrackingId,
                Error = result.ExtendedError
            };

            if (response != null)
            {
                response(wsaStoreConsumableResult);
            }
        }
#endif
    }





    public class WSAStoreProductResult
    {
        public WSAStoreProduct Product { get; set; }
        public Exception Error { get; set; }
    }

    public class WSAStoreProductQueryResult
    {
        public System.Collections.Generic.Dictionary<string, WSAStoreProduct> Products { get; set; }
        public Exception Error { get; set; }
    }
}