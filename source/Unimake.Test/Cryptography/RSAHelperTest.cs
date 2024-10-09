using EBank.Solutions.Primitives.Security;
using System;
using System.Security.Cryptography;
using System.Text;
using Unimake.Cryptography;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Cryptography
{
    public class RSAHelperTest
    {
        #region Private Methods

        private static byte[] ConvertPemToBytes(string pem)
        {
            var pemHeader = "-----BEGIN PUBLIC KEY-----";
            var pemFooter = "-----END PUBLIC KEY-----";
            var base64 = pem.Replace(pemHeader, string.Empty)
                            .Replace(pemFooter, string.Empty)
                            .Trim();

            return Convert.FromBase64String(base64);
        }

        private static bool ValidatePublicKey(string publicKey)
        {
            try
            {
                var publicKeyBytes = ConvertPemToBytes(publicKey);
                using(RSA rsa = RSA.Create())
                {
                    rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
                    var originalMessage = Encoding.UTF8.GetBytes("Mensagem de teste");
                    var encryptedMessage = rsa.Encrypt(originalMessage, RSAEncryptionPadding.Pkcs1);
                    return encryptedMessage.Length > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion Private Methods

        #region Public Methods

        [Fact]
        public void CreatePublicKey()
        {
            var publicKey = RSAHelper.CreatePublicKey();
            Assert.NotNull(publicKey);
            Assert.NotEmpty(publicKey);
            Assert.True(ValidatePublicKey(publicKey));

            var signed = LinkSigner.SignLink("https://unimake.app", "issuer", "subject", null, publicKey);
            Assert.NotNull(signed);
            Assert.NotEmpty(signed);

            var values = LinkSigner.ValidateAndGetValues(signed, publicKey);
            
            Assert.True(values.Count > 0);
            Assert.NotNull(values);
            Assert.NotNull(values["iat"]);
            Assert.NotNull(values["iss"]);
            Assert.NotNull(values["sub"]);
            
            //invalidar a chave
            publicKey = RSAHelper.CreatePublicKey();
            publicKey = $"{publicKey[..48]}{publicKey[49..]}";
            Assert.False(ValidatePublicKey(publicKey));
        }

        #endregion Public Methods
    }
}