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
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Storage;
using CI.WSANative.Facebook.Core;
using Newtonsoft.Json;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookApi
    {
        public bool IsLoggedIn { get; private set; }

        private string _facebookAppId;
        private string _packageSID;
        private string _accessToken;

        private const string _facebookGraphUri = "https://graph.facebook.com/v2.7/";
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
                                string.Format("{0}me/permissions?access_token={1}", _facebookGraphUri, _accessToken));

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
                    string.Format("{0}me?fields={1}&access_token={2}", _facebookGraphUri, fields, _accessToken));


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
                Uri requestUri = new Uri(string.Format("{0}me/likes/{1}?access_token={2}", _facebookGraphUri, pageId, _accessToken));

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

        public async Task<WSAFacebookResponse<bool>> PostPhotoToTimeline(string caption, byte[] photo)
        {
            WSAFacebookResponse<bool> postPhotoResponse = new WSAFacebookResponse<bool>();

            if (IsLoggedIn)
            {
                Uri requestUri = new Uri(string.Format("{0}me/photos?access_token={1}", _facebookGraphUri, _accessToken));

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        MultipartFormDataContent content = new MultipartFormDataContent();
                        content.Add(new StringContent(caption));
                        content.Add(new ByteArrayContent(photo));

                        HttpResponseMessage response = await client.PostAsync(requestUri, content);

                        string responseAsString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            WSAFacebookDataResponse parsedResponse = JsonConvert.DeserializeObject<WSAFacebookDataResponse>(responseAsString);

                            postPhotoResponse.Data = parsedResponse.Data != null;
                            postPhotoResponse.Success = true;
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

                            postPhotoResponse.Success = false;
                            postPhotoResponse.Error = errorMessage;
                        }
                    }
                }
                catch
                {
                    postPhotoResponse.Success = false;
                }
            }
            else
            {
                postPhotoResponse.Success = false;
                postPhotoResponse.Error = new WSAFacebookError()
                {
                    AccessTokenExpired = true
                };
            }

            return postPhotoResponse;
        }
    }
}
#endif