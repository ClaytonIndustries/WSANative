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
    public class WSAStorePurchaseResult
    {
        public WSAStorePurchaseStatus Status { get; set; }
        public Exception Error { get; set; }
    }
}