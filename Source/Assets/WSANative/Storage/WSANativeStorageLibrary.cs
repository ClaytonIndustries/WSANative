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
using System.Linq;
using Windows.Storage;
#endif

namespace CI.WSANative.FileStorage
{
    public class WSANativeStorageLibrary
    {
        /// <summary>
        /// Gets a handle to a single file at the specified path
        /// </summary>
        /// <param name="library">The library to start in - make sure you have the relevant permission for it</param>
        /// <param name="relativePath">Path to the file</param>
        /// <param name="result">The specified file</param>
        public static void GetFile(WSAStorageLibrary library, string relativePath, Action<WSAStorageFile> result)
        {
#if NETFX_CORE
            if (result != null)
            {
                StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

                GetFileAsync(folder, relativePath, result);
            }
#endif
        }

#if NETFX_CORE
        private static async void GetFileAsync(StorageFolder knownFolder, string path, Action<WSAStorageFile> result)
        {
            StorageFile file = await knownFolder.GetFileAsync(path);

            WSAStorageFile mappedFile = MapStorageFileToWSAStorageFile(file);

            result(mappedFile);
        }
#endif

        /// <summary>
        /// Determines whether a file exists
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the file</param>
        /// <returns>True if the file exists, otherwise false</returns>
        public static bool DoesFileExist(WSAStorageLibrary library, string relativePath)
        {
#if NETFX_CORE
            StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

            try
            {
                folder.GetFileAsync(relativePath).AsTask().Wait();

                return true;
            }
            catch
            {
            }
#endif
            return false;
        }

        /// <summary>
        /// Gets a handle to each file at the specified path
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory to look in - empty string for the root</param>
        /// <param name="result">Collection of files at the specified path</param>
        public static void GetFiles(WSAStorageLibrary library, string relativePath, Action<IEnumerable<WSAStorageFile>> result)
        {
#if NETFX_CORE
            if (result != null)
            {
                StorageFolder knownFolder = GetStorageFolderForWSAKnownLibrary(library);

                GetFilesAsync(knownFolder, relativePath, result);
            }
#endif
        }

#if NETFX_CORE
        private static async void GetFilesAsync(StorageFolder knownFolder, string path, Action<IEnumerable<WSAStorageFile>> result)
        {
            StorageFolder folder = string.IsNullOrWhiteSpace(path) ? knownFolder : await knownFolder.GetFolderAsync(path);

            IEnumerable<WSAStorageFile> files = (await folder.GetFilesAsync())
                                                             .Select(x => MapStorageFileToWSAStorageFile(x)).ToList();

            result(files);
        }
#endif

        /// <summary>
        /// Gets the names of all the directories at the specified path
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory to look in - empty string for the root</param>
        /// <param name="result">Collection of files at the specified path</param>
        public static void GetDirectories(WSAStorageLibrary library, string relativePath, Action<IEnumerable<string>> result)
        {
#if NETFX_CORE
            if (result != null)
            {
                StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

                GetDirectoriesAsync(folder, relativePath, result);
            }
#endif
        }

#if NETFX_CORE
        private static async void GetDirectoriesAsync(StorageFolder knownFolder, string path, Action<IEnumerable<string>> result)
        {
            StorageFolder folder = string.IsNullOrWhiteSpace(path) ? knownFolder : await knownFolder.GetFolderAsync(path);

            IEnumerable<string> directories = (await folder.GetFoldersAsync())
                                                             .Select(x => x.Name).ToList();

            result(directories);
        }
#endif

        /// <summary>
        /// Determines whether a directory exists
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory</param>
        /// <returns>True if the directory exists, otherwise false</returns>
        public static bool DoesDirectoryExist(WSAStorageLibrary library, string relativePath)
        {
#if NETFX_CORE
            StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

            try
            {
                folder.GetFolderAsync(relativePath).AsTask().Wait();

                return true;
            }
            catch
            {
            }
#endif
            return false;
        }

        /// <summary>
        /// Creates a file at the specified path and returns a handle to it. If the file already exists it will be overwritten
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory in which to create the file</param>
        /// <returns>A handle to the created file</returns>
        public static WSAStorageFile CreateFile(WSAStorageLibrary library, string relativePath)
        {
#if NETFX_CORE
            StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

            StorageFile file = folder.CreateFileAsync(relativePath, CreationCollisionOption.ReplaceExisting).AsTask().Result;

            WSAStorageFile mappedFile = MapStorageFileToWSAStorageFile(file);

            return mappedFile;
#else
            return new WSAStorageFile();
#endif
        }

        /// <summary>
        /// Creates a directory at the specified path, if the directory already exsists it will be overwitten
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory in which to create the new directory</param>
        public static void CreateDirectory(WSAStorageLibrary library, string relativePath)
        {
#if NETFX_CORE
            StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

            StorageFolder createdFolder = folder.CreateFolderAsync(relativePath, CreationCollisionOption.ReplaceExisting).AsTask().Result;
#endif
        }

        /// <summary>
        /// Attempts to delete the file at the specified path
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory in which the file exists</param>
        public static void DeleteFile(WSAStorageLibrary library, string relativePath)
        {
#if NETFX_CORE
            StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

            StorageFile file = folder.GetFileAsync(relativePath).AsTask().Result;

            file.DeleteAsync().AsTask().Wait();
#endif
        }

        /// <summary>
        /// Attempts to delete the directory at the specified path
        /// </summary>
        /// <param name="library">The library to start in - you must request permissions for it in the app manifest</param>
        /// <param name="relativePath">Path to the directory which should be deleted</param>
        public static void DeleteDirectory(WSAStorageLibrary library, string relativePath)
        {
#if NETFX_CORE
            StorageFolder folder = GetStorageFolderForWSAKnownLibrary(library);

            StorageFolder folderToDelete = folder.GetFolderAsync(relativePath).AsTask().Result;

            folderToDelete.DeleteAsync().AsTask().Wait();
#endif
        }

#if NETFX_CORE
        private static StorageFolder GetStorageFolderForWSAKnownLibrary(WSAStorageLibrary wsaStorageLibrary)
        {
            switch(wsaStorageLibrary)
            {
                case WSAStorageLibrary.Local:
                    return ApplicationData.Current.LocalFolder;
                case WSAStorageLibrary.Music:
                    return KnownFolders.MusicLibrary;
                case WSAStorageLibrary.Pictures:
                    return KnownFolders.PicturesLibrary;
                default:
                    return KnownFolders.VideosLibrary;
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