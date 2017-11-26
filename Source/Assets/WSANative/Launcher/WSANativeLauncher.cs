////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
#endif

namespace CI.WSANative.Launchers
{
    public static class WSANativeLauncher
    {
        /// <summary>
        /// Starts the default app associated with the uri scheme name for the specified uri (for example specifying a url will launch the default browser)
        /// </summary>
        /// <param name="uri">The uri</param>
        public static void LaunchUri(string uri)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(uri));
            }, false);
#endif
        }
    }
}