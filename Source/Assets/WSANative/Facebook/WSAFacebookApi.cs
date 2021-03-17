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

        private string _facebookAppId;
        private string _accessToken;

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

                _accessToken = await FileIO.ReadTextAsync(file);

                IsLoggedIn = true;
            }
            catch
            {
            }
        }

        public void Initialise(string facebookAppId)
        {
            _facebookAppId = facebookAppId;
        }

        public async Task<WSAFacebookLoginResult> Login(List<string> permissions)
        {
            WSAFacebookLoginResult loginResult = new WSAFacebookLoginResult()
            {
                Success = true
            };

            try
            {
                Logout(false);

                var requestPermissions = "email";

                if (permissions != null && permissions.Count > 0)
                {
                    requestPermissions = string.Join(",", permissions);
                }

                var requestUri = string.Format("https://www.facebook.com/v10.0/dialog/oauth?client_id={0}&response_type=token&redirect_uri={1}&scope={2}&display=popup",
                    _facebookAppId, WSAFacebookConstants.WebRedirectUri, requestPermissions);

                var dialog = new FacebookLogin(Screen.width, Screen.height);

                _accessToken = await dialog.Show(requestUri, WSAFacebookConstants.LoginDialogResponseUri, WSANativeCore.DxSwapChainPanel);

                if (string.IsNullOrEmpty(_accessToken))
                {
                    throw new InvalidOperationException("Access token was null");
                }

                try
                {
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(_savedDataFilename, CreationCollisionOption.ReplaceExisting);

                    await FileIO.WriteTextAsync(file, _accessToken);
                }
                catch
                {
                }

                var userDetails = await GetUserDetails(false);

                if (!userDetails.Success)
                {
                    throw new InvalidOperationException("Failed to get user details");
                }

                IsLoggedIn = true;

                loginResult.AccessToken = _accessToken;
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
                        var requestUri = string.Format("{0}me/permissions?access_token={1}", WSAFacebookConstants.GraphApiUri, _accessToken);

                        await MakeRequest(requestUri, HttpAction.Delete);
                    }
                    catch
                    {
                    }
                }

                IsLoggedIn = false;
                _accessToken = null;

                try
                {
                    var file = await ApplicationData.Current.LocalFolder.GetFileAsync(_savedDataFilename);

                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
                catch
                {
                }
            }
        }

        public async Task<WSAFacebookResponse<WSAFacebookUser>> GetUserDetails(bool checkLoginStatus)
        {
            WSAFacebookResponse<WSAFacebookUser> userDetailsResponse = new WSAFacebookResponse<WSAFacebookUser>();

            if (!checkLoginStatus || IsLoggedIn)
            {
                var fields = "id,email,first_name,name,picture";

                var requestUri = string.Format("{0}me?fields={1}&access_token={2}", WSAFacebookConstants.GraphApiUri, fields, _accessToken);

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

        public async Task<WSAFacebookResponse<string>> GraphApiRead(string edge, Dictionary<string, string> parameters)
        {
            WSAFacebookResponse<string> graphApiReadResponse = new WSAFacebookResponse<string>();

            if (IsLoggedIn)
            {
                var fields = string.Empty;

                if(parameters != null && parameters.Count > 0)
                {
                    fields = parameters.Aggregate(string.Empty, (total, next) => total += (next.Key + "=" + next.Value + "&"));
                }

                var requestUri = string.Format("{0}{1}?{2}access_token={3}", WSAFacebookConstants.GraphApiUri, edge, fields, _accessToken);

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
                throw new InvalidOperationException("CI.WSANative.Common.Initialise() must first be called first");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link },
                { "source", source }
            };

            string feedBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_url={2}", WSAFacebookConstants.FeedDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            FeedDialog dialog = new FeedDialog(Screen.width, Screen.height);

            dialog.Show(feedBaseUri, parameters, WSAFacebookConstants.FeedDialogResponseUri, WSANativeCore.DxSwapChainPanel, closed);
        }

        public void ShowRequestDialog(string title, string message, Action<IEnumerable<string>> closed)
        {
            if (!WSANativeCore.IsDxSwapChainPanelConfigured())
            {
                throw new InvalidOperationException("CI.WSANative.Common.Initialise() must first be called first");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "title", title },
                { "message", message },
            };

            string requestBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.RequestDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            RequestDialog dialog = new RequestDialog(Screen.width, Screen.height);

            dialog.Show(requestBaseUri, parameters, WSAFacebookConstants.RequestDialogResponseUri, WSANativeCore.DxSwapChainPanel, closed);    
        }

        public void ShowSendDialog(string link, Action closed)
        {
            if(!WSANativeCore.IsDxSwapChainPanelConfigured())
            {
                throw new InvalidOperationException("CI.WSANative.Common.Initialise() must first be called first");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link }
            };

            string sendBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.SendDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            SendDialog dialog = new SendDialog(Screen.width, Screen.height);

            dialog.Show(sendBaseUri, parameters, WSAFacebookConstants.SendDialogResponseUri, WSANativeCore.DxSwapChainPanel, closed);
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