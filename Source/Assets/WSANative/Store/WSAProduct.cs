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
    public class WSAProduct
    {
        public string Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string FormattedPrice
        {
            get; set;
        }

        public Uri ImageUri
        {
            get; set;
        }

        public string ProductType
        {
            get; set;
        }
    }
}