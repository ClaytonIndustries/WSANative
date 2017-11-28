////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Twitter
{
    public class WSATwitterResponse
    {
        /// <summary>
        /// Did the request succeed
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Data returned from the request
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// If an error occurs details will be provided here - check if null first
        /// </summary>
        public WSATwitterError Error { get; set; }
    }
}