using System;
using System.Collections.Generic;

namespace CI.WSANative.Facebook
{
    public static class WSANativeFacebook
    {
#if NETFX_CORE
        private static WSAFacebookApi _facebookApi = new WSAFacebookApi();
#endif

        /// <summary>
        /// Is the user currently logged in
        /// </summary>
        public static bool IsLoggedIn
        {
#if NETFX_CORE
            get { return _facebookApi.IsLoggedIn; }
#else
            get { return false; }
#endif
        }

        /// <summary>
        /// Initialise the facebook api - this must be called first - see the website for additional information
        /// </summary>
        /// <param name="facebookAppId">Your apps facebook id</param>
        /// <param name="packageSID">Your apps SID</param>
        public static void Initialise(string facebookAppId, string packageSID)
        {
#if NETFX_CORE
            _facebookApi.Initialise(facebookAppId, packageSID);
#endif
        }

        /// <summary>
        /// Show the login dialog to the user and request permissions.
        /// If login is successful an access token will be generated and automatically stored.
        /// The token normally last for about 60 days at which point the user will have to be re-authenticated
        /// </summary>
        /// <param name="permissions">Any combination of "public_profile, email, user_birthday" - only request what you need</param>
        /// <param name="response">Did the login request succeed</param>
        public static void Login(List<string> permissions, Action<bool> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                bool result = await _facebookApi.Login(permissions);

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

        /// <summary>
        /// Log the user out of your application (i.e delete their access token) and optionally uninstall your app from their profile
        /// </summary>
        /// <param name="uninstall">If true attempts to uninstall your app from their profile (i.e removes all permissions)</param>
        public static void Logout(bool uninstall)
        {
#if NETFX_CORE
            _facebookApi.Logout(uninstall);
#endif
        }

        /// <summary>
        /// Requests details about the logged in user based on the permissions you asked for when they logged in
        /// </summary>
        /// <param name="response">Response containing user details if successful</param>
        public static void GetUserDetails(Action<WSAFacebookResponse<WSAFacebookUser>> response)
        {
#if NETFX_CORE
            GetUserDetailsAsync(response);
#endif
        }

#if NETFX_CORE
        private static async void GetUserDetailsAsync(Action<WSAFacebookResponse<WSAFacebookUser>> response)
        {
            WSAFacebookResponse<WSAFacebookUser> result = await _facebookApi.GetUserDetails();

            if (response != null)
            {
                response(result);
            }
        }
#endif
    }
}