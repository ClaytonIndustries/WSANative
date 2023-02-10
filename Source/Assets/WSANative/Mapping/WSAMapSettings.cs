using CI.WSANative.Common;

namespace CI.WSANative.Mapping
{
    public class WSAMapSettings
    {
        /// <summary>
        /// Your bing maps auth token - register on the bing maps portal (can leave blank for testing)
        /// </summary>
        public string MapServiceToken { get; set; }
        /// <summary>
        /// Width of the map in pixels - ignored if HorizontalPlacement is set to Stretch
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the map in pixels - ignored if VerticalPlacement is set to Stretch
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// The outer margin around the map
        /// </summary>
        public WSAMargin Margin { get; set; }
        /// <summary>
        /// Center the map on this lat / long
        /// </summary>
        public WSAGeoPoint Centre { get; set; }
        /// <summary>
        /// How zoomed in should the map initially be (1 - 20)
        /// </summary>
        public int ZoomLevel { get; set; }
        /// <summary>
        /// How should the user be allowed to interact with the map
        /// </summary>
        public WSAMapInteractionMode InteractionMode { get; set; }
        /// <summary>
        /// Horizontal position of the map
        /// </summary>
        public WSAHorizontalPlacement HorizontalPlacement { get; set; }
        /// <summary>
        /// Vertical position of the map
        /// </summary>
        public WSAVerticalPlacement VerticalPlacement { get; set; }
    }
}