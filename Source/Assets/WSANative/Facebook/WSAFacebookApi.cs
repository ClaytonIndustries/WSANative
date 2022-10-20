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
                if (!WSANativeCore.IsDxSwapChainPanelConfigured())
                {
                    throw new InvalidOperationException("CI.WSANative.Common.Initialise() must be called first");
                }

                var requestPermissions = "email";

                if (permissions != null && permissions.Count > 0)
                {
                    requestPermissions = string.Join(",", permissions);
                }

                var requestUri = string.Format("{0}?client_id={1}&response_type=token&redirect_uri={2}&display=popup",
                    WSAFacebookConstants.LoginApiUri, _facebookAppId, _redirectUri);

                var dialog = new FacebookLogin(Screen.width, Screen.height);

                var result = await dialog.Show(requestUri, new Uri(_redirectUri).AbsolutePath, WSANativeCore.DxSwapChainPanel);

                if (result == null)
                {
                    throw new InvalidOperationException("Login was cancelled");
                }

                if (string.IsNullOrEmpty(result.AccessToken))
                {
                    throw new InvalidOperationException("Access token was null");
                }

                AccessToken = result.AccessToken;
                AccessTokenExpiry = result.AccessTokenExpiry;

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

        public void ShowFeedDialog(string link, string source, Action closed)
        {
            if (!WSANativeCore.IsDxSwapChainPanelConfigured())
            {
                throw new InvalidOperationException("CI.WSANative.Common.Initialise() must be called first");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link },
                { "source", source }
            };

            string feedBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_url={2}", WSAFacebookConstants.FeedDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            FeedDialog dialog = new FeedDialog(Screen.width, Screen.height);

            dialog.Show(feedBaseUri, parameters, new Uri(WSAFacebookConstants.WebRedirectUri).AbsolutePath, WSANativeCore.DxSwapChainPanel, closed);
        }

        public void ShowRequestDialog(string title, string message, Action<IEnumerable<string>> closed)
        {
            if (!WSANativeCore.IsDxSwapChainPanelConfigured())
            {
                throw new InvalidOperationException("CI.WSANative.Common.Initialise() must be called first");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "title", title },
                { "message", message },
            };

            string requestBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.RequestDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            RequestDialog dialog = new RequestDialog(Screen.width, Screen.height);

            dialog.Show(requestBaseUri, parameters, new Uri(WSAFacebookConstants.WebRedirectUri).AbsolutePath, WSANativeCore.DxSwapChainPanel, closed);    
        }

        public void ShowSendDialog(string link, Action closed)
        {
            if(!WSANativeCore.IsDxSwapChainPanelConfigured())
            {
                throw new InvalidOperationException("CI.WSANative.Common.Initialise() must be called first");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link }
            };

            string sendBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.SendDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            SendDialog dialog = new SendDialog(Screen.width, Screen.height);

            dialog.Show(sendBaseUri, parameters, new Uri(WSAFacebookConstants.WebRedirectUri).AbsolutePath, WSANativeCore.DxSwapChainPanel, closed);
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