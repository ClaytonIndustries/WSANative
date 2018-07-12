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
    public class WSAStoreProductQueryResult
    {
        public System.Collections.Generic.Dictionary<string, WSAStoreProduct> Products { get; set; }
        public Exception Error { get; set; }
    }
}