////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using Windows.UI.Xaml.Controls;
#endif

using System;
using System.Collections.Generic;

namespace CI.WSANative.Facebook
{
    public static class WSANativeFacebook
    {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static readonly WSAFacebookApi _facebookApi = new WSAFacebookApi();
#endif

        /// <summary>
        /// Is the user currently logged in
        /// </summary>
        public static bool IsLoggedIn
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            _facebookApi.Initialise(facebookAppId, packageSID);
#endif
        }

#if NETFX_CORE
        public static void ConfigureDialogs(Grid dxSwapChainPanel)
        {
            _facebookApi.ConfigureDialogs(dxSwapChainPanel);
        }
#endif

        /// <summary>
        /// Shows the login dialog to the user and request permissions.
        /// If login is successful an access token will be generated and automatically stored.
        /// The token normally lasts for about 60 days at which point the user will have to be re-authenticated
        /// </summary>
        /// <param name="permissions">Any combination of permissions eg "public_profile, email, user_birthday, user_likes" etc - only request what you need</param>
        /// <param name="response">Did the login request succeed</param>
        public static void Login(List<string> permissions, Action<WSAFacebookLoginResult> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                WSAFacebookLoginResult result = await _facebookApi.Login(permissions);

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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            _facebookApi.Logout(uninstall);
#endif
        }

        /// <summary>
        /// Requests details about the logged in user based on the permissions you asked for when they logged in
        /// </summary>
        /// <param name="response">Response containing user details if successful</param>
        public static void GetUserDetails(Action<WSAFacebookResponse<WSAFacebookUser>> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            GetUserDetailsAsync(response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void GetUserDetailsAsync(Action<WSAFacebookResponse<WSAFacebookUser>> response)
        {
            WSAFacebookResponse<WSAFacebookUser> result = await _facebookApi.GetUserDetails();

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Determines whether the user has liked a page - you must have requested the permission user_likes at login
        /// </summary>
        /// <param name="pageId">The id of the page - see the documentation for details on how to find this</param>
        /// <param name="response">Response indicating whether the user has liked the page</param>
        public static void HasUserLikedPage(string pageId, Action<WSAFacebookResponse<bool>> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            HasUserLikedPageAsync(pageId, response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void HasUserLikedPageAsync(string pageId, Action<WSAFacebookResponse<bool>> response)
        {
            WSAFacebookResponse<bool> result = await _facebookApi.HasUserLikedPage(pageId);

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Allows you to read from the facebook graph api - see the facebook developer guide for a full list of available actions
        /// </summary>
        /// <param name="edge">The api edge e.g me/photos</param>
        /// <param name="parameters">Parameters to include in the request - null or empty if none</param>
        /// <param name="response">A callback containing the response</param>
        public static void GraphApiRead(string edge, Dictionary<string, string> parameters, Action<WSAFacebookResponse<string>> response)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            GraphApiReadAsync(edge, parameters, response);
#endif
        }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        private static async void GraphApiReadAsync(string edge, Dictionary<string, string> parameters, Action<WSAFacebookResponse<string>> response)
        {
            WSAFacebookResponse<string> result = await _facebookApi.GraphApiRead(edge, parameters);

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Allows the user to publish a story to their timeline - this does not require any special permissions nor does it require the user to be currently logged in.
        /// You don't need to specify all of the following parameters, null the ones you don't want
        /// </summary>
        /// <param name="link">The link attached to this post</param>
        /// <param name="source">The URL of a media file (either SWF or MP3) attached to this post. If SWF, you must also specify a picture to provide a thumbnail for the video</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowFeedDialog(string link, string source, Action closed)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _facebookApi.ShowFeedDialog(link, source, closed);
            }, false);
#elif (ENABLE_IL2CPP && UNITY_WSA_10_0)
            throw new Exception("Dialogs are not currently supported in IL2CPP");
#endif
        }

        /// <summary>
        /// Allows the user to send invites / requests to their Facebook friends - this does not require any special permissions nor does it require the user to be currently logged in
        /// </summary>
        /// <param name="title">The title for the Dialog. Maximum length is 50 characters.</param>
        /// <param name="message">A plain-text message to be sent as part of the request. This text will surface in the App Center view of the request, but not on the notification jewel</param>
        /// <param name="closed">A callback indicating that the dialog has closed. Containing a list of user ids that were invited</param>
        public static void ShowRequestDialog(string title, string message, Action<IEnumerable<string>> closed)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _facebookApi.ShowRequestDialog(title, message, closed);
            }, false);
#elif (ENABLE_IL2CPP && UNITY_WSA_10_0)
            throw new Exception("Dialogs are not currently supported in IL2CPP");
#endif
        }

        /// <summary>
        /// Allows the user to send a private message to their Facebook friends that contains a link - this does not require any special permissions nor does it require the user to be currently logged in
        /// </summary>
        /// <param name="link">The link to send</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowSendDialog(string link, Action closed)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _facebookApi.ShowSendDialog(link, closed);
            }, false);
#elif (ENABLE_IL2CPP && UNITY_WSA_10_0)
            throw new Exception("Dialogs are not currently supported in IL2CPP");
#endif
        }
    }
}