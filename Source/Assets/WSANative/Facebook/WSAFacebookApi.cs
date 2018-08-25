////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using CI.WSANative.Common.Http;
using CI.WSANative.Facebook.Core;
using CI.WSANative.Facebook.Models;
using UnityEngine;

#if UNITY_WSA_10_0
using System.Text.RegularExpressions;
using Windows.Security.Authentication.Web;
#endif

#if (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System.Runtime.InteropServices;
#endif

namespace CI.WSANative.Facebook
{
    public class WSAFacebookApi
    {
        public bool IsLoggedIn { get; private set; }

        private string _facebookAppId;
        private string _packageSID;
        private string _accessToken;
        private Windows.UI.Xaml.Controls.Grid _dxSwapChainPanel;

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
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(_savedDataFilename);

                _accessToken = await FileIO.ReadTextAsync(file);

                IsLoggedIn = true;
            }
            catch
            {
            }
        }

        public void Initialise(string facebookAppId, string packageSID)
        {
            _facebookAppId = facebookAppId;
            _packageSID = packageSID;
        }

#if NETFX_CORE
        public void ConfigureDialogs(Windows.UI.Xaml.Controls.Grid dxSwapChainPanel)
        {
            _dxSwapChainPanel = dxSwapChainPanel;
        }
#endif

#if (ENABLE_IL2CPP && UNITY_WSA_10_0)
        [DllImport("__Internal")]
        private static extern Windows.UI.Xaml.Controls.SwapChainPanel GetSwapChainPanel();
#endif

        public async Task<WSAFacebookLoginResult> Login(List<string> permissions)
        {
            WSAFacebookLoginResult loginResult = new WSAFacebookLoginResult();

            try
            {
                Logout(false);

                string requestPermissions = "public_profile";

                if (permissions != null && permissions.Count > 0)
                {
                    requestPermissions = string.Join(",", permissions);
                }

                string accessToken = string.Empty;

#if UNITY_WSA_10_0
                Uri appCallbackUri = new Uri("ms-app://" + _packageSID);

                Uri requestUri = new Uri(
                    string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&response_type=token&redirect_uri={1}&scope={2}", 
                                    _facebookAppId, appCallbackUri, requestPermissions));

                WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, appCallbackUri);

                if (result.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    Match match = Regex.Match(result.ResponseData, "access_token=(.+)&");

                    accessToken = match.Groups[1].Value;
                }
#else
                if (_dxSwapChainPanel != null)
                {
                     string requestUri = string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&response_type=token&redirect_uri={1}&scope={2}", 
                                                        _facebookAppId, WSAFacebookConstants.WebRedirectUri, requestPermissions);

                    FacebookLogin dialog = new FacebookLogin(Screen.width, Screen.height);

                    accessToken = await dialog.Show(requestUri, WSAFacebookConstants.LoginDialogResponseUri, _dxSwapChainPanel);
                }
