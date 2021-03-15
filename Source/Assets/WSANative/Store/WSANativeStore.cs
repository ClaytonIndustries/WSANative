////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if ENABLE_WINMD_SUPPORT
using System.Linq;
using Windows.Services.Store;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Store
{
    public static class WSANativeStore
    {
        /// <summary>
        /// Raised when the status of the app's license changes (for example, the trial period has expired or the user has purchased the full version of the app)
        /// </summary>
        public static Action<WSAStoreAppLicense> OfflineLicensesChanged { get; set; }

        static WSANativeStore()
        {
#if ENABLE_WINMD_SUPPORT
            StoreContext.GetDefault().OfflineLicensesChanged += delegate
            {
                if (OfflineLicensesChanged != null)
                {
                    OfflineLicensesChanged(GetAppLicense());
                }
            };
#endif
        }

        /// <summary>
        /// Gets license info for the current app, including licenses for add-ons
        /// </summary>
        /// <returns>License info for the current app and its associated add-ons</returns>
        public static WSAStoreAppLicense GetAppLicense()
        {
#if ENABLE_WINMD_SUPPORT
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
#else
            return new WSAStoreAppLicense();
#endif
        }

        /// <summary>
        /// Gets Microsoft Store listing info for the current app
        /// </summary>
        /// <param name="response">A callback containing product info about the current app</param>
        public static void GetAppInfo(Action<WSAStoreProductResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            GetAppInfoAsync(response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
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

        /// <summary>
        /// Gets Microsoft Store listing info for the products that can be purchased from within the current app
        /// </summary>
        /// <param name="response">A callback containing product info about each available add-on</param>
        public static void GetAddOns(Action<WSAStoreProductQueryResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            GetAddOnsAsync(response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
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
                    Videos = y.Value.Videos.Select(x => x.Uri).ToList(),
                    SubscriptionInfo = y.Value.Skus.First().SubscriptionInfo != null ? new WSAStoreSubscriptionInfo()
                    {
                        BillingPeriod = (int)y.Value.Skus.First().SubscriptionInfo.BillingPeriod,
                        BillingPeriodUnit = (WSAStoreDurationUnit)y.Value.Skus.First().SubscriptionInfo.BillingPeriodUnit,
                        HasTrialPeriod = y.Value.Skus.First().SubscriptionInfo.HasTrialPeriod,
                        TrialPeriod = (int)y.Value.Skus.First().SubscriptionInfo.TrialPeriod,
                        TrialPeriodUnit = (WSAStoreDurationUnit)y.Value.Skus.First().SubscriptionInfo.TrialPeriodUnit
                    } : null
                }),
                Error = result.ExtendedError
            };

            if (response != null)
            {
                response(wsaStoreProductQuery);
            }
        }
#endif

        /// <summary>
        /// Requests the purchase for the specified app or add-on and displays the UI that is used to complete the transaction via the Microsoft Store
        /// </summary>
        /// <param name="storeId">The store id of the app or add-on</param>
        /// <param name="response">A callback indicating if the purchase was successful</param>
        public static void RequestPurchase(string storeId, Action<WSAStorePurchaseResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                StorePurchaseResult result = await StoreContext.GetDefault().RequestPurchaseAsync(storeId);

                WSAStorePurchaseResult wsaStorePurchaseResult = new WSAStorePurchaseResult()
                {
                    Error = result.ExtendedError,
                    Status = (WSAStorePurchaseStatus)result.Status
                };

                ThreadRunner.RunOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(wsaStorePurchaseResult);
                    }
                }, true);
            });
#endif
        }

        /// <summary>
        /// Reports a consumable add-on for the current app as fulfilled in the Microsoft Store
        /// </summary>
        /// <param name="storeId">The store id of the add-on</param>
        /// <param name="quantity">The quantity to report as consumed - specify 1 if this is a developer managed consumable</param>
        /// <param name="response">A callback indicating if the action was succesful</param>
        public static void ConsumeAddOn(string storeId, int quantity, Action<WSAStoreConsumableResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            ConsumeAddOnAsync(storeId, quantity, response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
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

        /// <summary>
        /// Gets the remaining balance for the specified consumable add-on for the current app
        /// </summary>
        /// <param name="storeId">The store id of the add-on</param>
        /// <param name="response">A callback containing balance information about the specified consumable add-on</param>
        public static void GetRemainingBalanceForConsumableAddOn(string storeId, Action<WSAStoreConsumableResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            GetRemainingBalanceForConsumableAddOnAsync(storeId, response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
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
}