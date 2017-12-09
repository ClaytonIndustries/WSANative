////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Storage;
#endif

namespace CI.WSANative.IAPStore
{
    public static class WSANativeStore
    {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static bool _isTest = false;
#endif

        /// <summary>
        /// Configures the store to run in test mode - test mode must be disabled when you publish your app to the store, removing any calls to this function is sufficient
        /// </summary>
        /// <param name="enable">Should test mode be enabled</param>
        public static void EnableTestMode()
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            _isTest = true;
#endif
        }

        /// <summary>
        /// Reloads the simulator with an xml file containing test products - see the documentation for detailed usage
        /// </summary>
        public static void ReloadSimulator()
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                ReloadSimulatorAsync();
            }
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        public static async void ReloadSimulatorAsync()
        {
            StorageFolder installFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile appSimulatorStorageFile = await installFolder.GetFileAsync("WindowsStoreProxy.xml");
            await CurrentAppSimulator.ReloadSimulatorAsync(appSimulatorStorageFile);
        }
#endif

        /// <summary>
        /// Gets the in app products for this app
        /// </summary>
        /// <param name="response">Callback containing the products found</param>
        public static void GetProductListings(Action<List<WSAProduct>> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            GetProductListingsAsync(response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void GetProductListingsAsync(Action<List<WSAProduct>> response)
        {
            ListingInformation listings = null;

            try
            {
                if (_isTest)
                {
                    listings = await CurrentAppSimulator.LoadListingInformationAsync();
                }
                else
                {
                    listings = await CurrentApp.LoadListingInformationAsync();
                }
            }
            catch
            {
            }

            List<WSAProduct> products = new List<WSAProduct>();

            if (listings != null)
            {
                foreach (ProductListing product in listings.ProductListings.Values)
                {
                    products.Add(new WSAProduct()
                    {
                        Id = product.ProductId,
                        Name = product.Name,
#if (UNITY_WSA_10_0 || UNITY_WP8_1)
                        Description = product.Description,
                        ImageUri = product.ImageUri,
#endif
                        FormattedPrice = product.FormattedPrice,
                        ProductType = Enum.GetName(typeof(ProductType), product.ProductType)
                    });
                }
            }

            if (response != null)
            {
                response(products);
            }
        }
#endif

        /// <summary>
        /// Purchase a product
        /// </summary>
        /// <param name="id">The products id</param>
        /// <param name="response">A callback indicating if the action was successful</param>
        public static void PurchaseProduct(string id, Action<WSAPurchaseResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    PurchaseResults purchaseResults = await CurrentAppSimulator.RequestProductPurchaseAsync(id);

                    WSAPurchaseResult result = new WSAPurchaseResult()
                    {
                        OfferId = purchaseResults.OfferId,
                        ReceiptXml = purchaseResults.ReceiptXml,
                        Status = MapPurchaseResult(purchaseResults.Status),
                        TransactionId = purchaseResults.TransactionId
                    };

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(result);
                        }
                    }, true);
                }, false);
            }
            else
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    PurchaseResults purchaseResults = await CurrentApp.RequestProductPurchaseAsync(id);

                    WSAPurchaseResult result = new WSAPurchaseResult()
                    {
                        OfferId = purchaseResults.OfferId,
                        ReceiptXml = purchaseResults.ReceiptXml,
                        Status = MapPurchaseResult(purchaseResults.Status),
                        TransactionId = purchaseResults.TransactionId
                    };

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(result);
                        }
                    }, true);
                }, false);
            }
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static WSAPurchaseResultStatus MapPurchaseResult(ProductPurchaseStatus result)
        {
            switch (result)
            {
                case ProductPurchaseStatus.AlreadyPurchased:
                    return WSAPurchaseResultStatus.AlreadyPurchased;
                case ProductPurchaseStatus.NotFulfilled:
                    return WSAPurchaseResultStatus.NotFulfilled;
                case ProductPurchaseStatus.NotPurchased:
                    return WSAPurchaseResultStatus.NotPurchased;
                case ProductPurchaseStatus.Succeeded:
                    return WSAPurchaseResultStatus.Succeeded;
            }

            return WSAPurchaseResultStatus.NotPurchased;
        }
