////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if NETFX_CORE
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
#endif

namespace WSANativeFilePickers
{
    public static class WSANativeFilePicker
    {
        /// <summary>
        /// Launches a picker which allows the user to choose a file
        /// </summary>
        /// <param name="commitButtonText">Text for the ok button</param>
        /// <param name="viewMode">Display files as either a list or thumbnails</param>
        /// <param name="suggestedStartLocation">Suggested initial directory</param>
        /// <param name="filters">File extension filters - specify an empty array to allow all</param>
        /// <param name="response">Contains the chosen file or null if nothing was selected</param>
        public static void ChooseFile(string commitButtonText, WSAPickerViewMode viewMode, WSAPickerLocationId suggestedStartLocation, string[] filters, Action<WSAStorageFile> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileOpenPicker filePicker = new FileOpenPicker();
                filePicker.SuggestedStartLocation = MapWSAPickerLocationIdToPickerLocationId(suggestedStartLocation);
                filePicker.ViewMode = viewMode == WSAPickerViewMode.List ? PickerViewMode.List : PickerViewMode.Thumbnail;
                filePicker.CommitButtonText = commitButtonText;

                if (filters != null && filters.Length > 0)
                {
                    foreach (string filter in filters)
                    {
                        filePicker.FileTypeFilter.Add(filter);
                    }
                }
                else
                {
                    filePicker.FileTypeFilter.Add("*");
                }

                StorageFile file = await filePicker.PickSingleFileAsync();

                UnityEngine.WSA.Application.InvokeOnAppThread(() =>
                {
                    if(response != null)
                    {
                        response(MapStorageFileToWSAStorageFile(file));
                    }
                }, true);
            }, false);
#endif
        }

#if NETFX_CORE
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

        private static WSAStorageFile MapStorageFileToWSAStorageFile(StorageFile file)
        {
            if (file != null)
            {
                return new WSAStorageFile()
                {
                    DateCreated = file.DateCreated,
                    DisplayName = file.DisplayName,
                    FileType = file.FileType,
                    Name = file.Name,
                    Path = file.Path,
                    DisplayType = file.DisplayType,
                    IsAvailable = file.IsAvailable,
                    OriginalFile = file
                };
            }
            else
            {
                return null;
            }
        }
#endif
    }
}