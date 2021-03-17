////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Twitter
{
    public class WSATwitterLoginResult
    {
        /// <summary>
        /// Was the login successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The users twitter User Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The users twitter Screen Name
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// The users full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The users email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Error message if the login was not successful
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}