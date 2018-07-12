////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.Store
{
    public class WSAStoreLicense
    {
        public DateTimeOffset ExpirationDate { get; set; }
        public string InAppOfferToken { get; set; }
        public bool IsActive { get; set; }
        public string StoreId { get; set; }
    }
}