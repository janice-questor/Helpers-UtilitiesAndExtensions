using System;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Cryptography
{
    public class AESTest
    {
        #region Public Methods

        [Fact]
        public void EncryptAndDecrypt()
        {
            var text = "Hello World!";
            var key = "1234567890123456";
            var encrypted = text.Encrypt(key);
            var decrypted = encrypted.Decrypt(key);

            Assert.True(encrypted.IsBase64());
            Assert.Equal(text, decrypted);
            Assert.NotEqual(decrypted, encrypted);
            Assert.NotEqual(text, encrypted);
        }

        #endregion Public Methods
    }
}