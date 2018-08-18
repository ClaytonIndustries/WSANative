#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CI.WSANative.Common.Http
{
    public class HttpClient
    {
        public async Task<HttpResponseMessage> Get(string url)
        {
            return await MakeRequest(url, "GET");
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            return await MakeRequest(url, "DELETE");
        }

        private async Task<HttpResponseMessage> MakeRequest(string url, string method)
        {
            HttpWebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = method;

                try
                {
                    response = (HttpWebResponse)await request.GetResponseAsync();
                }
                catch (AggregateException e)
                {
                    if (e.InnerExceptions.Any() && e.InnerExceptions.First() is WebException)
                    {
                        response = (HttpWebResponse)(e.InnerExceptions.First() as WebException).Response;
                    }
                    else
                    {
                        throw;
                    }
                }

                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string result = await streamReader.ReadToEndAsync();

                    return new HttpResponseMessage()
                    {
                        Data = result,
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (Exception e)
            {
                return new HttpResponseMessage()
                {
                    StatusCode = GetStatusCode(e, response)
                };
            }
        }

        private HttpStatusCode GetStatusCode(Exception exception, HttpWebResponse response)
        {
            if (response != null)
            {
                return response.StatusCode;
            }

            if (exception.Message.Contains("The remote server returned an error:"))
            {
                int statusCode = 0;

                Match match = Regex.Match(exception.Message, "\\(([0-9]+)\\)");

                if (match.Groups.Count == 2 && int.TryParse(match.Groups[1].Value, out statusCode))
                {
                    return (HttpStatusCode)statusCode;
                }
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}
#endif