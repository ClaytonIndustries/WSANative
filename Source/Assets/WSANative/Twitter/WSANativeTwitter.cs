////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if NETFX_CORE
using CI.WSANative.Twitter.Core;
#endif

namespace CI.WSANative.Twitter
{
    public static class WSANativeTwitter
    {
#if NETFX_CORE
        private static readonly WSATwitterApi _twitterApi = new WSATwitterApi();
#endif

        public static bool IsLoggedIn
        {
#if NETFX_CORE
            get { return _twitterApi.IsLoggedIn; }
#else
            get { return false; }
#endif
        }

        public static void Initialise(string consumerKey, string consumerSecret, string oauthCallback)
        {
#if NETFX_CORE
            _twitterApi.Initialise(consumerKey, consumerSecret, oauthCallback);
#endif
        }

        public static void Login(Action<WSATwitterLoginResult> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                WSATwitterLoginResult result = await _twitterApi.Login();

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(result);
                    }
                }, true);
            }, false);
#endif
        }

        public static void Logout()
        {
#if NETFX_CORE
            _twitterApi.Logout();
#endif
        }

        public static void GetUserDetails(bool includeEmail, Action<WSATwitterResponse> response)
        {
#if NETFX_CORE
            GetUserDetailsAsync(includeEmail, response);
#endif
        }

#if NETFX_CORE
        private static async void GetUserDetailsAsync(bool includeEmail, Action<WSATwitterResponse> response)
        {
            WSATwitterResponse result = await _twitterApi.GetUserDetails(includeEmail);

            if (response != null)
            {
                response(result);
            }
        }
#endif
    }
}