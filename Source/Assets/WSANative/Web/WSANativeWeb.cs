using System;
using System.Collections.Generic;

#if NETFX_CORE
using System.Net.Http;
#endif

namespace CI.WSANative.Web
{
    public static class WSANativeWeb
    {
        /// <summary>
        /// Sends a GET request to the specified url and returns whether the request succeeded along with the response data as a string
        /// </summary>
        /// <param name="url">The url</param>
        /// <param name="response">A callback containing the response</param>
        public static void GetString(string url, Action<bool, string> response)
        {
#if NETFX_CORE
            GetStringAsync(url, response);
#endif
        }

#if NETFX_CORE
        private static async void GetStringAsync(string url, Action<bool, string> response)
        {
            string result = string.Empty;
            bool isSuccess = false;

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(url);

                    if(responseMessage.IsSuccessStatusCode)
                    {
                        result = await responseMessage.Content.ReadAsStringAsync();
                        isSuccess = true;
                    }
                }
                catch
                {
                }

                if(response != null)
                {
                    response(isSuccess, result);
                }
            }
        }
#endif

        /// <summary>
        /// Sends a GET request to the specified url and returns whether the request succeeded along with the response data as a byte array
        /// </summary>
        /// <param name="url">The url</param>
        /// <param name="response">A callback containing the response</param>
        public static void GetBytes(string url, Action<bool, byte[]> response)
        {
#if NETFX_CORE
            GetBytesAsync(url, response);
#endif
        }

#if NETFX_CORE
        private static async void GetBytesAsync(string url, Action<bool, byte[]> response)
        {
            byte[] result = null;
            bool isSuccess = false;

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage responseMessage = await client.GetAsync(url);

                    if(responseMessage.IsSuccessStatusCode)
                    {
                        result = await responseMessage.Content.ReadAsByteArrayAsync(); 
                        isSuccess = true;
                    }     
                }
                catch
                {
                }

                if(response != null)
                {
                    response(isSuccess, result);
                }
            }
        }
#endif

        /// <summary>
        /// Sends a POST request to the specified url and returns whether the request succeeded along with the response data as a string
        /// </summary>
        /// <param name="url">The url</param>
        /// <param name="keyValuePairs">A callback containing the response</param>
        public static void PostReturnString(string url, Dictionary<string, string> content, Action<bool, string> response)
        {
#if NETFX_CORE
            PostReturnStringAsync(url, content, response);
#endif
        }

#if NETFX_CORE
        private static async void PostReturnStringAsync(string url, Dictionary<string, string> content, Action<bool, string> response)
        {
            string result = string.Empty;
            bool isSuccess = false;

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsync(url, new FormUrlEncodedContent(content));
        
                    if(responseMessage.IsSuccessStatusCode)
                    {
                        result = await responseMessage.Content.ReadAsStringAsync();
                        isSuccess = true;
                    } 
                }
                catch
                {
                }

                if(response != null)
                {
                    response(isSuccess, result);
                }
            }
        }
#endif

        /// <summary>
        /// Sends a POST request to the specified url and returns whether the request succeeded along with the response data as a byte array
        /// </summary>
        /// <param name="url">The url</param>
        /// <param name="keyValuePairs">A callback containing the response</param>
        public static void PostReturnBytes(string url, Dictionary<string, string> content, Action<bool, byte[]> response)
        {
#if NETFX_CORE
            PostReturnBytesAsync(url, content, response);
#endif
        }

#if NETFX_CORE
        private static async void PostReturnBytesAsync(string url, Dictionary<string, string> content, Action<bool, byte[]> response)
        {
            byte[] result = null;
            bool isSuccess = false;

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage responseMessage = await client.PostAsync(url, new FormUrlEncodedContent(content));
        
                    if(responseMessage.IsSuccessStatusCode)
                    {
                        result = await responseMessage.Content.ReadAsByteArrayAsync();
                        isSuccess = true;
                    } 
                }
                catch
                {
                }

                if(response != null)
                {
                    response(isSuccess, result);
                }
            }
        }
#endif
    }
}