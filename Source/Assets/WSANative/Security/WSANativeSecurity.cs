////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
#endif

namespace CI.WSANative.Security
{
    public static class WSANativeSecurity
    {
        /// <summary>
        /// Encrypts the specified data using the advanced encryption standard (AES) algorithm
        /// </summary>
        /// <param name="key">The key which should be a string containing 32 random ascii characters (i.e 32 bytes)</param>
        /// <param name="iv">The initialistion vector which should be a string containing 16 random ascii characters (i.e 16 bytes)</param>
        /// <param name="data">The data to encrypt</param>
        /// <returns>The encrypted data</returns>
        public static string SymmetricEncrypt(string key, string iv, string data)
        {
#if ENABLE_WINMD_SUPPORT
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8);

            SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

            CryptographicKey aesKey = aes.CreateSymmetricKey(CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8));
            IBuffer aesIV = CryptographicBuffer.ConvertStringToBinary(iv, BinaryStringEncoding.Utf8);

            return CryptographicBuffer.EncodeToBase64String(CryptographicEngine.Encrypt(aesKey, buffMsg, aesIV));
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Decrypts data that was encrypted using the advanced encryption standard (AES) algorithm
        /// </summary>
        /// <param name="key">The key used to encrypt the data</param>
        /// <param name="iv">The initialisation vector used to encrypt the data</param>
        /// <param name="data">The encrypted data</param>
        /// <returns>The decrypted data</returns>
        public static string SymmetricDecrypt(string key, string iv, string data)
        {
#if ENABLE_WINMD_SUPPORT
            SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);

            CryptographicKey aesKey = aes.CreateSymmetricKey(CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8));
            IBuffer aesIV = CryptographicBuffer.ConvertStringToBinary(iv, BinaryStringEncoding.Utf8);

            IBuffer buffDecrypted = CryptographicEngine.Decrypt(aesKey, CryptographicBuffer.DecodeFromBase64String(data), aesIV);

            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Encode a string using base64
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncodeBase64(string data)
        {
#if ENABLE_WINMD_SUPPORT
            return CryptographicBuffer.EncodeToBase64String(CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8));
#else
            return string.Empty;
#endif
        }

        /// <summary>
        /// Decode a string that has been base64 encoded
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DecodeBase64(string data)
        {
#if ENABLE_WINMD_SUPPORT
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, CryptographicBuffer.DecodeFromBase64String(data));
#else
            return string.Empty;
#endif
        }
    }
}