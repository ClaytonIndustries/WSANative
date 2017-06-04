////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace CI.WSANative.Image
{
    public static class WSANativeImage
    {
        /// <summary>
        /// Create a Texture2D from image data
        /// </summary>
        /// <param name="image">The image bytes</param>
        /// <returns></returns>
        public static Texture2D LoadImage(byte[] image)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(image);

            return texture;
        }

        /// <summary>
        /// Crop an image
        /// </summary>
        /// <param name="image">Image to crop</param>
        /// <param name="x">X position to start cropping from</param>
        /// <param name="y">Y position to start cropping from</param>
        /// <param name="width">Width of the cropped image</param>
        /// <param name="height">Height of the cropped image</param>
        /// <returns></returns>
        public static Texture2D Crop(Texture2D image, int x, int y, int width, int height)
        {
            Color[] colors = image.GetPixels(x, y, width, height, 0);

            Texture2D croppedImage = new Texture2D(width, height, TextureFormat.RGBA32, false);
            croppedImage.SetPixels(colors, 0);

            return croppedImage;
        }
    }
}