////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if ENABLE_WINMD_SUPPORT
using Windows.Storage;
using Windows.Storage.Pickers;
using CI.WSANative.Common;
#endif

namespace CI.WSANative.Pickers
{
    public class WSANativeFolderPicker
    {
        /// <summary>
        /// Launches a picker which allows the user to choose a folder
        /// </summary>
        /// <param name="commitButtonText">Text for the ok button</param>
        /// <param name="viewMode">Display folders as either a list or thumbnails</param>
        /// <param name="suggestedStartLocation">Suggested initial directory</param>
        /// <param name="filters">File extension filters - specify an empty array to allow all</param>
        /// <param name="response">Contains the chosen folder or null if nothing was selected</param>
        public static void PickSingleFolder(string commitButtonText, WSAPickerViewMode viewMode, WSAPickerLocationId suggestedStartLocation, string[] filters, Action<WSAStorageFolder> response)
        {
#if ENABLE_WINMD_SUPPORT
            ThreadRunner.RunOnUIThread(async () =>
            {
                FolderPicker folderPicker = new FolderPicker();
                folderPicker.SuggestedStartLocation = MapWSAPickerLocationIdToPickerLocationId(suggestedStartLocation);
                folderPicker.ViewMode = viewMode == WSAPickerViewMode.List ? PickerViewMode.List : PickerViewMode.Thumbnail;
                folderPicker.CommitButtonText = commitButtonText;

                if (filters != null && filters.Length > 0)
                {
                    foreach (string filter in filters)
                    {
                        folderPicker.FileTypeFilter.Add(filter);
                    }
                }
                else
                {
                    folderPicker.FileTypeFilter.Add("*");
                }

                StorageFolder folder = await folderPicker.PickSingleFolderAsync();

                ThreadRunner.RunOnAppThread(() =>
                {
                    if (response != null)
                    {
                        response(folder != null ? MapStorageFolderToWSAStorageFolder(folder) : null);
                    }
                }, true);
            });
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private static PickerLocationId MapWSAPickerLocationIdToPickerLocationId(WSAPickerLocationId pickerLocationId)
        {
            switch (pickerLocationId)
            {
                case WSAPickerLocationId.ComputerFolder:
                    return PickerLocationId.ComputerFolder;
                case WSAPickerLocationId.Desktop:
                    return PickerLocationId.Desktop;
                case WSAPickerLocationId.DocumentsLibrary:
                    return PickerLocationId.DocumentsLibrary;
                case WSAPickerLocationId.Downloads:
                    return PickerLocationId.Downloads;
                case WSAPickerLocationId.HomeGroup:
                    return PickerLocationId.HomeGroup;
                case WSAPickerLocationId.MusicLibrary:
                    return PickerLocationId.MusicLibrary;
                case WSAPickerLocationId.PicturesLibrary:
                    return PickerLocationId.PicturesLibrary;
                case WSAPickerLocationId.VideosLibrary:
                    return PickerLocationId.VideosLibrary;
                default:
                    return PickerLocationId.PicturesLibrary;
            }
        }

        private static WSAStorageFolder MapStorageFolderToWSAStorageFolder(StorageFolder folder)
        {
            return new WSAStorageFolder()
            {
                DateCreated = folder.DateCreated,
                DisplayName = folder.DisplayName,
                Name = folder.Name,
                Path = folder.Path,
                DisplayType = folder.DisplayType,
                OriginalFolder = folder   
            };
        }
#endif
    }
}