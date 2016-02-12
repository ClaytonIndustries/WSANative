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
    public class WSAProductLicense
    {
        public bool IsActive
        {
            get; set;
        }

        /// <summary>
        /// If the license is for an app - is it in trial mode
        /// </summary>
        public bool IsTrial
        {
            get; set;
        }

        /// <summary>
        /// If the license is for a product - is it a consumable
        /// </summary>
        public bool IsConsumable
        {
            get; set;
        }

        public DateTimeOffset ExpirationDate
        {
            get; set;
        }
    }
}