using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Unimake.Cryptography.JWT
{
    /*
     * Eu não quis instanciar um monte de pacotes, então peguei este fragmento de código em:
     * Thanks to https://github.com/jwt-dotnet/jwt (｡◕‿◕｡)
     * Adaptei para HS512.
     * */

    /// <summary>
    /// Codifica e decodifica um token com o algorítimo HS512
    /// </summary>
    public abstract class JsonWebToken
    {
        #region Private Fields

        private static readonly Func<byte[], byte[], byte[]> hashAlgorithm;

        #endregion Private Fields

        #region Private Constructors

        private JsonWebToken()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch(output.Length % 4) // Pad with trailing '='s
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    output += "==";
                    break; // Two pad chars
                case 3:
                    output += "=";
                    break; // One pad char
                default:
                    throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        // from JWT spec
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        #endregion Private Methods

        #region Public Constructors

        static JsonWebToken()
        {
            hashAlgorithm = new Func<byte[], byte[], byte[]>((key, value) =>
            {
                using(var sha = new HMACSHA512(key))
                {
                    return sha.ComputeHash(value);
                }
            });
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Decodifica um token e retorna um <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="token">Token para decodificação</param>
        /// <param name="publicKey">chave pública</param>
        /// <returns></returns>
        /// <exception cref="CryptographicException">Se a assinatura for inválida</exception>
        public static Dictionary<string, object> Decode(string token, string publicKey, bool verify = true)
        {
            var parts = token.Split('.');
            var header = parts[0];
            var payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JObject.Parse(headerJson);
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JObject.Parse(payloadJson);

            var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
            var keyBytes = Encoding.UTF8.GetBytes(publicKey);
            var algorithm = (string)headerData["alg"];

            var signature = hashAlgorithm(keyBytes, bytesToSign);
            var decodedCrypto = Convert.ToBase64String(crypto);
            var decodedSignature = Convert.ToBase64String(signature);

            if(verify &&
               decodedCrypto != decodedSignature)
            {
                throw new CryptographicException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
            }

            return payloadData.ToObject<Dictionary<string, object>>();
        }

        /// <summary>
        /// Codifica um objeto e retorna
        /// </summary>
        /// <remarks>
        /// O <paramref name="payload"/> é convertido para json com <see cref="JsonConvert.SerializeObject(object?, Formatting)"/>
        /// </remarks>
        /// <param name="payload">Dados para codificação</param>
        /// <param name="publicKey">Chave pública</param>
        /// <returns></returns>
        public static string Encode(object payload, string publicKey)
        {
            return Encode(payload, Encoding.UTF8.GetBytes(publicKey));
        }

        /// <summary>
        /// Codifica um objeto e retorna
        /// </summary>
        /// <remarks>
        /// O <paramref name="payload"/> é convertido para json com <see cref="JsonConvert.SerializeObject(object?, Formatting)"/>
        /// </remarks>
        /// <param name="payload">Dados para codificação</param>
        /// <param name="keyBytes">Chave pública em bytes</param>
        /// <returns></returns>
        public static string Encode(object payload, byte[] keyBytes)
        {
            var segments = new List<string>();
            var header = new { alg = "HS512", typ = "JWT" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            var stringToSign = string.Join(".", segments.ToArray());

            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = hashAlgorithm(keyBytes, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        #endregion Public Methods
    }
}