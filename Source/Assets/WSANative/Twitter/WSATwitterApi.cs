////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE && UNITY_WSA_10_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using Windows.Security.Authentication.Web;
using Windows.Storage;

namespace CI.WSANative.Twitter.Core
{
    public class WSATwitterApi
    {
        public bool IsLoggedIn { get; private set; }

        private string _oauthToken;
        private string _oauthTokenSecret;
        private string _oauthCallback;

        private WSATwitterHeaderGenerator _headerGenerator;
        private Windows.UI.Xaml.Controls.Grid _dxSwapChainPanel;

        private const string _savedDataFilename = "TwitterData.sav";

        public WSATwitterApi()
        {
            Startup();
        }

        private async void Startup()
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(_savedDataFilename);

                string content = await FileIO.ReadTextAsync(file);

                string[] items = content.Split('&');

                if(items.Length == 2)
                {
                    _oauthToken = items[0];
                    _oauthTokenSecret = items[1];
                }

                IsLoggedIn = true;
            }
            catch
            {
            }
        }

        public void Initialise(string consumerKey, string consumerSecret, string oauthCallback)
        {
            _oauthCallback = oauthCallback;

            _headerGenerator = new WSATwitterHeaderGenerator(consumerKey, consumerSecret);
        }

        public void ConfigureDialogs(Windows.UI.Xaml.Controls.Grid dxSwapChainPanel)
        {
            _dxSwapChainPanel = dxSwapChainPanel;
        }

        public async Task<WSATwitterLoginResult> Login()
        {
            Logout();

            WSATwitterLoginResult result = new WSATwitterLoginResult()
            {
                Success = true
            };

            result = await GetRequestToken(result);
            if (!result.Success)
            {
                return result;
            }

            var userLoginResult = await UserLogin(result);
            if (!userLoginResult.Item1.Success)
            {
                return userLoginResult.Item1;
            }

            result = await GetAccessToken(userLoginResult.Item1, userLoginResult.Item2);
            if (!result.Success)
            {
                return result;
            }

            IsLoggedIn = true;

            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(_savedDataFilename, CreationCollisionOption.ReplaceExisting);

                await FileIO.WriteTextAsync(file, string.Format("{0}&{1}", _oauthToken, _oauthTokenSecret));
            }
            catch
            {
            }

            return result;
        }

        public async void Logout()
        {
            if (IsLoggedIn)
            {
                _oauthToken = null;
                _oauthTokenSecret = null;
                IsLoggedIn = false;

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

        public async Task<WSATwitterResponse> ApiRead(string url, IDictionary<string, string> parameters)
        {
            WSATwitterResponse wsaTwitterResponse = new WSATwitterResponse()
            {
                Success = false
            };

            if (!IsLoggedIn)
            {
                wsaTwitterResponse.Error = new WSATwitterError()
                {
                    Message = "User is not logged in",
                    Unauthorised = true
                };

                return wsaTwitterResponse;
            }

            try
            {
                IDictionary<string, string> additionalParts = new Dictionary<string, string>()
                {
                    { "oauth_token", _oauthToken }
                };

                string authHeader = _headerGenerator.Generate("GET", url, additionalParts, parameters, _oauthTokenSecret);

                HttpResponseMessage response = await MakeRequest(CombineUrlAndQuery(url, parameters), authHeader, null, HttpMethod.Get);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    wsaTwitterResponse.Success = true;
                    wsaTwitterResponse.Data = content;
                }
                else
                {
                    wsaTwitterResponse.Error = new WSATwitterError()
                    {
                        Message = await response.Content.ReadAsStringAsync()
                    };

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Logout();

                        wsaTwitterResponse.Error.Unauthorised = true;
                    }
                }
            }
            catch (Exception e)
            {
                wsaTwitterResponse.Error = new WSATwitterError()
                {
                    Message = e.Message,
                    Unauthorised = true
                };
            }

            return wsaTwitterResponse;
        }

        public void ShowTweetDialog(string baseUrl, IDictionary<string, string> parameters, Action closed)
        {
            if (_dxSwapChainPanel == null)
            {
                throw new InvalidOperationException("WSATwitterApi.ConfigureDialogs must first be called from MainPage.xaml.cs");
            }

            TwitterWebIntent dialog = new TwitterWebIntent(Screen.width, Screen.height);

            dialog.Show(baseUrl, parameters, _dxSwapChainPanel, closed);
        }

        private async Task<WSATwitterLoginResult> GetRequestToken(WSATwitterLoginResult result)
        {
            try
            {
                IDictionary<string, string> additionalParts = new Dictionary<string, string>()
                {
                    { "oauth_callback", _oauthCallback }
                };

                string authHeader = _headerGenerator.Generate("POST", "https://api.twitter.com/oauth/request_token", additionalParts, null, null);

                HttpResponseMessage response = await MakeRequest("https://api.twitter.com/oauth/request_token", authHeader, null, HttpMethod.Post);

                if (!response.IsSuccessStatusCode)
                {
                    result.Success = false;
                    result.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return result;
                }

                string content = await response.Content.ReadAsStringAsync();

                IDictionary<string, string> parsed = ParseResponse(content);

                if (!parsed.ContainsKey("oauth_callback_confirmed") || parsed["oauth_callback_confirmed"] == "false" || !parsed.ContainsKey("oauth_token") || !parsed.ContainsKey("oauth_token_secret"))
                {
                    result.Success = false;
                    result.ErrorMessage = "GetRequestToken response does not contain the expected properties";
                    return result;
                }

                _oauthToken = parsed["oauth_token"];
                _oauthTokenSecret = parsed["oauth_token_secret"];

                return result;
            }
            catch(Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
                return result;
            }
        }

        private async Task<Tuple<WSATwitterLoginResult, string>> UserLogin(WSATwitterLoginResult result)
        {
            try
            {
                Uri requestUri = new Uri(string.Format("https://api.twitter.com/oauth/authenticate?oauth_token={0}", _oauthToken));

                Uri callbackUri = new Uri(_oauthCallback);

                WebAuthenticationResult authentication = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);

                if (authentication.ResponseStatus != WebAuthenticationStatus.Success)
                {
                    result.Success = false;
                    result.ErrorMessage = authentication.ResponseStatus.ToString();
                    return new Tuple<WSATwitterLoginResult, string>(result, null);
                }

                IDictionary<string, string> parsed = ParseResponse(authentication.ResponseData);

                if ((parsed.ContainsKey("oauth_token") && parsed["oauth_token"] != _oauthToken) || !parsed.ContainsKey("oauth_verifier"))
                {
                    result.Success = false;
                    result.ErrorMessage = "UserLogin response does not contain the expected properties";
                    return new Tuple<WSATwitterLoginResult, string>(result, null);
                }

                string oauthVerifier = parsed["oauth_verifier"];

                return new Tuple<WSATwitterLoginResult, string>(result, oauthVerifier);
            }
            catch(Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
                return new Tuple<WSATwitterLoginResult, string>(result, null);
            }
        }

        private async Task<WSATwitterLoginResult> GetAccessToken(WSATwitterLoginResult result, string oauthVerifier)
        {
            try
            {
                IDictionary<string, string> additionalParts = new Dictionary<string, string>()
                {
                    { "oauth_token", _oauthToken }
                };

                string authHeader = _headerGenerator.Generate("POST", "https://api.twitter.com//oauth/access_token", additionalParts, null, null);

                IDictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "oauth_verifier", oauthVerifier }
                };

                HttpResponseMessage response = await MakeRequest("https://api.twitter.com//oauth/access_token", authHeader, new FormUrlEncodedContent(body), HttpMethod.Post);

                if (!response.IsSuccessStatusCode)
                {
                    result.Success = false;
                    result.ErrorMessage = await response.Content.ReadAsStringAsync();
                    return result;
                }

                string content = await response.Content.ReadAsStringAsync();

                IDictionary<string, string> parsed = ParseResponse(content);

                if (!parsed.ContainsKey("oauth_token") || !parsed.ContainsKey("oauth_token_secret"))
                {
                    result.Success = false;
                    result.ErrorMessage = "GetAccessToken response does not contain the expected properties";
                    return result;
                }

                if (parsed.ContainsKey("user_id"))
                {
                    result.UserId = parsed["user_id"];
                }

                if (parsed.ContainsKey("screen_name"))
                {
                    result.ScreenName = parsed["screen_name"];
                }

                _oauthToken = parsed["oauth_token"];
                _oauthTokenSecret = parsed["oauth_token_secret"];

                return result;
            }
            catch(Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;
                return result;
            }
        }

        private async Task<HttpResponseMessage> MakeRequest(string url, string authHeader, HttpContent content, HttpMethod method)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Authorization", authHeader);

                if (method == HttpMethod.Post)
                {
                    return await client.PostAsync(url, content);
                }
                else
                {
                    return await client.GetAsync(url);
                }
            }
        }

        private IDictionary<string, string> ParseResponse(string response)
        {
            string[] parts = response.Split('&');

            IDictionary<string, string> kvps = new Dictionary<string, string>();

            foreach (string item in parts)
            {
                string[] pairs = item.Split('=');

                if (pairs.Length == 2)
                {
                    kvps.Add(pairs[0], pairs[1]);
                }
            }

            return kvps;
        }

        private string CombineUrlAndQuery(string url, IDictionary<string, string> items)
        {
            if (items == null || items.Count == 0)
            {
                return url;
            }

            return url + "?" + string.Join("&", items.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        }
    }
}
#endif