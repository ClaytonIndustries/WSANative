////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if ENABLE_WINMD_SUPPORT
using CI.WSANative.Twitter.Core;
using Windows.UI.Xaml.Controls;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Twitter
{
    public static class WSANativeTwitter
    {
#if ENABLE_WINMD_SUPPORT
        private static readonly WSATwitterApi _twitterApi = new WSATwitterApi();
#endif

        /// <summary>
        /// Is the user currently logged in
        /// </summary>
        public static bool IsLoggedIn
        {
#if ENABLE_WINMD_SUPPORT
            get { return _twitterApi.IsLoggedIn; }
#else
            get { return false; }
#endif
        }

        /// <summary>
        /// Initialise the twitter api - this must be called first - see the website for additional information
        /// </summary>
        /// <param name="consumerKey">Your apps twitter consumer key</param>
        /// <param name="consumerSecret">Your apps twitter consumer secret</param>
        /// /// <param name="oauthCallback">A callback url for oauth (should match the value entered on twitter)</param>
        public static void Initialise(string consumerKey, string consumerSecret, string oauthCallback)
        {
#if ENABLE_WINMD_SUPPORT
            _twitterApi.Initialise(consumerKey, consumerSecret, oauthCallback);
#endif
        }

        /// <summary>
        /// Shows the login dialog to the user.
        /// If login is successful an access token will be generated and automatically stored.
        /// </summary>
        /// <param name="includeEmail">Should the response include the users email ("Request email addresses from users" must be set on twitter first)</param>
        /// <param name="response">Did the login request succeed</param>
        public static void Login(bool includeEmail, Action<WSATwitterLoginResult> response)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                WSATwitterLoginResult result = await _twitterApi.Login(includeEmail);

                ThreadRunner.RunOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(result);
                    }
                }, true);
            });
#endif
        }

        /// <summary>
        /// Log the user out of your application (i.e delete their access token)
        /// </summary>
        public static void Logout()
        {
#if ENABLE_WINMD_SUPPORT
            _twitterApi.Logout();
#endif
        }

        /// <summary>
        /// Requests full details about the logged in user - json response is returned (parse the fields you need)
        /// </summary>
        /// <param name="includeEmail">Should the response include the users email ("Request email addresses from users" must be set on twitter first)</param>
        /// <param name="response">Response containing user details if successful</param>
        public static void GetUserDetails(bool includeEmail, Action<WSATwitterResponse> response)
        {
#if ENABLE_WINMD_SUPPORT
            GetUserDetailsAsync(includeEmail, response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static async void GetUserDetailsAsync(bool includeEmail, Action<WSATwitterResponse> response)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "include_email", includeEmail ? "true" : "false" }
            };

            WSATwitterResponse result = await _twitterApi.ApiRead("https://api.twitter.com/1.1/account/verify_credentials.json", parameters, true);

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Call any GET method on the twitter api - json response is returned (parse the fields you need)
        /// </summary>
        /// <param name="url">The base url (e.g https://api.twitter.com/1.1/statuses/user_timeline.json) - any query string arguments should be added to the parameters dictionary</param>
        /// <param name="parameters">Any parameters to be appended to the url - can be a null or empty dictionary if there are none</param>
        /// <param name="response">Response containing the requested data if successful</param>
        public static void ApiRead(string url, IDictionary<string, string> parameters, Action<WSATwitterResponse> response)
        {
#if ENABLE_WINMD_SUPPORT
            ApiReadAsync(url, parameters, response);
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static async void ApiReadAsync(string url, IDictionary<string, string> parameters, Action<WSATwitterResponse> response)
        {
            WSATwitterResponse result = await _twitterApi.ApiRead(url, parameters, true);

            if (response != null)
            {
                response(result);
            }
        }
#endif

        /// <summary>
        /// Allows the user to publish a tweet - this does not require any special permissions nor does it require the user to be currently logged in.
        /// </summary>
        /// <param name="parameters">Optional parameters to include - see the web docs for the full list - can be a null or empty dictionary if there are none</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowTweetDialog(IDictionary<string, string> parameters, Action closed)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(() =>
            {
                _twitterApi.ShowTweetDialog("https://twitter.com/intent/tweet", parameters, closed);
            });
#endif
        }

        /// <summary>
        /// Allows the user to retweet a tweet - this does not require any special permissions nor does it require the user to be currently logged in.
        /// </summary>
        /// <param name="tweetId">The id of thw tweet to be retweeted</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowRetweetDialog(string tweetId, Action closed)
        {
#if ENABLE_WINMD_SUPPORT
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "tweet_id", tweetId }
            };

            ThreadRunner.RunOnUIThread(() =>
            {
                _twitterApi.ShowTweetDialog("https://twitter.com/intent/retweet", parameters, closed);
            });
#endif
        }

        /// <summary>
        /// Allows the user to like a tweet - this does not require any special permissions nor does it require the user to be currently logged in.
        /// </summary>
        /// <param name="tweetId">The id of thw tweet to be retweeted</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowLikeTweetDialog(string tweetId, Action closed)
        {
#if ENABLE_WINMD_SUPPORT
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "tweet_id", tweetId }
            };

            ThreadRunner.RunOnUIThread(() =>
            {
                _twitterApi.ShowTweetDialog("https://twitter.com/intent/like", parameters, closed);
            });
#endif
        }

        /// <summary>
        /// Shows a mini view of a profile - this does not require any special permissions nor does it require the user to be currently logged in.
        /// </summary>
        /// <param name="userId">The Twitter user identifier of the account</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowMiniProfileDialog(string userId, Action closed)
        {
#if ENABLE_WINMD_SUPPORT
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "user_id", userId }
            };

            ThreadRunner.RunOnUIThread(() =>
            {
                _twitterApi.ShowTweetDialog("https://twitter.com/intent/user", parameters, closed);
            });
#endif
        }

        /// <summary>
        /// Allows the user to follow an account - this does not require any special permissions nor does it require the user to be currently logged in.
        /// </summary>
        /// <param name="userId">The Twitter user identifier of the account</param>
        /// <param name="closed">A callback indicating that the dialog has closed</param>
        public static void ShowFollowDialog(string userId, Action closed)
        {
#if ENABLE_WINMD_SUPPORT
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "user_id", userId }
            };

            ThreadRunner.RunOnUIThread(() =>
            {
                _twitterApi.ShowTweetDialog("https://twitter.com/intent/follow", parameters, closed);
            });
#endif
        }
    }
}