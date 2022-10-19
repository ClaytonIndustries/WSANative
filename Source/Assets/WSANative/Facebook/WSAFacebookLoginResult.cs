////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookLoginResult
    {
        /// <summary>
        /// Was the login successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The users access token which will be automatically used in future requests to the facebook api. Will be null if the login was not successful
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The date that the users access token expires
        /// </summary>
        public DateTime AccessTokenExpiry { get; set; }

        /// <summary>
        /// Data about the user
        /// </summary>
        public WSAFacebookUser User { get; set; }

        /// <summary>
        /// Error message if the login was not successful
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}