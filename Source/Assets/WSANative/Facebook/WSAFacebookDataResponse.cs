////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using Newtonsoft.Json;
#endif

namespace CI.WSANative.Facebook.Core
{
    public class WSAFacebookDataResponse
    {
#if NETFX_CORE
        [JsonProperty("data")]
#endif
        public object Data { get; set; }
#if NETFX_CORE
        [JsonProperty("paging")]
#endif
        public object Paging { get; set; }
    }
}