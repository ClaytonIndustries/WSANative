////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System.Runtime.InteropServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Common
{
    public static class WSANativeCore
    {
#if ENABLE_WINMD_SUPPORT
        [DllImport("__Internal")]
        private static extern int GetPageContent([MarshalAs(UnmanagedType.IInspectable)]object frame, [MarshalAs(UnmanagedType.IInspectable)]out object pageContent);

        public static SwapChainPanel DxSwapChainPanel { get; private set; }

        private static bool _isInitialised;
#endif

        /// <summary>
        /// Initialises WSANAtive - call this once when your app first starts up
        /// </summary>
        public static void Initialise()
        {
#if ENABLE_WINMD_SUPPORT
            if (!_isInitialised)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    object pageContent;
                    var result = GetPageContent(Window.Current.Content, out pageContent);
                    if (result < 0)
                    {
                        Marshal.ThrowExceptionForHR(result);
                    }
                    DxSwapChainPanel = pageContent as SwapChainPanel;
                    _isInitialised = DxSwapChainPanel != null;
                });
            }
#endif
        }

        /// <summary>
        /// For internal use only
        /// </summary>
        /// <returns></returns>
        public static bool IsDxSwapChainPanelConfigured()
        {
#if ENABLE_WINMD_SUPPORT
            return DxSwapChainPanel != null;
#else
            return false;
#endif
        }
    }
}