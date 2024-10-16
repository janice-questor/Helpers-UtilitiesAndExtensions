using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace System.Security.Cryptography
{
    public static class RSAExtensions
    {
        #region Public Methods

        public static RSA ImportSubjectPublicKeyInfo(this RSA rsa, byte[] publicKeyInfo)
        {
            try
            {
                // Parse the SubjectPublicKeyInfo using BouncyCastle
                var asn1Stream = new Asn1InputStream(publicKeyInfo);
                var asn1Object = asn1Stream.ReadObject();
                var spki = SubjectPublicKeyInfo.GetInstance(asn1Object);
                var rsaKeyParameters = (RsaKeyParameters)PublicKeyFactory.CreateKey(spki);
                var rsaParameters = new RSAParameters
                {
                    Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned(),
                    Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned()
                };

                rsa.ImportParameters(rsaParameters);

                return rsa;
            }
            catch(Exception ex)
            {
                throw new CryptographicException("Failed to import RSA public key.", ex);
            }
        }

        #endregion Public Methods
    }
}