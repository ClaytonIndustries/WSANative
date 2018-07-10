using System;

namespace CI.WSANative.StoreNew
{
    public class WSAStoreLicense
    {
        public DateTimeOffset ExpirationDate { get; set; }
        public string InAppOfferToken { get; set; }
        public bool IsActive { get; set; }
        public string StoreId { get; set; }
    }
}