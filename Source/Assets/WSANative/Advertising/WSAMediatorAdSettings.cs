////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

namespace CI.WSANative.Advertising
{
    public class WSAMediatorAdSettings
    {
        public string WAppId
        {
            get; set;
        }

        public string WAdUnitId
        {
            get; set;
        }

        public string AdDuplexAppKey
        {
            get; set;
        }

        public string AdDuplexAdUnitId
        {
            get; set;
        }

        public int AdDuplexWeight
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