////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using CI.WSANative.Facebook.Models;

namespace CI.WSANative.Facebook
{
    public class WSAFacebookUser
    {
        public string UserId { get; set; }

        /// <summary>
        /// The users first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The users full name - this will probably only contain their first name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The users email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The users profile picture
        /// </summary>
        public WSAFacebookPicture Picture { get; set; }

        public static WSAFacebookUser FromDto(WSAFacebookUserDto dto)
        {
            return new WSAFacebookUser()
            {
                Email = dto.email,
                FirstName = dto.first_name,
                UserId = dto.id,
                Name = dto.name,
                Picture = WSAFacebookPicture.FromDto(dto.picture),
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