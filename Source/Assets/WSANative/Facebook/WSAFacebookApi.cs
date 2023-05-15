////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using CI.WSANative.Common;
using CI.WSANative.Common.Http;
using CI.WSANative.Facebook.Core;
using CI.WSANative.Facebook.Models;
using UnityEngine;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookApi
    {
        public bool IsLoggedIn { get; private set; }
        public string AccessToken { get; private set; }
        public DateTime AccessTokenExpiry { get; private set; }
        public string UserId { get; private set; }

        private string _facebookAppId;
        private string _redirectUri;

        private const string _savedDataFilename = "FacebookData.sav";
        private const string _authenticationErrorCode = "190";

        public WSAFacebookApi()
        {
            Startup();
        }

        private async void Startup()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(_savedDataFilename);

                var content = await FileIO.ReadTextAsync(file);

                var split = content.Split('&');

                AccessToken = split.Length > 0 ? split[0] : null;
                AccessTokenExpiry = split.Length > 1 ? DateTime.Parse(split[1]) : DateTime.MinValue;
                UserId = split.Length > 2 ? split[2] : null;

                IsLoggedIn = true;
            }
            catch
            {
            }
        }

        public void Initialise(string facebookAppId, string redirectUri)
        {
            _facebookAppId = facebookAppId;
            _redirectUri = redirectUri;
        }

        public async Task<WSAFacebookLoginResult> Login(List<string> permissions)
        {
            WSAFacebookLoginResult loginResult = new WSAFacebookLoginResult()
            {
                Success = true
            };

            try
            {
                var requestPermissions = "email";

                if (permissions != null && permissions.Count > 0)
                {
                    requestPermissions = string.Join(",", permissions);
                }

                var requestUri = string.Format("{0}?client_id={1}&response_type=token&redirect_uri={2}&scope={3}&display=popup",
                    WSAFacebookConstants.LoginApiUri, _facebookAppId, _redirectUri, requestPermissions);

                var dialog = new FacebookDialog();

                var result = await dialog.Show(Screen.width, Screen.height, requestUri, new Uri(_redirectUri).AbsolutePath, IsLoggedIn);

                if (result == null)
                {
                    throw new InvalidOperationException("Login was cancelled");
                }

                var accessTokenMatch = Regex.Match(result, "access_token=(.+?)(&|$)");
                var accessToken = accessTokenMatch.Groups.Count >= 2 ? accessTokenMatch.Groups[1].Value : string.Empty;
                var accessTokenExpiryMatch = Regex.Match(result, "expires_in=(.+?)(&|$)");
                int.TryParse(accessTokenExpiryMatch.Groups.Count >= 2 ? accessTokenExpiryMatch.Groups[1].Value : "0", out int accessTokenExpiry);

                loginResult.AccessToken = accessToken;
                loginResult.AccessTokenExpiry = DateTime.Now.AddSeconds(accessTokenExpiry);

                if (string.IsNullOrEmpty(loginResult.AccessToken))
                {
                    throw new InvalidOperationException("Access token was null");
                }

                AccessToken = loginResult.AccessToken;
                AccessTokenExpiry = loginResult.AccessTokenExpiry;

                var userDetails = await GetUserDetails(false);

                if (!userDetails.Success)
                {
                    throw new InvalidOperationException("Failed to get user details");
                }
                
                UserId = userDetails.Data.UserId;
                IsLoggedIn = true;

                try
                {
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(_savedDataFilename, CreationCollisionOption.ReplaceExisting);

                    await FileIO.WriteTextAsync(file, AccessToken + "&" + AccessTokenExpiry.ToString("s", System.Globalization.CultureInfo.InvariantCulture) + "&" + UserId);
                }
                catch
                {
                }

                loginResult.AccessToken = AccessToken;
                loginResult.AccessTokenExpiry = AccessTokenExpiry;
                loginResult.User = userDetails.Data;
                return loginResult;
            }
            catch (Exception e)
            {
                IsLoggedIn = false;
                loginResult.Success = false;
                loginResult.ErrorMessage = e.Message;
                return loginResult;
            }
        }

        public async void Logout(bool uninstall)
        {
            if (IsLoggedIn)
            {
                if (uninstall)
                {
                    try
                    {
                        var requestUri = string.Format("{0}me/permissions?access_token={1}", WSAFacebookConstants.GraphApiUri, AccessToken);

                        await MakeRequest(requestUri, HttpAction.Delete);
                    }
                    catch
                    {
                    }
                }

                IsLoggedIn = false;
                AccessToken = null;
                AccessTokenExpiry = DateTime.MinValue;
                UserId = null;

                try
                {
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync(_savedDataFilename);

                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
                catch
                {
                }

                ThreadRunner.RunOnUIThread(async () =>
                {
                    await Windows.UI.Xaml.Controls.WebView.ClearTemporaryWebDataAsync();
                });
            }
        }

        public async Task<WSAFacebookResponse<WSAFacebookUser>> GetUserDetails(bool checkLoginStatus)
        {
            WSAFacebookResponse<WSAFacebookUser> userDetailsResponse = new WSAFacebookResponse<WSAFacebookUser>();

            if (!checkLoginStatus || IsLoggedIn)
            {
                var fields = "id,email,first_name,name,picture";

                var requestUri = string.Format("{0}me?fields={1}&access_token={2}", WSAFacebookConstants.GraphApiUri, fields, AccessToken);

                try
                {
                    HttpResponseMessage response = await MakeRequest(requestUri, HttpAction.Get);

                    var responseAsString = response.ReadAsString();

                    if (response.IsSuccessStatusCode)
                    {
                        userDetailsResponse.Data = WSAFacebookUser.FromDto(JsonUtility.FromJson<WSAFacebookUserDto>(responseAsString));
                        userDetailsResponse.Success = true;
                    }
                    else
                    {
                        WSAFacebookError errorMessage = WSAFacebookError.FromDto(JsonUtility.FromJson<WSAFacebookErrorDto>(responseAsString));

                        if (errorMessage.Code == _authenticationErrorCode)
                        {
                            Logout(false);
                            errorMessage.AccessTokenExpired = true;
                        }

                        userDetailsResponse.Success = false;
                        userDetailsResponse.Error = errorMessage;
                    }
                }
                catch (Exception e)
                {
                    userDetailsResponse.Error = new WSAFacebookError()
                    {
                        Message = e.Message
                    };
                    userDetailsResponse.Success = false;
                }
            }
            else
            {
                userDetailsResponse.Success = false;
                userDetailsResponse.Error = new WSAFacebookError()
                {
                    AccessTokenExpired = true
                };
            }

            return userDetailsResponse;
        }

        public async Task<WSAFacebookResponse<string>> GraphApiRead(string edge, IDictionary<string, string> parameters)
        {
            WSAFacebookResponse<string> graphApiReadResponse = new WSAFacebookResponse<string>();

            if (IsLoggedIn)
            {
                var fields = string.Empty;

                if(parameters != null && parameters.Count > 0)
                {
                    fields = parameters.Aggregate(string.Empty, (total, next) => total += (next.Key + "=" + next.Value + "&"));
                }

                var requestUri = string.Format("{0}{1}?{2}access_token={3}", WSAFacebookConstants.GraphApiUri, edge, fields, AccessToken);

                try
                {
                    var response = await MakeRequest(requestUri, HttpAction.Get);

                    var responseAsString = response.ReadAsString();

                    if (response.IsSuccessStatusCode)
                    {
                        graphApiReadResponse.Data = responseAsString;
                        graphApiReadResponse.Success = true;
                    }
                    else
                    {
                        WSAFacebookError errorMessage = WSAFacebookError.FromDto(JsonUtility.FromJson<WSAFacebookErrorDto>(responseAsString));

                        if (errorMessage.Code == _authenticationErrorCode)
                        {
                            Logout(false);
                            errorMessage.AccessTokenExpired = true;
                        }

                        graphApiReadResponse.Success = false;
                        graphApiReadResponse.Error = errorMessage;
                    }
                }
                catch (Exception e)
                {
                    graphApiReadResponse.Error = new WSAFacebookError()
                    {
                        Message = e.Message
                    };
                    graphApiReadResponse.Success = false;
                }
            }
            else
            {
                graphApiReadResponse.Success = false;
                graphApiReadResponse.Error = new WSAFacebookError()
                {
                    AccessTokenExpired = true
                };
            }

            return graphApiReadResponse;
        }

        public async Task ShowFeedDialog(string link, string hashtag)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "app_id", _facebookAppId },
                { "redirect_uri", _redirectUri },
                { "href", link },
                { "hashtag", hashtag },
                { "display", "popup" }
            };

            var requestUri = string.Format("{0}{1}", WSAFacebookConstants.ShareDialogUri, parameters.Aggregate(string.Empty, (total, current) =>
            {
                if(!string.IsNullOrEmpty(current.Value))
                {
                    total = string.Format("{0}{1}{2}={3}", total, string.IsNullOrEmpty(total) ? "?" : "&", current.Key, Uri.EscapeUriString(current.Value));
                }

                return total;
            }));

            var dialog = new FacebookDialog();

            await dialog.Show(Screen.width, Screen.height, requestUri, new Uri(_redirectUri).AbsolutePath, false);
        }

        public async Task<IEnumerable<string>> ShowRequestDialog(string title, string message)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "app_id", _facebookAppId },
                { "redirect_uri", _redirectUri },
                { "title", title },
                { "message", message },
                { "display", "popup" }
            };

            var requestUri = string.Format("{0}{1}", WSAFacebookConstants.RequestDialogUri, parameters.Aggregate(string.Empty, (total, current) =>
            {
                if(!string.IsNullOrEmpty(current.Value))
                {
                    total = string.Format("{0}{1}{2}={3}", total, string.IsNullOrEmpty(total) ? "?" : "&", current.Key, Uri.EscapeUriString(current.Value));
                }

                return total;
            }));

            var dialog = new FacebookDialog();

            var result = await dialog.Show(Screen.width, Screen.height, requestUri, new Uri(_redirectUri).AbsolutePath, false);

            IList<string> userIds = new List<string>();

            if (result != null)
            {
                MatchCollection matches = Regex.Matches(result, "&to\\[\\d+\\]=(\\d+)");

                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].Success)
                    {
                        userIds.Add(matches[i].Groups[1].Value);
                    }
                }
            }

            return userIds;
        }

        public async Task ShowSendDialog(string link)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "app_id", _facebookAppId },
                { "redirect_uri", _redirectUri },
                { "link", link },
                { "display", "popup" }
            };

            var requestUri = string.Format("{0}{1}", WSAFacebookConstants.SendDialogUri, parameters.Aggregate(string.Empty, (total, current) =>
            {
                if(!string.IsNullOrEmpty(current.Value))
                {
                    total = string.Format("{0}{1}{2}={3}", total, string.IsNullOrEmpty(total) ? "?" : "&", current.Key, Uri.EscapeUriString(current.Value));
                }

                return total;
            }));

            var dialog = new FacebookDialog();

            await dialog.Show(Screen.width, Screen.height, requestUri, new Uri(_redirectUri).AbsolutePath, false);
        }

        private Task<HttpResponseMessage> MakeRequest(string url, HttpAction method)
        {
            var task = new TaskCompletionSource<HttpResponseMessage>();

            var client = new HttpClient()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var headers = new Dictionary<string, string>()
            {
                { "Accept-Encoding", "gzip, deflate" }
            };

            var request = new HttpRequestMessage()
            {
                Uri = new Uri(url),
                Method = method
            }; 

            client.Send(request, HttpCompletionOption.AllResponseContent, r =>
            {
                task.SetResult(r);
            });

            return task.Task;
        }
    }
}
#endif