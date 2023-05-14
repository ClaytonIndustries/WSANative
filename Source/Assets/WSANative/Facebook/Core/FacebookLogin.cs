////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AOT;
using CI.WSANative.Facebook.Models;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookLogin
    {
        private delegate void FacebookLoginResponsCallbackDelegate([MarshalAs(UnmanagedType.LPWStr)]string response);

        [DllImport("__Internal")]
        private static extern void _FacebookLoginShow(FacebookLoginResponsCallbackDelegate callback, int screenWidth, int screenHeight, [MarshalAs(UnmanagedType.LPWStr)]string requestUri, [MarshalAs(UnmanagedType.LPWStr)]string responseUri, bool delayDialog);

		private static TaskCompletionSource<WSAFacebookLoginResult> _taskCompletionSource;

        public FacebookLogin()
        {		
			_taskCompletionSource = new TaskCompletionSource<WSAFacebookLoginResult>();
        }

        public async Task<WSAFacebookLoginResult> Show(int screenWidth, int screenHeight, string requestUri, string responseUri, bool delayDialog)
        {
            _FacebookLoginShow(FacebookLoginCallback, screenWidth, screenHeight, requestUri, responseUri, delayDialog);

            return await _taskCompletionSource.Task;
        }

        [MonoPInvokeCallback(typeof(FacebookLoginResponsCallbackDelegate))]
        private static void FacebookLoginCallback(string response)
        {
            WSAFacebookLoginResult result = null;

            if (response != null)
            {
                var accessTokenMatch = Regex.Match(response, "access_token=(.+?)(&|$)");
                var accessToken = accessTokenMatch.Groups.Count >= 2 ? accessTokenMatch.Groups[1].Value : string.Empty;
                var accessTokenExpiryMatch = Regex.Match(response, "expires_in=(.+?)(&|$)");
                int.TryParse(accessTokenExpiryMatch.Groups.Count >= 2 ? accessTokenExpiryMatch.Groups[1].Value : "0", out int accessTokenExpiry);

                result = new WSAFacebookLoginResult()
                {
                    AccessToken = accessToken,
                    AccessTokenExpiry = DateTime.Now.AddSeconds(accessTokenExpiry)
                };
            }

            _taskCompletionSource.SetResult(result);
        }
    }
}
#endif