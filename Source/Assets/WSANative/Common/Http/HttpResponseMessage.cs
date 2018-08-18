using System.Net;

namespace CI.WSANative.Common.Http
{
    public class HttpResponseMessage
    {
        public string Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccessStatusCode
        {
            get { return ((int)StatusCode >= 200) && ((int)StatusCode <= 299); }
        }
    }
}