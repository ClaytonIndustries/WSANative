using System;

namespace CI.WSANative.StoreNew
{
    public class WSAStorePurchaseResult
    {
        public WSAStorePurchaseStatus Status { get; set; }
        public Exception Error { get; set; }
    }
}