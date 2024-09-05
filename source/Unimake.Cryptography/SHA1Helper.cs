using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Unimake.Cryptography
{
    public class SHA1Helper
    {
        #region Private Constructors

        /// <summary>
        /// Impede a criação da instância
        /// </summary>
        private SHA1Helper()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Criptografa uma string com RSA-SHA1 e retorna o conteúdo convertido para Base64String
        /// </summary>
        /// <param name="certificado">certificado utilizado na criptografia</param>
        /// <param name="value">Conteúdo a ser criptografado</param>
        /// <returns>Retorna a string assinada com RSA SHA1 e convertida para Base64String</returns>
        public static string ToRSASHA1(X509Certificate2 certificado, string value)
        {
            // Converter a cadeia de caracteres ASCII para bytes.
            var asciiEncoding = new ASCIIEncoding();
            var asciiBytes = asciiEncoding.GetBytes(value);

            // Gerar o HASH (array de bytes) utilizando SHA1
            using (var sha1 = SHA1.Create())
            {
                var sha1Hash = sha1.ComputeHash(asciiBytes);

                using (var rsa = certificado.GetRSAPrivateKey())
                {
                    if (rsa == null)
                    {
                        throw new InvalidOperationException("Chave privada RSA não encontrada no certificado.");
                    }

                    asciiBytes = rsa.SignHash(sha1Hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                }
            }

            return Convert.ToBase64String(asciiBytes);
        }

        /// <summary>
        /// Converte conteúdo para HSA1HashData
        /// </summary>
        /// <param name="data">Conteúdo a ser convertido</param>
        /// <returns>Conteúdo convertido para SH1HashData</returns>
        public static string ToSHA1HashData(string data) => ToSHA1HashData(data, false);

        /// <summary>
        /// Converte conteúdo para HSA1HashData
        /// </summary>
        /// <param name="data">Conteúdo a ser convertido</param>
        /// <param name="toUpper">Resultado todo em maiúsculo?</param>
        /// <returns>Conteúdo convertido para SH1HashData</returns>
        public static string ToSHA1HashData(string data, bool toUpper)
        {
            using (HashAlgorithm algorithm = new SHA1CryptoServiceProvider())
            {
                var buffer = algorithm.ComputeHash(Encoding.ASCII.GetBytes(data));
                var builder = new StringBuilder(buffer.Length);

                foreach (var num in buffer)
                {
                    if (toUpper)
                    {
                        builder.Append(num.ToString("X2"));
                        continue;
                    }

                    builder.Append(num.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        #endregion Public Methods
    }
}