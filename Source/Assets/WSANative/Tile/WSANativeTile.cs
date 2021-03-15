////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if ENABLE_WINMD_SUPPORT
using System.Linq;
using Windows.UI.StartScreen;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Tile
{
    public static class WSANativeTile
    {
        /// <summary>
        /// Displays the pin to start flyout which allows the user to decide if they want to create a seconday tile
        /// </summary>
        /// <param name="tileId">A unique id for this tile (no spaces)</param>
        /// <param name="displayName">Name to display on the tile</param>
        /// <param name="Square150x150Logo">Uri of the image file in the built uwp solution e.g ms-appx:///Assets/Square150x150Logo.png</param>
        /// <param name="ShowNameOnSquare150x150Logo">Should the display name be shown on this tile size</param>
        /// <param name="additionalTilesSizes">Additional sizes for the secondary tile - all are optional</param>
        public static void CreateSecondaryTile(string tileId, string displayName, Uri Square150x150Logo, bool ShowNameOnSquare150x150Logo, WSATileData additionalTilesSizes = null)
        {
#if ENABLE_WINMD_SUPPORT
            SecondaryTile secondaryTile = new SecondaryTile()
            {
                TileId = tileId,
                DisplayName = displayName,
                Arguments = string.Format("Activated by tile {0}", tileId)
            };

            secondaryTile.VisualElements.Square150x150Logo = Square150x150Logo;
            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = ShowNameOnSquare150x150Logo;

            if(additionalTilesSizes != null)
            {
                if (additionalTilesSizes.Square310x310Logo != null)
                {
                    secondaryTile.VisualElements.Square310x310Logo = additionalTilesSizes.Square310x310Logo;
                    secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = additionalTilesSizes.ShowNameOnSquare310x310Logo;
                }
#if UNITY_WSA_10_0
                if (additionalTilesSizes.Square44x44Logo != null)
                {
                    secondaryTile.VisualElements.Square44x44Logo = additionalTilesSizes.Square44x44Logo;
                }

                if (additionalTilesSizes.Square71x71Logo != null)
                {
                    secondaryTile.VisualElements.Square71x71Logo = additionalTilesSizes.Square71x71Logo;
                }
#endif
                if (additionalTilesSizes.Wide310x150Logo != null)
                {
                    secondaryTile.VisualElements.Wide310x150Logo = additionalTilesSizes.Wide310x150Logo;
                    secondaryTile.VisualElements.ShowNameOnWide310x150Logo = additionalTilesSizes.ShowNameOnWide310x150Logo;
                }
            }

            ThreadRunner.RunOnUIThread(async () =>
            {
                await secondaryTile.RequestCreateAsync();
            });
#endif
        }

        /// <summary>
        /// Deletes a secondary tile if it exists
        /// </summary>
        /// <param name="tileId">Id of the secondary tile to delete</param>
        public static void RemoveSecondaryTile(string tileId)
        {
#if ENABLE_WINMD_SUPPORT
            RemoveSecondaryTileAsync(tileId);
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static async void RemoveSecondaryTileAsync(string tileId)
        {
            if (SecondaryTile.Exists(tileId))
            {
                SecondaryTile secondaryTile = new SecondaryTile()
                {
                    TileId = tileId
                };

                await secondaryTile.RequestDeleteAsync();
            }
        }
#endif

        /// <summary>
        /// Find the tile ids of all secondary tiles associated with this app
        /// </summary>
        /// <returns>A collection of tile ids</returns>
        public static IEnumerable<string> FindAllSecondaryTiles()
        {
            IEnumerable<string> existingTiles = new List<string>();

#if ENABLE_WINMD_SUPPORT
            IReadOnlyList<SecondaryTile> tiles = SecondaryTile.FindAllAsync().AsTask().Result;

            if (tiles != null)
            {
                existingTiles = tiles.Select(x => x.TileId);
            }
#endif

            return existingTiles;
        }
    }
}