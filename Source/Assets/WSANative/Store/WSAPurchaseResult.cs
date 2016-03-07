////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.IAPStore
{
    public class WSAPurchaseResult
    {
        public WSAPurchaseResultStatus Status
        {
            get; set;
        }

        public Guid TransactionId
        {
            get; set;
        }

        public string ReceiptXml
        {
            get; set;
        }

        public string OfferId
        {
            get; set;
        }
    }
}