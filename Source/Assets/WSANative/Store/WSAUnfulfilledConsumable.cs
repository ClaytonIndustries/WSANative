////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace WSANativeIAPStore
{
    public class WSAUnfulfilledConsumable
    {
        public string OfferId
        {
            get; set; 
        }

        public string ProductId
        {
            get; set;
        }
        public Guid TransactionId
        {
            get; set;
        }
    }
}