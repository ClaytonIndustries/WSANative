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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace CI.WSANative.Twitter.Core
{
    public class WSATwitterApi
    {
        private string _oauthToken = "";
        private string _oauthTokenSecret = "";
        private string _oauthVerifier = "";

        private string _userId = "";
        private string _screenName = "";

        private WSATwitterHeaderGenerator _headerGenerator;

        public void Initialise(string consumerKey, string consumerSecret)
        {
            _headerGenerator = new WSATwitterHeaderGenerator(consumerKey, consumerSecret);
        }

        public async Task<WSATwitterLoginResult> Login()
        {
            string callback = "https://www.twitter.co.uk";

            if (!await GetRequestToken(callback) || !await UserLogin(callback) || !await GetAccessToken())
            {
                return new WSATwitterLoginResult()
                {
                    Success = false,
                };
            }
            else
            {
                return new WSATwitterLoginResult()
                {
                    Success = true,
                    AccessToken = _oauthToken,
                    UserId = _userId,
                    ScreenName = _screenName
                };
            }
        }

        public async Task<WSATwitterResponse> GetUserDetails(bool includeEmail)
        {
            WSATwitterResponse wsaTwitterResponse = new WSATwitterResponse()
            {
                Success = false
            };

            try
            {
                Dictionary<string, string> additionalParts = new Dictionary<string, string>()
                {
                    { "oauth_token", _oauthToken }
                };

                Dictionary<string, string> signatureOnlyParts = new Dictionary<string, string>()
                {
                    { "include_email", includeEmail ? "true" : "false" }
                };

                string authHeader = _headerGenerator.Generate("GET", "https://api.twitter.com/1.1/account/verify_credentials.json", additionalParts, signatureOnlyParts, _oauthTokenSecret);

                HttpResponseMessage response = await MakeRequest(CombineUrlAndQuery("https://api.twitter.com/1.1/account/verify_credentials.json", signatureOnlyParts), authHeader, null, HttpMethod.Get);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    wsaTwitterResponse.Success = true;
                    wsaTwitterResponse.Data = content;
                }
            }
            catch
            {
            }

            return wsaTwitterResponse;
        }

        private async Task<bool> GetRequestToken(string callback)
        {
            try
            {
                Dictionary<string, string> additionalParts = new Dictionary<string, string>()
                {
                    { "oauth_callback", callback }
                };

                string authHeader = _headerGenerator.Generate("POST", "https://api.twitter.com/oauth/request_token", additionalParts, null, null);

                HttpResponseMessage response = await MakeRequest("https://api.twitter.com/oauth/request_token", authHeader, null, HttpMethod.Post);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                string content = await response.Content.ReadAsStringAsync();

                Dictionary<string, string> parsed = ParseResponse(content);

                if (!parsed.ContainsKey("oauth_callback_confirmed") || parsed["oauth_callback_confirmed"] == "false" || !parsed.ContainsKey("oauth_token") || !parsed.ContainsKey("oauth_token_secret"))
                {
                    return false;
                }

                _oauthToken = parsed["oauth_token"];
                _oauthTokenSecret = parsed["oauth_token_secret"];

                return true;
            }
            catch
            {
            }

            return false;
        }

        private async Task<bool> UserLogin(string callback)
        {
            try
            {
                Uri requestUri = new Uri(string.Format("https://api.twitter.com/oauth/authenticate?oauth_token={0}", _oauthToken));

                Uri callbackUri = new Uri(callback);

                WebAuthenticationResult auth = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, requestUri, callbackUri);

                if (auth.ResponseStatus != WebAuthenticationStatus.Success)
                {
                    return false;
                }

                Dictionary<string, string> parsed = ParseResponse(auth.ResponseData);

                if ((parsed.ContainsKey("oauth_token") && parsed["oauth_token"] != _oauthToken) || !parsed.ContainsKey("oauth_verifier"))
                {
                    return false;
                }

                _oauthVerifier = parsed["oauth_verifier"];

                return true;
            }
            catch
            {
            }

            return false;
        }

        private async Task<bool> GetAccessToken()
        {
            try
            {
                Dictionary<string, string> additionalParts = new Dictionary<string, string>()
                {
                    { "oauth_token", _oauthToken }
                };

                string authHeader = _headerGenerator.Generate("POST", "https://api.twitter.com//oauth/access_token", additionalParts, null, null);

                Dictionary<string, string> body = new Dictionary<string, string>()
                {
                    { "oauth_verifier", _oauthVerifier }
                };

                HttpResponseMessage response = await MakeRequest("https://api.twitter.com//oauth/access_token", authHeader, new FormUrlEncodedContent(body), HttpMethod.Post);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                string content = await response.Content.ReadAsStringAsync();

                Dictionary<string, string> parsed = ParseResponse(content);

                if (!parsed.ContainsKey("oauth_token") || !parsed.ContainsKey("oauth_token_secret"))
                {
                    return false;
                }

                if (parsed.ContainsKey("user_id"))
                {
                    _userId = parsed["user_id"];
                }

                if (parsed.ContainsKey("screen_name"))
                {
                    _screenName = parsed["screen_name"];
                }

                _oauthToken = parsed["oauth_token"];
                _oauthTokenSecret = parsed["oauth_token_secret"];

                return true;
            }
            catch
            {
            }

            return false;
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

        private Dictionary<string, string> ParseResponse(string response)
        {
            string[] parts = response.Split('&');

            Dictionary<string, string> kvps = new Dictionary<string, string>();

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

        private string CombineUrlAndQuery(string url, Dictionary<string, string> items)
        {
            return url + "?" + string.Join("&", items.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
        }
    }
}
#endif