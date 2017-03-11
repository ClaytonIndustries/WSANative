////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if NETFX_CORE
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
#endif

using System.Collections.Generic;

namespace CI.WSANative.FileStorage
{
    public static class WSANativeStorage
    {
#if NETFX_CORE
        private static StorageFolder _rootfolder = ApplicationData.Current.LocalFolder;
#endif

        /// <summary>
        /// Save a file to your applications persistent data store - will automatically overwrite
        /// </summary>
        /// <param name="filename">Filename including extension</param>
        /// <param name="content">String content to write to the file</param>
        public static void SaveFile(string filename, string content)
        {
#if NETFX_CORE
            Task.WaitAll(SaveFileAsync(filename, content));
#endif
        }

#if NETFX_CORE
        private static async Task SaveFileAsync(string filename, string content)
        {
            StorageFile file = await _rootfolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, content);
        }
#endif

        /// <summary>
        /// Load a file from your applications persistent data store
        /// </summary>
        /// <param name="filename">Filename including extension</param>
        /// <returns>String content of the file</returns>
        public static string LoadFile(string filename)
        {
#if NETFX_CORE
            var task = LoadFileAsync(filename);

            Task.WaitAll(task);

            return task.Result;
#else
            return string.Empty;
#endif
        }

#if NETFX_CORE
        private static async Task<string> LoadFileAsync(string filename)
        {
            StorageFile file = await _rootfolder.GetFileAsync(filename);

            return await FileIO.ReadTextAsync(file);
        }
#endif

        /// <summary>
        /// Delete a file from your applications persistent data store
        /// </summary>
        /// <param name="filename">Filename including extension</param>
        public static void DeleteFile(string filename)
        {
#if NETFX_CORE
            Task.WaitAll(DeleteFileAsync(filename));
#endif
        }

#if NETFX_CORE
        private static async Task DeleteFileAsync(string filename)
        {
            StorageFile file = await _rootfolder.GetFileAsync(filename);

            await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }
#endif

        /// <summary>
        /// Check if a file exists in your applications persistent data store
        /// </summary>
        /// <param name="filename">Filename including extension</param>
        /// <returns>Boolean indicating whether the file exists</returns>
        public static bool DoesFileExist(string filename)
        {
#if NETFX_CORE
            var task = DoesFileExistAsync(filename);

            Task.WaitAll(task);

            return task.Result;
#else
            return false;
#endif
        }

#if NETFX_CORE
        private static async Task<bool> DoesFileExistAsync(string filename)
        {
            try
            {
                await _rootfolder.GetFileAsync(filename);

                return true;
            }
            catch
            {
                return false;
            }
        }
#endif

        /// <summary>
        /// Gets all files in your applications persistent data store
        /// </summary>
        /// <returns>The list of file names with extensions</returns>
        public static List<string> GetAllFiles()
        {
#if NETFX_CORE
            var task = GetAllFilesAsync();

            Task.WaitAll(task);

            return task.Result.Select(x => x.Name).ToList();
#else
            return new List<string>();
#endif
        }

#if NETFX_CORE
        private static async Task<IReadOnlyList<StorageFile>> GetAllFilesAsync()
        {
            return await _rootfolder.GetFilesAsync();
        }
#endif
    }
}