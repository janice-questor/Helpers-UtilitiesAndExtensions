using System.IO;
using System.Security.Cryptography;

namespace Unimake.Cryptography
{
    public static class AES
    {
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

        #endregion Public Methods
    }
}