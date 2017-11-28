////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Twitter
{
    public class WSATwitterError
    {
        /// <summary>
        /// Message describing the error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Is the users access token invalid - if they so will need to login again
        /// </summary>
        public bool Unauthorised { get; set; }
    }
}