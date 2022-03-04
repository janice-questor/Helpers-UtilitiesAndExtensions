using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Unimake.Cryptography
{
    public static class Cipher
    {
        #region Private Methods

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            using(var ms = new MemoryStream())
            {
                using(var AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, Rijndael.Key, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using(var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            using(var ms = new MemoryStream())
            {
                using(var AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, Rijndael.Key, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using(var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        #endregion Private Methods

        #region Public Methods

        public static string Decrypt(string encryptedText, string password)
        {
            if(string.IsNullOrEmpty(encryptedText))
            {
                return "";
            }

            if(password == null)
            {
                password = string.Empty;
            }

            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        public static string Encrypt(string plainText, string password)
        {
            if(string.IsNullOrEmpty(plainText))
            {
                return "";
            }

            if(password == null)
            {
                password = string.Empty;
            }

            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        #endregion Public Methods
    }
}