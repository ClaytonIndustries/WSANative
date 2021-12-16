////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Store
{
    public enum WSAStorePurchaseStatus
    {
        Succeeded = 0,
        AlreadyPurchased = 1,
        NotPurchased = 2,
        NetworkError = 3,
        ServerError = 4
    }
}