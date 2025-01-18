using Unimake.Cryptography;

namespace System
{
    public static class StringExtensions
    {
        #region Public Methods

        public static string Decrypt(this string input, string password) => AES.Decrypt(input, password);

        public static string Encrypt(this string input, string password) => AES.Encrypt(input, password);

        #endregion Public Methods
    }
}