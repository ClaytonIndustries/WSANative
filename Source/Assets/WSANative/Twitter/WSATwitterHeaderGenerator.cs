////////////////////////////////////////////////////////////////////////////////
//  
// @module WSA Native for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

#if ENABLE_WINMD_SUPPORT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace CI.WSANative.Twitter.Core
{
    public class WSATwitterHeaderGenerator
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;

        public WSATwitterHeaderGenerator(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        public string Generate(string method, string url, IDictionary<string, string> additionalParts, IDictionary<string, string> signatureOnlyParts, string oauthTokenSecret)
        {
            Dictionary<string, string> baseParts = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", _consumerKey },
                { "oauth_nonce", Guid.NewGuid().ToString().Replace("-", "") },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
                { "oauth_version", "1.0" },
            };

            ICollection<KeyValuePair<string, string>> combinedParts = baseParts.ToList().Concat(additionalParts.ToList()).ToList();

            combinedParts.Add(new KeyValuePair<string, string>("oauth_signature", GenerateSignature(method, url, combinedParts, signatureOnlyParts, oauthTokenSecret)));

            return "OAuth " + string.Join(", ", combinedParts.Select(x => string.Format("{0}=\"{1}\"", Uri.EscapeDataString(x.Key), Uri.EscapeDataString(x.Value))));
        }

        private string GenerateSignature(string method, string url, IEnumerable<KeyValuePair<string, string>> parts, IDictionary<string, string> signatureOnlyParts, string oauthTokenSecret)
        {
            if(signatureOnlyParts != null)
            {
                parts = parts.Concat(signatureOnlyParts.ToList());
            }

            parts = parts.OrderBy(x => x.Key);

            string signingKey = Uri.EscapeDataString(_consumerSecret) + "&" + Uri.EscapeDataString((!string.IsNullOrWhiteSpace(oauthTokenSecret) ? oauthTokenSecret : string.Empty));

            string signatureBase = string.Join("&", parts.Select(x => string.Format("{0}={1}", Uri.EscapeDataString(x.Key), Uri.EscapeDataString(x.Value))));

            signatureBase = Uri.EscapeDataString(signatureBase);

            signatureBase = method.ToUpper() + "&" + Uri.EscapeDataString(url) + "&" + signatureBase;

            return HashSignature(signatureBase, signingKey);
        }

        private string HashSignature(string signatureBase, string signingKey)
        {
            MacAlgorithmProvider macAlgorithmProvider = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);

            IBuffer signatureBaseBuffer = CryptographicBuffer.ConvertStringToBinary(signatureBase, BinaryStringEncoding.Utf8);

            IBuffer signingKeyBuffer = CryptographicBuffer.ConvertStringToBinary(signingKey, BinaryStringEncoding.Utf8);

            CryptographicKey cryptographicKey = macAlgorithmProvider.CreateKey(signingKeyBuffer);

            IBuffer encryptedsignatureBuffer = CryptographicEngine.Sign(cryptographicKey, signatureBaseBuffer);

            return Convert.ToBase64String(encryptedsignatureBuffer.ToArray());
        }
    }
}
#endif