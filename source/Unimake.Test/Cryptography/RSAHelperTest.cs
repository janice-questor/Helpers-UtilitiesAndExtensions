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


        #endregion Private Methods

        #region Public Methods

        [Fact]
        public void CreatePublicKey()
        {
            var publicKey = RSAHelper.CreatePublicKey();
            Assert.NotNull(publicKey);
            Assert.NotEmpty(publicKey);
            Assert.True(RSAHelper.ValidatePublicKey(publicKey));

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
            Assert.False(RSAHelper.ValidatePublicKey(publicKey));
        }

        #endregion Public Methods
    }
}