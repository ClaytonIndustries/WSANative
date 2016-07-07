////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if NETFX_CORE
using Newtonsoft.Json;
#endif

namespace CI.WSANative.Facebook
{
    public class WSAFacebookUser
    {
        /// <summary>
        /// Requires no permissions
        /// </summary>
#if NETFX_CORE
        [JsonProperty("id")]
#endif
        public string Id { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("age_range")]
#endif
        public WSAFacebookAgeRange AgeRange { get; set; }
        /// <summary>
        /// Requires no permissions
        /// </summary>
#if NETFX_CORE
        [JsonProperty("name")]
#endif
        public string Name { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("first_name")]
#endif
        public string FirstName { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("last_name")]
#endif
        public string LastName { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("link")]
#endif
        public string Link { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("gender")]
#endif
        public string Gender { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("locale")]
#endif
        public string Locale { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("picture")]
#endif
        public WSAFacebookPicture Picture { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
#if NETFX_CORE
        [JsonProperty("timezone")]
#endif
        public int TimeZone { get; set; }
        /// <summary>
        /// Requires email
        /// </summary>
#if NETFX_CORE
        [JsonProperty("email")]
#endif
        public string Email { get; set; }
        /// <summary>
        /// Requires user_birthday
        /// </summary>
#if NETFX_CORE
        [JsonProperty("birthday")]
#endif
        public DateTime? Birthday { get; set; }
    }

    public class WSAFacebookAgeRange
    {
#if NETFX_CORE
        [JsonProperty("min")]
#endif
        public int Min { get; set; }
#if NETFX_CORE
        [JsonProperty("Max")]
#endif
        public int Max { get; set; }
    }

    public class WSAFacebookPicture
    {
#if NETFX_CORE
        [JsonProperty("data")]
#endif
        public WSAFacebookPictureData Data { get; set; }
    }

    
    public class WSAFacebookPictureData
    {
#if NETFX_CORE
        [JsonProperty("url")]
#endif
        public string Url { get; set; }
#if NETFX_CORE
        [JsonProperty("is_silhouette")]
#endif
        public bool IsSilhouette { get; set; }
    }
}