////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Mapping
{
    public class WSAMapSettings
    {
        public string MapServiceToken
        {
            get; set;
        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }

        public WSAPosition Position
        {
            get; set;
        }

        public WSAGeoPoint Center
        {
            get; set;
        }

        public int ZoomLevel
        {
            get; set;
        }

        public WSAMapInteractionMode InteractionMode
        {
            get; set;
        }
    }
}