using System;

namespace CI.WSANative.StoreNew
{
    public class WSAStoreConsumableResult
    {
        public int BalanceRemaining { get; set; }
        public Exception Error { get; set; }
        public WSAStoreConsumableStatus Status { get; set; }
        public Guid TrackingId { get; set; }
    }
}