#endif

        /// <summary>
        /// Reports that your app has fullfilled the consumable product, the product cannot be purchased again until this is called
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="transactionId">The transaction id</param>
        /// <param name="response">A callback indicating the result</param>
        public static void ReportConsumableProductFulfillment(string id, Guid transactionId, Action<WSAFulfillmentResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    FulfillmentResult result = await CurrentAppSimulator.ReportConsumableFulfillmentAsync(id, transactionId);

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(MapFulfillmentResult(result));
                        }
                    }, true);
                }, false);
            }
            else
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    FulfillmentResult result = await CurrentApp.ReportConsumableFulfillmentAsync(id, transactionId);

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(MapFulfillmentResult(result));
                        }
                    }, true);
                }, false);
            }
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static WSAFulfillmentResult MapFulfillmentResult(FulfillmentResult result)
        {
            switch (result)
            {
                case FulfillmentResult.NothingToFulfill:
                    return WSAFulfillmentResult.NothingToFulfill;
                case FulfillmentResult.PurchasePending:
                    return WSAFulfillmentResult.PurchasePending;
                case FulfillmentResult.PurchaseReverted:
                    return WSAFulfillmentResult.PurchaseReverted;
                case FulfillmentResult.ServerError:
                    return WSAFulfillmentResult.ServerError;
                case FulfillmentResult.Succeeded:
                    return WSAFulfillmentResult.Succeeded;
            }

            return WSAFulfillmentResult.NothingToFulfill;
        }
#endif

        /// <summary>
        /// Gets a list of unfulfilled consumable products
        /// </summary>
        /// <param name="response">A callback containing the found unfulfilled consumable products</param>
        public static void GetUnfulfilledConsumableProducts(Action<List<WSAUnfulfilledConsumable>> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    IReadOnlyList<UnfulfilledConsumable> products = await CurrentAppSimulator.GetUnfulfilledConsumablesAsync();

                    List<WSAUnfulfilledConsumable> unfulfilled = products.Select(x => new WSAUnfulfilledConsumable() { OfferId = x.OfferId, ProductId = x.ProductId, TransactionId = x.TransactionId }).ToList();

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(unfulfilled != null ? unfulfilled : new List<WSAUnfulfilledConsumable>());
                        }
                    }, true);
                }, false);
            }
            else
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    IReadOnlyList<UnfulfilledConsumable> products = await CurrentApp.GetUnfulfilledConsumablesAsync();

                    List<WSAUnfulfilledConsumable> unfulfilled = products.Select(x => new WSAUnfulfilledConsumable() { OfferId = x.OfferId, ProductId = x.ProductId, TransactionId = x.TransactionId }).ToList();

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(unfulfilled != null ? unfulfilled : new List<WSAUnfulfilledConsumable>());
                        }
                    }, true);
                }, false);
            }
#endif
        }

        /// <summary>
        /// Requests purchase of your app if it is in trial mode, returns an xml reciept if successful
        /// </summary>
        /// <param name="response">A callback containing the receipt</param>
        public static void PurchaseApp(Action<string> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    string result = await CurrentAppSimulator.RequestAppPurchaseAsync(true);

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(result);
                        }
                    }, true);
                }, true);
            }
            else
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
                {
                    string result = await CurrentApp.RequestAppPurchaseAsync(true);

                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        if (response != null)
                        {
                            response(result);
                        }
                    }, true);
                }, true);
            }
#endif
        }

        /// <summary>
        /// The get license info for your app
        /// </summary>
        /// <returns></returns>
        public static WSAProductLicense GetLicenseForApp()
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                return new WSAProductLicense()
                {
                    ExpirationDate = CurrentAppSimulator.LicenseInformation.ExpirationDate,
                    IsActive = CurrentAppSimulator.LicenseInformation.IsActive,
                    IsTrial = CurrentAppSimulator.LicenseInformation.IsTrial
                };
            }
            else
            {
                return new WSAProductLicense()
                {
                    ExpirationDate = CurrentApp.LicenseInformation.ExpirationDate,
                    IsActive = CurrentApp.LicenseInformation.IsActive,
                    IsTrial = CurrentApp.LicenseInformation.IsTrial
                };
            }
#else
            return new WSAProductLicense();
#endif
        }

        /// <summary>
        /// Get the license info for the specified product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        public static WSAProductLicense GetLicenseForProduct(string id)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            if (_isTest)
            {
                WSAProductLicense license = new WSAProductLicense();

                license.ExpirationDate = CurrentAppSimulator.LicenseInformation.ProductLicenses[id].ExpirationDate;
                license.IsActive = CurrentAppSimulator.LicenseInformation.ProductLicenses[id].IsActive;

                try
                {
#if (UNITY_WSA_10_0 || UNITY_WP8_1)
                    license.IsConsumable = CurrentAppSimulator.LicenseInformation.ProductLicenses[id].IsConsumable;
#endif
                }
                catch
                {
                }

                return license;
            }
            else
            {
                WSAProductLicense license = new WSAProductLicense();

                license.ExpirationDate = CurrentApp.LicenseInformation.ProductLicenses[id].ExpirationDate;
                license.IsActive = CurrentApp.LicenseInformation.ProductLicenses[id].IsActive;

                try
                {
#if (UNITY_WSA_10_0 || UNITY_WP8_1)
                    license.IsConsumable = CurrentApp.LicenseInformation.ProductLicenses[id].IsConsumable;
#endif
                }
                catch
                {
                }

                return license;
            }
#else
            return new WSAProductLicense();
#endif
        }
    }
}