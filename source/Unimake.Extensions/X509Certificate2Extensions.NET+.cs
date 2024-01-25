#if NETCOREAPP1_0_OR_GREATER

using System.Diagnostics;
using System.IO;
using System.Linq;

namespace System.Security.Cryptography.X509Certificates
{
    /// <summary>
    /// Extensões para certificados
    /// </summary>
    public static class X509Certificate2Extensions
    {
        #region Private Enums

        private enum KeyFileKinds
        {
            None = 0,
            Pkcs8,
            EncryptedPkcs8,
            RsaPrivateKey,
            Any = -1,
        }

        #endregion Private Enums

        #region Private Methods

        private static byte[] MakePfx(X509Certificate2 cert, ReadOnlySpan<char> key, string exportPassword)
        {
            if(cert is null)
            {
                throw new ArgumentNullException(nameof(cert));
            }
            // Thanks to @bartonjs on https://stackoverflow.com/questions/44465574/net-standard-merge-a-certificate-and-a-private-key-into-a-pfx-file-programma

            var keyBytes = default(byte[]);
            var kinds = KeyFileKinds.Any;

            // PemEncoding.TryFind requires net5.0+
            if(PemEncoding.TryFind(key, out PemFields pemFields))
            {
                keyBytes = new byte[pemFields.DecodedDataLength];

                if(!Convert.TryFromBase64Chars(key[pemFields.Base64Data], keyBytes, out int written) ||
                    written != keyBytes.Length)
                {
                    Debug.Fail("PemEncoding.TryFind and Convert.TryFromBase64Chars disagree on Base64 encoding");
                    throw new InvalidOperationException();
                }

                var label = key[pemFields.Label];

                if(label.SequenceEqual("PRIVATE KEY"))
                {
                    kinds = KeyFileKinds.Pkcs8;
                }
                else if(label.SequenceEqual("ENCRYPTED PRIVATE KEY"))
                {
                    kinds = KeyFileKinds.EncryptedPkcs8;
                }
                else if(label.SequenceEqual("RSA PRIVATE KEY"))
                {
                    kinds = KeyFileKinds.RsaPrivateKey;
                }
                else
                {
                    throw new NotSupportedException($"The PEM file type '{label.ToString()}' is not supported.");
                }
            }
            else
            {
                kinds = KeyFileKinds.Any;
                keyBytes = key.ToArray().Select(s => (byte)s).ToArray();
            }

            var rsa = default(RSA);
            var ecdsa = default(ECDsa);
            var dsa = default(DSA);

            switch(cert.GetKeyAlgorithm())
            {
                case "1.2.840.113549.1.1.1":
                    var cspParams = new CspParameters();
                    cspParams.KeyContainerName = cert.Thumbprint;
                    cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                    rsa = new RSACryptoServiceProvider(cspParams);
                    break;

                case "1.2.840.10045.2.1":
                    ecdsa = ECDsa.Create();
                    break;

                case "1.2.840.10040.4.1":
                    dsa = DSA.Create();
                    break;

                default:
                    throw new NotSupportedException($"The certificate key algorithm '{cert.GetKeyAlgorithm()}' is unknown");
            }

            var anyAlg = rsa ?? ecdsa ?? (AsymmetricAlgorithm)dsa;
            var loaded = false;
            var bytesRead = 0;

            using(rsa)
            using(ecdsa)
            using(dsa)
            {
                if(!loaded && rsa != null && kinds.HasFlag(KeyFileKinds.RsaPrivateKey))
                {
                    try
                    {
                        rsa.ImportRSAPrivateKey(keyBytes, out bytesRead);
                        loaded = bytesRead == keyBytes.Length;
                    }
                    catch(CryptographicException)
                    {
                        // (╯°□°）╯︵ ┻━┻
                    }
                }

                if(!loaded && kinds.HasFlag(KeyFileKinds.Pkcs8))
                {
                    try
                    {
                        anyAlg.ImportPkcs8PrivateKey(keyBytes, out bytesRead);
                        loaded = bytesRead == keyBytes.Length;
                    }
                    catch(CryptographicException)
                    {
                        // (╯°□°）╯︵ ┻━┻
                    }
                }

                if(!loaded && kinds.HasFlag(KeyFileKinds.EncryptedPkcs8))
                {
                    try
                    {
                        // This assumes that the private key was already exported
                        // with the same password that the PFX will be exported with.
                        // Not true? Add a parameter :).
                        anyAlg.ImportEncryptedPkcs8PrivateKey(exportPassword, keyBytes, out bytesRead);
                        loaded = bytesRead == keyBytes.Length;
                    }
                    catch(CryptographicException)
                    {
                        // (╯°□°）╯︵ ┻━┻
                    }
                }

                if(!loaded)
                {
                    // ┬─┬ ノ( ゜-゜ノ)
                    throw new InvalidOperationException("Could not load the key as any known format.");
                }

                var withKey = default(X509Certificate2);

                if(rsa != null)
                {
                    withKey = cert.CopyWithPrivateKey(rsa);
                }
                else if(ecdsa != null)
                {
                    withKey = cert.CopyWithPrivateKey(ecdsa);
                }
                else
                {
                    Debug.Assert(dsa != null);
                    withKey = cert.CopyWithPrivateKey(dsa);
                }

                using(withKey)
                {
                    return withKey.Export(X509ContentType.Pfx, exportPassword);
                }
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Cria um novo certificado com o conteúdo da chave informado em <paramref name="keyContent"/>
        /// </summary>
        /// <param name="certificate">Certificado válido e carregado</param>
        /// <param name="keyContent">Conteúdo da chave</param>
        /// <param name="exportPassword">Mesma senha usada para exportação do certificado</param>
        /// <returns></returns>
        public static X509Certificate2 UseKey(this X509Certificate2 certificate, string keyContent, string exportPassword = "") =>
            new X509Certificate2(MakePfx(certificate, keyContent, exportPassword));

        /// <summary>
        /// Carrega o arquivo informado em <paramref name="keyPath"/> e criar um novo certificado com base na chave
        /// </summary>
        /// <param name="certificate">Certificado válido e carregado</param>
        /// <param name="keyPath">Caminho do arquivo de chave</param>
        /// <param name="exportPassword">Mesma senha usada para exportação do certificado</param>
        /// <returns></returns>
        public static X509Certificate2 UseKeyFromFile(this X509Certificate2 certificate, string keyPath, string exportPassword = "") =>
            new X509Certificate2(MakePfx(certificate, File.ReadAllText(keyPath), exportPassword));

        #endregion Public Methods
    }
}

#endif