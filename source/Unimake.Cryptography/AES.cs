using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Unimake.Cryptography
{
    public static class AES
    {
        #region Private Methods

        private static byte[] GenerateSalt()
        {
            var salt = new byte[16];

            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        #endregion Private Methods

        #region Public Methods

        public static string Decrypt(int aesKeySizeInBits, string pw, byte[] salt, byte[] db, int rfc2898KeygenIterations)
        {
            byte[] plainText = null;

            using(Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = aesKeySizeInBits;
                var KeyStrengthInBytes = aes.KeySize / 8;
                var rfc2898 = new Rfc2898DeriveBytes(pw, salt, rfc2898KeygenIterations);
                aes.Key = rfc2898.GetBytes(KeyStrengthInBytes);
                aes.IV = rfc2898.GetBytes(KeyStrengthInBytes);

                using(var ms = new MemoryStream())
                {
                    using(var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(db, 0, db.Length);
                    }
                    plainText = ms.ToArray();
                }
            }

            return System.Text.Encoding.Unicode.GetString(plainText);
        }

        /// <summary>
        /// Descriptografa uma string criptografada usando a senha especificada.
        /// </summary>
        /// <param name="cipherText">A string criptografada em formato Base64.</param>
        /// <param name="password">A senha usada para derivar a chave de descriptografia.</param>
        /// <returns>O texto plano descriptografado.</returns>
        public static string Decrypt(string cipherText, string password)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);

            // Extrai o salt do início dos dados criptografados
            var salt = new byte[16];
            Array.Copy(cipherBytes, 0, salt, 0, salt.Length);

            // Deriva a chave e o IV usando PBKDF2
            using(var keyDerivation = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                var key = keyDerivation.GetBytes(32); // Chave de 256 bits
                var iv = keyDerivation.GetBytes(16);  // IV de 128 bits

                using(var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;

                    using(var memoryStream = new MemoryStream())
                    {
                        using(var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(cipherBytes, salt.Length, cipherBytes.Length - salt.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Criptografa uma string usando a senha especificada.
        /// </summary>
        /// <param name="plainText">A string a ser criptografada.</param>
        /// <param name="password">A senha usada para derivar a chave de criptografia.</param>
        /// <returns>Uma string criptografada codificada em formato Base64.</returns>
        public static string Encrypt(string plainText, string password)
        {
            // Gera um salt aleatório
            var salt = GenerateSalt();

            // Deriva a chave e o IV usando PBKDF2
            using(var keyDerivation = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                var key = keyDerivation.GetBytes(32); // Chave de 256 bits
                var iv = keyDerivation.GetBytes(16);  // IV de 128 bits

                using(var aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;

                    using(var memoryStream = new MemoryStream())
                    {
                        // Escreve o salt no início do fluxo de memória
                        memoryStream.Write(salt, 0, salt.Length);

                        using(var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            var plainBytes = Encoding.UTF8.GetBytes(plainText);
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        #endregion Public Methods
    }
}