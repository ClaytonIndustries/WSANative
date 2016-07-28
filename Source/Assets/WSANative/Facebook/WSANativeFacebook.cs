﻿////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using Windows.UI.Xaml.Controls;
#endif

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

        /// <summary>
        /// Determines whether the user has liked a page - you must have requested the permissiom user_likes at login
        /// </summary>
        /// <param name="pageId">The id of the page - see the documentation for details on how to find this</param>
        /// <param name="response">Response indicating whether the user has liked the page</param>
        public static void HasUserLikedPage(string pageId, Action<WSAFacebookResponse<bool>> response)
        {
#if NETFX_CORE
            HasUserLikedPageAsync(pageId, response);
#endif
        }

#if NETFX_CORE
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
        /// <typeparam name="T">A type that will contain the serialised response - see WSAFacebookUser for an example and also check the facebook developer guide for details on what each call returns</typeparam>
        /// <param name="edge">The api edge e.g me/photos</param>
        /// <param name="parameters">Parameters to include in the request - null or empty if none</param>
        /// <param name="response">A callback containing the response</param>
        public static void GraphApiRead<T>(string edge, Dictionary<string, string> parameters, Action<WSAFacebookResponse<T>> response)
        {
#if NETFX_CORE
            GraphApiReadAsync<T>(edge, parameters, response);
#endif
        }

#if NETFX_CORE
        private static async void GraphApiReadAsync<T>(string edge, Dictionary<string, string> parameters, Action<WSAFacebookResponse<T>> response)
        {
            WSAFacebookResponse<T> result = await _facebookApi.GraphApiRead<T>(edge, parameters);

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Allows a user to publish a story to their timeline - this does not require any special permissions nor does it require the user to be currently logged in.
        /// You don't need to specify all of the following parameters, null the ones you don't want
        /// </summary>
        /// <param name="link">The link attached to this post</param>
        /// <param name="picture">The URL of a picture attached to this post. The picture must be at least 200px by 200px</param>
        /// <param name="source">The URL of a media file (either SWF or MP3) attached to this post. If SWF, you must also specify a picture to provide a thumbnail for the video</param>
        /// <param name="name">The name of the link attachment</param>
        /// <param name="caption">The caption of the link (appears beneath the link name). If not specified, this field is automatically populated with the URL of the link</param>
        /// <param name="description">The description of the link (appears beneath the link caption). If not specified, this field is automatically populated by information scraped from the link, typically the title of the page</param>
        public static void ShowFeedDialog(string link, string picture, string source, string name, string caption, string description)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _facebookApi.ShowFeedDialog(link, picture, source, name, caption, description);
            }, false);
#endif
        }

        /// <summary>
        /// Allows the user to send invites / requests to their Facebook friends
        /// </summary>
        /// <param name="title">The title for the Dialog. Maximum length is 50 characters.</param>
        /// <param name="message">A plain-text message to be sent as part of the request. This text will surface in the App Center view of the request, but not on the notification jewel</param>
        public static void ShowRequestDialog(string title, string message)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(() =>
            {
                _facebookApi.ShowRequestDialog(title, message);
            }, false);
#endif
        }
    }
}