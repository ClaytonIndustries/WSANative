////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace CI.WSANative.Personalisation
{
    public static class WSANativePersonalisation
    {
        /// <summary>
        /// For internal use only
        /// </summary>
        public static Func<Color> _GetAccentColour;

        /// <summary>
        /// Attempts to return the system accent colour
        /// </summary>
        /// <returns>The accent colour or white if it fails</returns>
        public static Color GetSystemAccentColour()
        {
#if NETFX_CORE
            if (_GetAccentColour != null)
            {
                return _GetAccentColour();
            }
#endif
            return Color.white;
        }
    }
}