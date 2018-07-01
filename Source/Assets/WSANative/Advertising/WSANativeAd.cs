using System.Collections.Generic;

namespace CI.WSANative.Advertising
{
    public class WSANativeAd
    {
        public Dictionary<string, string> AdditionalAssets { get; set; }
        public string PrivacyUrl { get; set; }
        public string Rating { get; set; }
        public string Price { get; set; }
        public string SponsoredBy { get; set; }
        public string CallToActionText { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public WSANativeAdImage IconImage { get; set; }
        public List<WSANativeAdImage> MainImages { get; set; }
    }
}