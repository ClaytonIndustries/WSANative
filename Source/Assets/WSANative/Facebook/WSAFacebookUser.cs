////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using CI.WSANative.Facebook.Models;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookUser
    {
        /// <summary>
        /// Requires no permissions
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public WSAFacebookAgeRange AgeRange { get; set; }
        /// <summary>
        /// Requires no permissions
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public string Locale { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public WSAFacebookPicture Picture { get; set; }
        /// <summary>
        /// Requires public_profile
        /// </summary>
        public int TimeZone { get; set; }
        /// <summary>
        /// Requires email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Requires user_birthday
        /// </summary>
        public string Birthday { get; set; }

        public static WSAFacebookUser FromDto(WSAFacebookUserDto dto)
        {
            return new WSAFacebookUser()
            {
                AgeRange = WSAFacebookAgeRange.FromDto(dto.age_range),
                Birthday = dto.birthday,
                Email = dto.email,
                FirstName = dto.first_name,
                Gender = dto.gender,
                Id = dto.id,
                LastName = dto.last_name,
                Link = dto.link,
                Locale = dto.locale,
                Name = dto.name,
                Picture = WSAFacebookPicture.FromDto(dto.picture),
                TimeZone = dto.timezone
            };
        }
    }

    public class WSAFacebookAgeRange
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public static WSAFacebookAgeRange FromDto(WSAFacebookAgeRangeDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new WSAFacebookAgeRange()
            {
                Max = dto.Max,
                Min = dto.min
            };
        }
    }

    public class WSAFacebookPicture
    {
        public WSAFacebookPictureData Data { get; set; }

        public static WSAFacebookPicture FromDto(WSAFacebookPictureDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new WSAFacebookPicture()
            {
                Data = WSAFacebookPictureData.FromDto(dto.data)
            };
        }
    }
    
    public class WSAFacebookPictureData
    {
        public string Url { get; set; }
        public bool IsSilhouette { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public static WSAFacebookPictureData FromDto(WSAFacebookPictureDataDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            return new WSAFacebookPictureData()
            {
                IsSilhouette = dto.is_silhouette,
                Url = dto.url,
                Width = dto.width,
                Height = dto.height
            };
        }
    }
}