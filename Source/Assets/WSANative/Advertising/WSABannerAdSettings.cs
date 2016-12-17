////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Advertising
{
    public class WSABannerAdSettings
    {
        public WSABannerAdType AdType
        {
            get; set;
        }

        public string AppId
        {
            get; set;
        }

        public string AdUnitId
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

        public WSAAdVerticalPlacement VerticalPlacement
        {
            get; set;
        }

        public WSAAdHorizontalPlacement HorizontalPlacement
        {
            get; set;
        }
    }
}