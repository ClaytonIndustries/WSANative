////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using CI.WSANative.Twitter.Core;

namespace CI.WSANative.Twitter
{
    public static class WSANativeTwitter
    {
#if NETFX_CORE
        private static readonly WSATwitterApi _twitterApi = new WSATwitterApi();
#endif

        public static void Initialise(string consumerKey, string consumerSecret)
        {
#if NETFX_CORE
            _twitterApi.Initialise(consumerKey, consumerSecret);
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