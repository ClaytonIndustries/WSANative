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
using Windows.Storage.Streams;
#endif

namespace CI.WSANative.FilePickers
{
    public class WSAStorageFile
    {
        public DateTimeOffset DateCreated;
        public string DisplayName;
        public string FileType;
        public string Name;
        public string Path;
        public string DisplayType;

        /// <summary>
        /// Only available in universal 10 or desktop 8.1
        /// </summary>
        public bool IsAvailable;

#if NETFX_CORE
        /// <summary>
        /// The original file selected by the user - any calls to this must be wrapped in a NETFX_CORE block
        /// </summary>
        public StorageFile OriginalFile;
#endif

        /// <summary>
        /// Gets the contents of the file as a byte array - returns an empty array in the editor
        /// </summary>
        public byte[] ReadBytes()
        {
#if NETFX_CORE
            IBuffer buffer = FileIO.ReadBufferAsync(OriginalFile).AsTask().Result;
            DataReader dataReader = DataReader.FromBuffer(buffer);
            byte[] bytes = new byte[buffer.Length];
            dataReader.ReadBytes(bytes);
            return bytes;
#else
            return new byte[0];
#endif
        }

        /// <summary>
        /// Gets the contents of the file as a string - returns an empty string in the editor
        /// </summary>
        public string ReadText()
        {
#if NETFX_CORE
            return FileIO.ReadTextAsync(OriginalFile).AsTask().Result;
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Writes a series of bytes to the file - does nothing in the editor
        /// </summary>
        /// <param name="bytes">The bytes to write</param>
        public void WriteBytes(byte[] bytes)
        {
#if NETFX_CORE
            FileIO.WriteBytesAsync(OriginalFile, bytes).AsTask().Wait();
#endif
        }

        /// <summary>
        /// Writes a string to the file - does nothing in the editor
        /// </summary>
        /// <param name="text">The string to write</param>
        public void WriteText(string text)
        {
#if NETFX_CORE
            FileIO.WriteTextAsync(OriginalFile, text).AsTask().Wait();
#endif
        }
    }
}