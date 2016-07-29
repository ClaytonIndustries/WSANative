////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using CI.WSANative.Facebook.Core;
using Newtonsoft.Json;
using UnityEngine;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookApi
    {
        public bool IsLoggedIn { get; private set; }
        public bool IsDialogOpen { get; private set; }

        private string _facebookAppId;
        private string _packageSID;
        private string _accessToken;
        private Grid _dxSwapChainPanel;

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

        public void ConfigureDialogs(Grid dxSwapChainPanel)
        {
            _dxSwapChainPanel = dxSwapChainPanel;
        }

        public async Task<bool> Login(List<string> permissions)
        {
            try
            {
                string requestPermissions = "public_profile";

                if (permissions != null && permissions.Count > 0)
                {
                    requestPermissions = string.Join(",", permissions);
                }

                Uri appCallbackUri = new Uri("ms-app://" + _packageSID);

                Uri requestUri = new Uri(
                    string.Format("https://www.facebook.com/dialog/oauth?client_id={0}&response_type=token&redirect_uri={1}&scope={2}", _facebookAppId, appCallbackUri, requestPermissions));

                WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, appCallbackUri);

                if (result.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    Match match = Regex.Match(result.ResponseData, "access_token=(.+)&");

                    _accessToken = match.Groups[1].Value;
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
            catch
            {
                IsLoggedIn = false;
            }

            return IsLoggedIn;
        }

        public async void Logout(bool uninstall)
        {
            if (IsLoggedIn)
            {
                if (uninstall)
                {
                    try
                    {
                        Uri requestUri = new Uri(
                                string.Format("{0}me/permissions?access_token={1}", WSAFacebookConstants.GraphApiUri, _accessToken));

                        using (HttpClient client = new HttpClient())
                        {
                            HttpResponseMessage message = await client.DeleteAsync(requestUri);

                            string response = await message.Content.ReadAsStringAsync();
                        }
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

                Uri requestUri = new Uri(
                    string.Format("{0}me?fields={1}&access_token={2}", WSAFacebookConstants.GraphApiUri, fields, _accessToken));


                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(requestUri);

                        string responseAsString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            userDetailsResponse.Data = JsonConvert.DeserializeObject<WSAFacebookUser>(responseAsString);
                            userDetailsResponse.Success = true;
                        }
                        else
                        {
                            WSAFacebookError errorMessage = JsonConvert.DeserializeObject<WSAFacebookError>(responseAsString);

                            if (errorMessage.Code == _authenticationErrorCode)
                            {
                                IsLoggedIn = false;
                                _accessToken = null;
                                errorMessage.AccessTokenExpired = true;
                            }

                            userDetailsResponse.Success = false;
                            userDetailsResponse.Error = errorMessage;
                        }
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
                Uri requestUri = new Uri(string.Format("{0}me/likes/{1}?access_token={2}", WSAFacebookConstants.GraphApiUri, pageId, _accessToken));

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(requestUri);

                        string responseAsString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            WSAFacebookDataResponse parsedResponse = JsonConvert.DeserializeObject<WSAFacebookDataResponse>(responseAsString);

                            hasUserLikedPageResponse.Data = parsedResponse.Data != null;
                            hasUserLikedPageResponse.Success = true;
                        }
                        else
                        {
                            WSAFacebookError errorMessage = JsonConvert.DeserializeObject<WSAFacebookError>(responseAsString);

                            if (errorMessage.Code == _authenticationErrorCode)
                            {
                                IsLoggedIn = false;
                                _accessToken = null;
                                errorMessage.AccessTokenExpired = true;
                            }

                            hasUserLikedPageResponse.Success = false;
                            hasUserLikedPageResponse.Error = errorMessage;
                        }
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

        public async Task<WSAFacebookResponse<T>> GraphApiRead<T>(string edge, Dictionary<string, string> parameters)
        {
            WSAFacebookResponse<T> graphApiReadResponse = new WSAFacebookResponse<T>();

            if (IsLoggedIn)
            {
                string fields = string.Empty;

                if(parameters != null && parameters.Count > 0)
                {
                    fields = parameters.Aggregate(string.Empty, (total, next) => total += (next.Key + "=" + next.Value + "&"));
                }

                Uri requestUri = new Uri(string.Format("{0}{1}?{2}access_token={3}", WSAFacebookConstants.GraphApiUri, edge, fields, _accessToken));

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(requestUri);

                        string responseAsString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            graphApiReadResponse.Data = JsonConvert.DeserializeObject<T>(responseAsString);
                            graphApiReadResponse.Success = true;
                        }
                        else
                        {
                            WSAFacebookError errorMessage = JsonConvert.DeserializeObject<WSAFacebookError>(responseAsString);

                            if (errorMessage.Code == _authenticationErrorCode)
                            {
                                IsLoggedIn = false;
                                _accessToken = null;
                                errorMessage.AccessTokenExpired = true;
                            }

                            graphApiReadResponse.Success = false;
                            graphApiReadResponse.Error = errorMessage;
                        }
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

        public void ShowFeedDialog(string link, string picture, string source, string name, string caption, string description, Action closed)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link },
                { "picture", picture },
                { "source", source },
                { "name", name },
                { "caption", caption },
                { "description", description }
            };

            string feedBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_url={2}", WSAFacebookConstants.FeedDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            ShowDialog(parameters, feedBaseUri, closed);
        }

        public void ShowRequestDialog(string title, string message, Action closed)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "title", title },
                { "message", message },
            };

            string requestBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.RequestDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            ShowDialog(parameters, requestBaseUri, closed);
        }

        public void ShowSendDialog(string link, Action closed)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "link", link }
            };

            string sendBaseUri = string.Format("{0}?app_id={1}&display=popup&redirect_uri={2}", WSAFacebookConstants.SendDialogUri, _facebookAppId, WSAFacebookConstants.WebRedirectUri);

            ShowDialog(parameters, sendBaseUri, closed);
        }

        private void ShowDialog(Dictionary<string, string> parameters, string baseUri, Action closed)
        {
            if (!IsDialogOpen)
            {
                FacebookDialog dialog = new FacebookDialog(Screen.width, Screen.height);

                if (_dxSwapChainPanel != null)
                {
                    IsDialogOpen = true;

                    if (closed != null)
                    {
                        UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                        {
                            dialog.Closed += () => { IsDialogOpen = false; closed(); };
                        }, false);
                    }

                    dialog.Show(baseUri, parameters, WSAFacebookConstants.SendDialogResponseUri, _dxSwapChainPanel);
                }
            }
            else
            {
                if (closed != null)
                {
                    UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                    {
                        closed();
                    }, false);
                }
            }
        }
    }
}
#endif