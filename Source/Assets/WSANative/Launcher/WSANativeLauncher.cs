////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using CI.WSANative.Common;
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
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri(uri));
            });
#endif
        }
    }
}