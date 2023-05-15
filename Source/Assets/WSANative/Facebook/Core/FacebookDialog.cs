////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AOT;

namespace CI.WSANative.Facebook.Core
{
    public sealed class FacebookDialog
    {
        private delegate void FacebookCallbackDialogResponseDelegate([MarshalAs(UnmanagedType.LPWStr)]string response);

        [DllImport("__Internal")]
        private static extern void _FacebookDialogShow(FacebookCallbackDialogResponseDelegate callback, int screenWidth, int screenHeight, [MarshalAs(UnmanagedType.LPWStr)]string requestUri, [MarshalAs(UnmanagedType.LPWStr)]string responseUri, bool delayDialog);

		private static TaskCompletionSource<string> _taskCompletionSource;

        public FacebookDialog()
        {		
			_taskCompletionSource = new TaskCompletionSource<string>();
        }

        public async Task<string> Show(int screenWidth, int screenHeight, string requestUri, string responseUri, bool delayDialog)
        {
            _FacebookDialogShow(FacebookDialogCallback, screenWidth, screenHeight, requestUri, responseUri, delayDialog);

            return await _taskCompletionSource.Task;
        }

        [MonoPInvokeCallback(typeof(FacebookCallbackDialogResponseDelegate))]
        private static void FacebookDialogCallback(string response)
        {
            _taskCompletionSource.SetResult(response);
        }
    }
}
#endif