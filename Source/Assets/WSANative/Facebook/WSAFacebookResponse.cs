////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Facebook
{
    public class WSAFacebookResponse<T>
    {
        /// <summary>
        /// Did the request succeed
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Data returned from the request
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// If an error occurs details may be provided here - check if null first
        /// </summary>
        public WSAFacebookError Error { get; set; }
    }
}