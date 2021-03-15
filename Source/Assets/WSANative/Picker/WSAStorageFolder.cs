////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;

#if ENABLE_WINMD_SUPPORT
using Windows.Storage;
#endif

// TODO Should these be made not async to reduce the amount of callbacks

namespace CI.WSANative.Pickers
{
    public class WSAStorageFolder
    {
        public DateTimeOffset DateCreated { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string DisplayType { get; set; }

#if ENABLE_WINMD_SUPPORT
        /// <summary>
        /// The original folder selected by the user - any calls to this must be wrapped in a NETFX_CORE block
        /// </summary>
        public StorageFolder OriginalFolder { get; set; }
#endif

        /// <summary>
        /// Gets a file with the specified name
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="result">A handle to the file</param>
        public void GetFile(string name, Action<WSAStorageFile> result)
        {
#if ENABLE_WINMD_SUPPORT
            if (result != null)
            {
                GetFileAsync(name, result);
            }
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private async void GetFileAsync(string name, Action<WSAStorageFile> result)
        {
            StorageFile file = await OriginalFolder.GetFileAsync(name);

            WSAStorageFile mappedFile = MapStorageFileToWSAStorageFile(file);

            result(mappedFile);
        }
#endif

        /// <summary>
        /// Gets a handle to each file in the current directory
        /// </summary>
        /// <param name="result">Collection of files</param>
        public void GetFiles(Action<IEnumerable<WSAStorageFile>> result)
        {
#if ENABLE_WINMD_SUPPORT
            if (result != null)
            {
                GetFilesAsync(result);
            }
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private async void GetFilesAsync(Action<IEnumerable<WSAStorageFile>> result)
        {
            IEnumerable<WSAStorageFile> files = (await OriginalFolder.GetFilesAsync()).Select(x => MapStorageFileToWSAStorageFile(x)).ToList();

            result(files);
        }
#endif

        /// <summary>
        /// Gets a handle to all the folders in the current directory
        /// </summary>
        /// <param name="result">Collection of folder names</param>
        public void GetFolders(Action<IEnumerable<WSAStorageFolder>> result)
        {
#if ENABLE_WINMD_SUPPORT
            if (result != null)
            {
                GetFoldersAsync(result);
            }
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private async void GetFoldersAsync(Action<IEnumerable<WSAStorageFolder>> result)
        {
            IEnumerable<WSAStorageFolder> folders = (await OriginalFolder.GetFoldersAsync()).Select(x => MapStorageFolderToWSAStorageFolder(x)).ToList();

            result(folders);
        }
#endif

        /// <summary>
        /// Determines whether a file exists
        /// </summary>
        /// <param name="name">Path to the file</param>
        /// <returns>True if the file exists, otherwise false</returns>
        public bool DoesFileExist(string name)
        {
#if ENABLE_WINMD_SUPPORT
            try
            {
                OriginalFolder.GetFileAsync(name).AsTask().Wait();

                return true;
            }
            catch
            {
            }
#endif
            return false;
        }

        /// <summary>
        /// Determines whether a folder exists
        /// </summary>
        /// <param name="name">Name of the folder</param>
        /// <returns>True if the folder exists, otherwise false</returns>
        public bool DoesFolderExist(string name)
        {
#if ENABLE_WINMD_SUPPORT
            try
            {
                OriginalFolder.GetFolderAsync(name).AsTask().Wait();

                return true;
            }
            catch
            {
            }
#endif
            return false;
        }

        /// <summary>
        /// Creates a file, if the file already exists it will be overwritten
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <returns>A handle to the created file</returns>
        public WSAStorageFile CreateFile(string name)
        {
#if ENABLE_WINMD_SUPPORT
            StorageFile file = OriginalFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting).AsTask().Result;

            WSAStorageFile mappedFile = MapStorageFileToWSAStorageFile(file);

            return mappedFile;
#else
            return new WSAStorageFile();
#endif
        }

        /// <summary>
        /// Creates a folder, if the folder already exists it will be overwitten
        /// </summary>
        /// <param name="name">Name of the folder</param>
        /// <returns>A handle to the created folder</returns>
        public WSAStorageFolder CreateFolder(string name)
        {
#if ENABLE_WINMD_SUPPORT
            StorageFolder createdFolder = OriginalFolder.CreateFolderAsync(name, CreationCollisionOption.ReplaceExisting).AsTask().Result;

            return MapStorageFolderToWSAStorageFolder(createdFolder);
#else
            return new WSAStorageFolder();
#endif
        }

        /// <summary>
        /// Attempts to delete a file with the specified name
        /// </summary>
        /// <param name="name">Name of the file to delete</param>
        public void DeleteFile(string name)
        {
#if ENABLE_WINMD_SUPPORT
            StorageFile file = OriginalFolder.GetFileAsync(name).AsTask().Result;

            file.DeleteAsync().AsTask().Wait();
#endif
        }

        /// <summary>
        /// Attempts to delete a folder with the specified name
        /// </summary>
        /// <param name="name">Name of the folder to delete</param>
        public void DeleteFolder(string name)
        {
#if ENABLE_WINMD_SUPPORT
            StorageFolder folderToDelete = OriginalFolder.GetFolderAsync(name).AsTask().Result;

            folderToDelete.DeleteAsync().AsTask().Wait();
#endif
        }

#if ENABLE_WINMD_SUPPORT
        private WSAStorageFile MapStorageFileToWSAStorageFile(StorageFile file)
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

        private WSAStorageFolder MapStorageFolderToWSAStorageFolder(StorageFolder folder)
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