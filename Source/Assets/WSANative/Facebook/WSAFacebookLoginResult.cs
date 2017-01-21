
namespace CI.WSANative.Facebook
{
    public class WSAFacebookLoginResult
    {
        /// <summary>
        /// Was the login successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// The access token which will be automatically used in future requests to the facebook api. Will be null if the login was not successful
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Error message if the login is not successful
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}