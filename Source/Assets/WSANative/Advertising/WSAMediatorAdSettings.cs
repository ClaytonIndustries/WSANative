using CI.WSANative.Advertising;

namespace Assets.WSANative.Advertising
{
    public class WSAMediatorAdSettings
    {
        public string AppId
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

        public WSAAdPlacement Placement
        {
            get; set;
        }
    }
}