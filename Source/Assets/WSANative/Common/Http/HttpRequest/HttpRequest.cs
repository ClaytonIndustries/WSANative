using System;
using System.Net;

namespace CI.WSANative.Common.Http
{
    public class HttpRequest : HttpBase
    {
        public HttpRequest(HttpAction httpAction, HttpWebRequest request)
        {
            _request = request;

            SetMethod(httpAction);
        }

        public void Execute(HttpCompletionOption completionOption, Action<HttpResponseMessage> responseCallback, int downloadBlockSize)
        {
            try
            {
                HandleResponseRead(responseCallback, completionOption, downloadBlockSize);
            }
            catch (Exception e)
            {
                RaiseErrorResponse(responseCallback, e);
            }
        }

        public void Execute(IHttpContent content, HttpCompletionOption completionOption, Action<HttpResponseMessage> responseCallback, Action<UploadStatusMessage> uploadStatusCallback,
            int downloadBlockSize, int uploadBlockSize)
        {
            try
            {
                SetContentHeaders(content);

                HandleRequestWrite(content, uploadStatusCallback, uploadBlockSize);
                HandleResponseRead(responseCallback, completionOption, downloadBlockSize);
            }
            catch (Exception e)
            {
                RaiseErrorResponse(responseCallback, e);
            }
        }
    }
}