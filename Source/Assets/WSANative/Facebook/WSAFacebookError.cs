////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using CI.WSANative.Facebook.Models;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookError
    {
        /// <summary>
        /// Message describing the error
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The type of error
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Is the users access token expired - if they so will need to login again
        /// </summary>
        public bool AccessTokenExpired { get; set; }

        public static WSAFacebookError FromDto(WSAFacebookErrorDto dto)
        {
            return new WSAFacebookError()
            {
                Message = dto.error.message,
                Type = dto.error.type,
                Code = dto.error.code
            };
        }
    }
}