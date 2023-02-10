////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if ENABLE_WINMD_SUPPORT
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Mapping
{
    public static class WSANativeMap
    {
        /// <summary>
        /// Raised when the user taps the map
        /// </summary>
        public static Action<WSAGeoPoint> MapTapped
        {
            get; set;
        }

        /// <summary>
        /// Raised when the user taps a map element
        /// </summary>
        public static Action<string, WSAGeoPoint> MapElementTapped
        {
            get; set;
        }

#if ENABLE_WINMD_SUPPORT
        private static MapControl _mapControl;
#endif

        /// <summary>
        /// Create a map
        /// </summary>
        /// <param name="settings">Settings to configure the map</param>
        public static void CreateMap(WSAMapSettings settings)
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mapControl == null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mapControl = new MapControl();
                    _mapControl.Width = settings.HorizontalPlacement == WSAHorizontalPlacement.Stretch ? double.NaN : settings.Width;
                    _mapControl.Height = settings.VerticalPlacement == WSAVerticalPlacement.Stretch ? double.NaN : settings.Height;
                    _mapControl.Margin = settings.Margin != null ? new Thickness(settings.Margin.Left, settings.Margin.Top, settings.Margin.Right, settings.Margin.Bottom) : new Thickness();
                    _mapControl.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = settings.Centre.Latitude, Longitude = settings.Centre.Longitude });
                    _mapControl.ZoomLevel = settings.ZoomLevel;
                    _mapControl.HorizontalAlignment = (HorizontalAlignment)settings.HorizontalPlacement;
                    _mapControl.VerticalAlignment = (VerticalAlignment)settings.VerticalPlacement;
                    _mapControl.ZoomInteractionMode = settings.InteractionMode == WSAMapInteractionMode.GestureAndControl ? MapInteractionMode.GestureAndControl : settings.InteractionMode == WSAMapInteractionMode.GestureOnly ? MapInteractionMode.GestureOnly : MapInteractionMode.Disabled;
                    _mapControl.TiltInteractionMode = settings.InteractionMode == WSAMapInteractionMode.GestureAndControl ? MapInteractionMode.GestureAndControl : settings.InteractionMode == WSAMapInteractionMode.GestureOnly ? MapInteractionMode.GestureOnly : MapInteractionMode.Disabled;
                    _mapControl.MapTapped += (s, e) => { if (MapTapped != null) { MapTapped(new WSAGeoPoint() { Latitude = e.Location.Position.Latitude, Longitude = e.Location.Position.Longitude }); } };
                    _mapControl.MapElementClick += (s, e) => 
                    { 
                        if (MapElementTapped != null) 
                        { 
                            var mapIcon = e.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
                            if (mapIcon != null)
                            {
                                MapElementTapped(mapIcon.Title, new WSAGeoPoint() { Latitude = mapIcon.Location.Position.Latitude, Longitude = mapIcon.Location.Position.Longitude });
                            }
                        } 
                    };
                    WSANativeCore.DxSwapChainPanel.Children.Add(_mapControl);
                });
            }
#endif
        }

        /// <summary>
        /// Center the map to a specified location and optionally zoom in or out
        /// </summary>
        /// <param name="location">Center the map to this lat / long</param>
        /// <param name="zoomLevel">Zoom in or out (don't specify to leave as is) </param>
        public static void CenterMap(WSAGeoPoint location, int zoomLevel = -1)
        {
#if ENABLE_WINMD_SUPPORT
            if (_mapControl != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mapControl.Center = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude });
                    if (zoomLevel >= 0)
                    {
                        _mapControl.ZoomLevel = zoomLevel;
                    }
                });
            }
#endif
        }

        /// <summary>
        /// Add an element to the map
        /// </summary>
        /// <param name="title">Title to show on the POI</param>
        /// <param name="location">Location of the POI in lat / long</param>
        /// <param name="imageUri">Uri of an image to use, image must exist in the Assets folder of the built solution (don't specify to use default image) 
        /// If your image was called test.png and was in the folder Assets/MapIcons/ you would specify MapIcons/test.png
        /// </param>
        public static void AddMapElement(string title, WSAGeoPoint location, string imageUri = null)
        {
#if ENABLE_WINMD_SUPPORT
            if (_mapControl != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    var mapIcon = new MapIcon()
                    {
                        Title = title,
                        Location = new Windows.Devices.Geolocation.Geopoint(new Windows.Devices.Geolocation.BasicGeoposition() { Latitude = location.Latitude, Longitude = location.Longitude }),
                        NormalizedAnchorPoint = new Point(0.5, 1.0),
                    };
                    if (imageUri != null)
                    {
                        mapIcon.Image = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/" + imageUri));
                    }
                    _mapControl.MapElements.Add(mapIcon);
                });
            }
#endif
        }

        /// <summary>
        /// Clear the map of all elements
        /// </summary>
        public static void ClearMap()
        {
#if ENABLE_WINMD_SUPPORT
            if (_mapControl != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    _mapControl.MapElements.Clear();
                });
            }
#endif
        }

        /// <summary>
        /// Destroy the map
        /// </summary>
        public static void DestroyMap()
        {
#if ENABLE_WINMD_SUPPORT
            if (WSANativeCore.IsDxSwapChainPanelConfigured() && _mapControl != null)
            {
                ThreadRunner.RunOnUIThread(() =>
                {
                    WSANativeCore.DxSwapChainPanel.Children.Remove(_mapControl);
                    _mapControl = null;
                });
            }
#endif
        }

        /// <summary>
        /// Launches the Maps app
        /// </summary>
        /// <param name="query">A query to control which part of the map is shown. Examples can be found in the online docs</param>
        public static void LaunchMapsApp(string query = "")
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                var launcherOptions = new Windows.System.LauncherOptions();
                launcherOptions.TargetApplicationPackageFamilyName = "Microsoft.WindowsMaps_8wekyb3d8bbwe";
                await Windows.System.Launcher.LaunchUriAsync(new Uri("bingmaps:?" + query), launcherOptions);
            });
#endif
        }
    }
}