#endif

                if (!string.IsNullOrEmpty(accessToken))
                {
                    _accessToken = accessToken;
                    IsLoggedIn = true;

                    try
                    {
                        StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(_savedDataFilename, CreationCollisionOption.ReplaceExisting);

                        await FileIO.WriteTextAsync(file, _accessToken);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception e)
            {
                IsLoggedIn = false;
                loginResult.ErrorMessage = e.Message;
            }

            loginResult.Success = IsLoggedIn;
            loginResult.AccessToken = !string.IsNullOrWhiteSpace(_accessToken) ? _accessToken : null;

            return loginResult;
        }

        public async void Logout(bool uninstall)
        {
            if (IsLoggedIn)
            {
                if (uninstall)
                {
                    try
                    {
                        string requestUri = string.Format("{0}me/permissions?access_token={1}", WSAFacebookConstants.GraphApiUri, _accessToken);

                        HttpClient client = new HttpClient();

                        HttpResponseMessage message = await client.Delete(requestUri);

                        string response = message.Data;
                    }
                    catch
                    {
                    }
                }

                IsLoggedIn = false;
                _accessToken = null;

                try
                {
                    StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(_savedDataFilename);

                    await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
                catch
                {
                }
            }
        }

        public async Task<WSAFacebookResponse<WSAFacebookUser>> GetUserDetails()
        {
            WSAFacebookResponse<WSAFacebookUser> userDetailsResponse = new WSAFacebookResponse<WSAFacebookUser>();

            if (IsLoggedIn)
            {
                string fields = "id,age_range,birthday,email,first_name,gender,last_name,link,locale,name,picture,timezone";

                string requestUri = string.Format("{0}me?fields={1}&access_token={2}", WSAFacebookConstants.GraphApiUri, fields, _accessToken);

                try
                {
                    HttpClient client = new HttpClient();

                    HttpResponseMessage response = await client.Get(requestUri);

                    string responseAsString = response.Data;

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
                catch
                {
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

        public async Task<WSAFacebookResponse<bool>> HasUserLikedPage(string pageId)
        {
            WSAFacebookResponse<bool> hasUserLikedPageResponse = new WSAFacebookResponse<bool>();

            if (IsLoggedIn)
            {
                string requestUri = string.Format("{0}me/likes/{1}?access_token={2}", WSAFacebookConstants.GraphApiUri, pageId, _accessToken);

                try
                {
                    HttpClient client = new HttpClient();

                    HttpResponseMessage response = await client.Get(requestUri);

                    string responseAsString = response.Data;

                    if (response.IsSuccessStatusCode)
                    {
                        WSAFacebookDataResponse parsedResponse = JsonUtility.FromJson<WSAFacebookDataResponse>(responseAsString);

                        hasUserLikedPageResponse.Data = parsedResponse.data != null;
                        hasUserLikedPageResponse.Success = true;
                    }
                    else
                    {
                        WSAFacebookError errorMessage = WSAFacebookError.FromDto(JsonUtility.FromJson<WSAFacebookErrorDto>(responseAsString));

                        if (errorMessage.Code == _authenticationErrorCode)
                        {
                            Logout(false);
                            errorMessage.AccessTokenExpired = true;
                        }

                        hasUserLikedPageResponse.Success = false;
                        hasUserLikedPageResponse.Error = errorMessage;
                    }
                }
                catch
                {
                    hasUserLikedPageResponse.Success = false;
                }
            }
            else
            {
                hasUserLikedPageResponse.Success = false;
                hasUserLikedPageResponse.Error = new WSAFacebookError()
                {
                    AccessTokenExpired = true
                };
            }

            return hasUserLikedPageResponse;
        }

        public async Task<WSAFacebookResponse<string>> GraphApiRead(string edge, Dictionary<string, string> parameters)
        {
            WSAFacebookResponse<string> graphApiReadResponse = new WSAFacebookResponse<string>();

            if (IsLoggedIn)
            {
                string fields = string.Empty;

                if(parameters != null && parameters.Count > 0)
                {
                    fields = parameters.Aggregate(string.Empty, (total, next) => total += (next.Key + "=" + next.Value + "&"));
                }

                string requestUri = string.Format("{0}{1}?{2}access_token={3}", WSAFacebookConstants.GraphApiUri, edge, fields, _accessToken);

                try
                {
                    HttpClient client = new HttpClient();

                    HttpResponseMessage response = await client.Get(requestUri);

                    string responseAsString = response.Data;

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
                catch
                {
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
            if (_dxSwapChainPanel == null)
            {
                throw new InvalidOperationException("WSANativeFacebook.ConfigureDialogs must first be called from MainPage.xaml.cs");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link },
                { "source", source }
            };

            string feedBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_url={2}", WSAFacebookConstants.FeedDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            FeedDialog dialog = new FeedDialog(Screen.width, Screen.height);

            dialog.Show(feedBaseUri, parameters, WSAFacebookConstants.FeedDialogResponseUri, _dxSwapChainPanel, closed);
        }

        public void ShowRequestDialog(string title, string message, Action<IEnumerable<string>> closed)
        {
            if (_dxSwapChainPanel == null)
            {
                throw new InvalidOperationException("WSANativeFacebook.ConfigureDialogs must first be called from MainPage.xaml.cs");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "title", title },
                { "message", message },
            };

            string requestBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.RequestDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            RequestDialog dialog = new RequestDialog(Screen.width, Screen.height);

            dialog.Show(requestBaseUri, parameters, WSAFacebookConstants.RequestDialogResponseUri, _dxSwapChainPanel, closed);    
        }

        public void ShowSendDialog(string link, Action closed)
        {
            if(_dxSwapChainPanel == null)
            {
                throw new InvalidOperationException("WSANativeFacebook.ConfigureDialogs must first be called from MainPage.xaml.cs");
            }

            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link }
            };

            string sendBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.SendDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            SendDialog dialog = new SendDialog(Screen.width, Screen.height);

            dialog.Show(sendBaseUri, parameters, WSAFacebookConstants.SendDialogResponseUri, _dxSwapChainPanel, closed);
        }
    }
}
#endif