using System;
using System.Collections.Generic;

namespace CI.WSANative.Store
{
    public class WSAStoreProduct
    {
        public string Description { get; set; }
        public List<Uri> Images { get; set; }
        public string InAppOfferToken { get; set; }
        public bool IsInUserCollection { get; set; }
        public string Language { get; set; }
        public Uri LinkUri { get; set; }
        public string FormattedPrice { get; set; }
        public string ProductKind { get; set; }
        public string StoreId { get; set; }
        public string Title { get; set; }
        public List<Uri> Videos { get; set; }
        public WSAStoreSubscriptionInfo SubscriptionInfo { get; set; }
    }
}