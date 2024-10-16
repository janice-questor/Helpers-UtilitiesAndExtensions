using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Unimake.Cryptography
{
    public sealed class RSAHelper
    {
        #region Private Constructors

        private RSAHelper()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for(var i = 0; i < value.Length; i++)
            {
                if(value[i] != 0)
                    break;
                prefixZeros++;
            }
            if(value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if(forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for(var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if(length < 0)
                throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if(length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while(temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for(var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }

        private static void ExportPublicKey(RSA csp, TextWriter outputStream)
        {
            var parameters = csp.ExportParameters(false);
            using(var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using(var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    innerWriter.Write((byte)0x30); // SEQUENCE
                    EncodeLength(innerWriter, 13);
                    innerWriter.Write((byte)0x06); // OBJECT IDENTIFIER
                    var rsaEncryptionOid = new byte[] { 0x2a, 0x86, 0x48, 0x86, 0xf7, 0x0d, 0x01, 0x01, 0x01 };
                    EncodeLength(innerWriter, rsaEncryptionOid.Length);
                    innerWriter.Write(rsaEncryptionOid);
                    innerWriter.Write((byte)0x05); // NULL
                    EncodeLength(innerWriter, 0);
                    innerWriter.Write((byte)0x03); // BIT STRING
                    using(var bitStringStream = new MemoryStream())
                    {
                        var bitStringWriter = new BinaryWriter(bitStringStream);
                        bitStringWriter.Write((byte)0x00); // # of unused bits
                        bitStringWriter.Write((byte)0x30); // SEQUENCE
                        using(var paramsStream = new MemoryStream())
                        {
                            var paramsWriter = new BinaryWriter(paramsStream);
                            EncodeIntegerBigEndian(paramsWriter, parameters.Modulus); // Modulus
                            EncodeIntegerBigEndian(paramsWriter, parameters.Exponent); // Exponent
                            var paramsLength = (int)paramsStream.Length;
                            EncodeLength(bitStringWriter, paramsLength);
                            bitStringWriter.Write(paramsStream.GetBuffer(), 0, paramsLength);
                        }
                        var bitStringLength = (int)bitStringStream.Length;
                        EncodeLength(innerWriter, bitStringLength);
                        innerWriter.Write(bitStringStream.GetBuffer(), 0, bitStringLength);
                    }
                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                outputStream.WriteLine("-----BEGIN PUBLIC KEY-----");
                for(var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                }
                outputStream.WriteLine("-----END PUBLIC KEY-----");
            }
        }

        #endregion Private Methods

        #region Public Methods

        public static byte[] ConvertPemToBytes(string pem)
        {
            var pemHeader = "-----BEGIN PUBLIC KEY-----";
            var pemFooter = "-----END PUBLIC KEY-----";
            var base64 = pem.Replace(pemHeader, string.Empty)
                            .Replace(pemFooter, string.Empty)
                            .Trim();

            return Convert.FromBase64String(base64);
        }

        public static string CreatePublicKey()
        {
            using(var csp = RSACryptoServiceProvider.Create())
            {
                using(var outputStream = new StringWriter())
                {
                    ExportPublicKey(csp, outputStream);
                    return outputStream.ToString();
                }
            }
        }

        /// <summary>
        /// Valida a chave pública e retorna verdadeiro ou falso.
        /// <para>Retorna o erro de validação, caso <paramref name="throwValidationError"/> for verdadeiro</para>
        /// </summary>
        /// <param name="publicKey">Chave pública</param>
        /// <param name="throwValidationError">Se verdadeiro, é lançado o erro ao tentar validar a chave</param>
        /// <param name="returnTrueIsNullOrWhiteSpace">Se verdadeiro e a chave <paramref name="publicKey"/> estiver vazia ou apenas espaços, retorna verdadeiro.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se <paramref name="returnTrueIsNullOrWhiteSpace"/> for falso e <paramref name="throwValidationError"/> for verdadeiro.</exception>
        public static bool ValidatePublicKey(string publicKey, bool returnTrueIsNullOrWhiteSpace = false, bool throwValidationError = false)
        {
            if(string.IsNullOrWhiteSpace(publicKey))
            {
                if(returnTrueIsNullOrWhiteSpace)
                {
                    return true;
                }

                if(throwValidationError)
                {
                    throw new ArgumentException($"'{nameof(publicKey)}' cannot be null or whitespace.", nameof(publicKey));
                }

                return false;
            }

            try
            {
                var publicKeyBytes = ConvertPemToBytes(publicKey);
                using(RSA rsa = RSA.Create())
                {
                    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes);
                    var originalMessage = Encoding.UTF8.GetBytes("Mensagem de teste");
                    var encryptedMessage = rsa.Encrypt(originalMessage, RSAEncryptionPadding.Pkcs1);
                    return encryptedMessage.Length > 0;
                }
            }
            catch
            {
                if(throwValidationError)
                {
                    throw;
                }

                return false;
            }
        }

        #endregion Public Methods
    }
}