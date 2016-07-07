////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace CI.WSANative.Mapping
{
    public static class WSANativeMap
    {
        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSAMapSettings> Create;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSAMapSettings> CenterMapToLocation;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action<WSAMapItem> AddElementToMap;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action ClearMapElements;

        /// <summary>
        /// For internal use only
        /// </summary>
        public static Action Destroy;

        /// <summary>
        /// Create a map
        /// </summary>
        /// <param name="mapServiceToken">Your bing maps auth token - register on the bing maps portal (can leave blank for testing)</param>
        /// <param name="width">Width of the map in pixels</param>
        /// <param name="height">Height of the map in pixels</param>
        /// <param name="position">Map position in pixels offset from the top left corner</param>
        /// <param name="center">Center the map on this lat / long</param>
        /// <param name="zoomLevel">How zoomed in should the map initially be (1 - 20)</param>
        /// <param name="interactionMode">How should the user be allowed to interact with the map</param>
        public static void CreateMap(string mapServiceToken, int width, int height, WSAPosition position, WSAGeoPoint center, int zoomLevel, WSAMapInteractionMode interactionMode)
        {
#if NETFX_CORE
            if (Create != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Create(new WSAMapSettings() { MapServiceToken = mapServiceToken, Width = width, Height = height, Position = position, Center = center, ZoomLevel = zoomLevel, InteractionMode = interactionMode });
                }, false);
            }
#endif
        }

        /// <summary>
        /// Center the map to a specified location and optionally zoom in or out
        /// </summary>
        /// <param name="position">Center the map to this lat / long</param>
        /// <param name="zoomLevel">(don't specify to leave as is) Zoom in or out</param>
        public static void CenterMap(WSAGeoPoint position, int zoomLevel = -1)
        {
#if NETFX_CORE
            if (CenterMapToLocation != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    CenterMapToLocation(new WSAMapSettings() { Center = position, ZoomLevel = zoomLevel });
                }, false);
            }
#endif
        }

        /// <summary>
        /// Add an element to the map
        /// </summary>
        /// <param name="title">Title to show on the POI</param>
        /// <param name="location">Location of the POI in lat / long</param>
        /// <param name="imageUri">(don't specify to use default image) Uri of an image to use, image must exist in the Assets folder of the built vs solution. 
        /// If your image was called test.png and was in the folder Assets/MapIcons/ you would specify MapIcons/test.png
        /// </param>
        public static void AddMapElement(string title, WSAGeoPoint location, string imageUri = null)
        {
#if NETFX_CORE
            if (AddElementToMap != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    AddElementToMap(new WSAMapItem() { Title = title, Location = location, ImageUri = imageUri });
                }, false);
            }
#endif
        }

        /// <summary>
        /// Clear the map of all elements
        /// </summary>
        public static void ClearMap()
        {
#if NETFX_CORE
            if (ClearMapElements != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    ClearMapElements();
                }, false);
            }
#endif
        }

        /// <summary>
        /// Destroy the map
        /// </summary>
        public static void DestroyMap()
        {
#if NETFX_CORE
            if (Destroy != null)
            {
                UnityEngine.WSA.Application.InvokeOnUIThread(() =>
                {
                    Destroy();
                }, false);
            }
#endif
        }
    }
}