////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
using Windows.Storage;
using Windows.Storage.Streams;
#endif

namespace CI.WSANative.FileStorage
{
    public class WSAStorageFile
    {
        public DateTimeOffset DateCreated { get; set; }
        public string DisplayName { get; set; }
        public string FileType { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string DisplayType { get; set; }

        /// <summary>
        /// Only available in universal 10 or desktop 8.1
        /// </summary>
        public bool IsAvailable { get; set; }

#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
        /// <summary>
        /// The original file selected by the user - any calls to this must be wrapped in a NETFX_CORE block
        /// </summary>
        public StorageFile OriginalFile { get; set; }
#endif

        /// <summary>
        /// Gets the contents of the file as a byte array - returns an empty array in the editor
        /// </summary>
        public byte[] ReadBytes()
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
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
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            FileIO.WriteBytesAsync(OriginalFile, bytes).AsTask().Wait();
#endif
        }

        /// <summary>
        /// Writes a string to the file - does nothing in the editor
        /// </summary>
        /// <param name="text">The string to write</param>
        public void WriteText(string text)
        {
#if NETFX_CORE || (ENABLE_IL2CPP && UNITY_WSA_10_0)
            FileIO.WriteTextAsync(OriginalFile, text).AsTask().Wait();
#endif
        }
    }
}