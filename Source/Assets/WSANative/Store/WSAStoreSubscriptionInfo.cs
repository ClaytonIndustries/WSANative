////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Store
{
    public class WSAStoreSubscriptionInfo
    {
        public int BillingPeriod { get; set; }
        public WSAStoreDurationUnit BillingPeriodUnit { get; set; }
        public bool HasTrialPeriod { get; set; }
        public int TrialPeriod { get; set; }
        public WSAStoreDurationUnit TrialPeriodUnit { get; set; }
    }
}