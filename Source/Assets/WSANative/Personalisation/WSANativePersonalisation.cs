////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

#if ENABLE_WINMD_SUPPORT
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Personalisation
{
    public static class WSANativePersonalisation
    {
        /// <summary>
        /// Attempts to return the system accent colour
        /// </summary>
        /// <returns>The accent colour or white if it fails</returns>
        public static Color GetSystemAccentColour()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured())
            {
                Windows.UI.Color colour = new Windows.UI.Color();
                ThreadRunner.RunOnUIThread(() =>
                {
                    colour = (Windows.UI.Color)WSANativeCore.DxSwapChainPanel.Resources["SystemAccentColor"];
                }, true);
                return new Color(colour.R, colour.G, colour.B, colour.A);
            }
#endif
            return Color.white;
        }
    }
}