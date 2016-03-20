////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

#if NETFX_CORE
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
#endif

namespace CI.WSANative.FilePickers
{
    public static class WSANativeFilePicker
    {
        /// <summary>
        /// Launches a picker which allows the user to choose a file to open
        /// </summary>
        /// <param name="commitButtonText">Text for the ok button</param>
        /// <param name="viewMode">Display files as either a list or thumbnails</param>
        /// <param name="suggestedStartLocation">Suggested initial directory</param>
        /// <param name="filters">File extension filters - specify an empty array to allow all</param>
        /// <param name="response">Contains the chosen file or null if nothing was selected</param>
        public static void PickSingleFile(string commitButtonText, WSAPickerViewMode viewMode, WSAPickerLocationId suggestedStartLocation, string[] filters, Action<WSAStorageFile> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.SuggestedStartLocation = MapWSAPickerLocationIdToPickerLocationId(suggestedStartLocation);
                openPicker.ViewMode = viewMode == WSAPickerViewMode.List ? PickerViewMode.List : PickerViewMode.Thumbnail;
                openPicker.CommitButtonText = commitButtonText;

                if (filters != null && filters.Length > 0)
                {
                    foreach (string filter in filters)
                    {
                        openPicker.FileTypeFilter.Add(filter);
                    }
                }
                else
                {
                    openPicker.FileTypeFilter.Add("*");
                }

                StorageFile file = await openPicker.PickSingleFileAsync();

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

        /// <summary>
        /// Launches a picker which enables a user to choose a file to save to
        /// </summary>
        /// <param name="commitButtonText">Text for the ok button</param>
        /// <param name="defaultFileExtension">Default extention for the file</param>
        /// <param name="suggestedFileName">Suggested file name for the file</param>
        /// <param name="suggestedStartLocation">Suggested initial directory</param>
        /// <param name="validFileTypes">List of valid file extensions</param>
        /// <param name="response">Contains the chosen file or null if nothing was selected</param>
        public static void PickSaveFile(string commitButtonText, string defaultFileExtension, string suggestedFileName, WSAPickerLocationId suggestedStartLocation, IList<KeyValuePair<string, IList<string>>> validFileTypes, Action<WSAStorageFile> response)
        {
#if NETFX_CORE
            UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.CommitButtonText = commitButtonText;
                savePicker.DefaultFileExtension = defaultFileExtension;
                savePicker.SuggestedFileName = suggestedFileName;
                savePicker.SuggestedStartLocation = MapWSAPickerLocationIdToPickerLocationId(suggestedStartLocation);

                if (validFileTypes != null && validFileTypes.Count > 0)
                {
                    foreach (KeyValuePair<string, IList<string>> fileType in validFileTypes)
                    {
                        savePicker.FileTypeChoices.Add(fileType);
                    }
                }

                StorageFile file = await savePicker.PickSaveFileAsync();

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
#if (UNITY_WSA_10_0 || UNITY_WSA_8_1)
                    IsAvailable = file.IsAvailable,
#endif
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