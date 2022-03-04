using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Unimake.Cryptography
{
    /// <summary>
    /// Classe de criptografia e descriptografia
    /// </summary>
    public class Rijndael
    {
        #region Private Fields

        private static readonly byte[] Vector = { 146, 64, 191, 111, 23, 3, 154, 119, 111, 121, 221, 112, 79, 32, 45, 156 };
        private readonly ICryptoTransform decryptor;
        private readonly UTF8Encoding encoder;
        private readonly ICryptoTransform encryptor;

        #endregion Private Fields

        #region Protected Methods

        /// <summary>
        /// Transforma um array de bytes em criptografado ou descriptografado
        /// </summary>
        /// <param name="buffer">array de bytes, pode estar criptografado ou descriptografado</param>
        /// <param name="transform">
        /// Utilizado para transformar o array de bytes em criptografado ou descriptografado
        /// </param>
        /// <returns></returns>
        protected byte[] Transform(byte[] buffer, ICryptoTransform transform)
        {
            var stream = new MemoryStream();
            using(var cs = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }

        #endregion Protected Methods

        #region Internal Fields

        internal static byte[] Key = { 164, 218, 19, 11, 28, 26, 85, 42, 114, 184, 24, 162, 37, 112, 222, 209, 241, 24, 171, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };

        #endregion Internal Fields

        #region Public Constructors

        /// <summary>
        /// Inicia uma nova instancia com as configurações padrões da criptografia
        /// </summary>
        public Rijndael()
            : this(Key, Vector)
        {
        }

        /// <summary>
        /// Inicia uma nova instância e define a chave
        /// </summary>
        /// <param name="key">Chave de criptografia</param>
        public Rijndael(string key)
            : this(Encoding.UTF8.GetBytes(key), Vector)
        {
        }

        /// <summary>
        /// Inicia uma nova instância e define a chave e o vetor
        /// </summary>
        /// <param name="key">Chave de criptografia</param>
        /// <param name="vector">Define o vetor de criptografia</param>
        public Rijndael(string key, string vector)
            : this(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(vector))
        {
        }

        /// <summary>
        /// Inicia uma nova instância e define a chave e o vetor
        /// </summary>
        /// <param name="key">Chave de criptografia</param>
        public Rijndael(byte[] key)
            : this(key, Vector)
        {
        }

        /// <summary>
        /// Inicia uma nova instância e define a chave e o vetor
        /// </summary>
        /// <param name="key">Chave de criptografia</param>
        /// <param name="vector">Define o vetor de criptografia</param>
        /// <remarks>Este construtor é o padrão para todos os outros</remarks>
        public Rijndael(byte[] key, byte[] vector)
        {
            var rm = new RijndaelManaged();
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
            encoder = new UTF8Encoding();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Descriptografa a string passada e retorna
        /// </summary>
        /// <param name="encrypted">String criptografada</param>
        /// <returns>String descriptografada</returns>
        public string Decrypt(string encrypted)
        {
            var b = Convert.FromBase64String(encrypted);
            b = Decrypt(b);
            return encoder.GetString(b, 0, b.Length);
        }

        /// <summary>
        /// Descriptografa um array de bytes passado e retorna
        /// </summary>
        /// <param name="buffer">Array de bytes criptografados</param>
        /// <returns>Array de bytes descriptografados</returns>
        public byte[] Decrypt(byte[] buffer) => Transform(buffer, decryptor);

        /// <summary>
        /// Criptografa a string passada e retorna
        /// </summary>
        /// <param name="unencrypted">String sem criptografar</param>
        /// <returns>String criptografada</returns>
        public string Encrypt(string unencrypted) => Convert.ToBase64String(Encrypt(encoder.GetBytes(unencrypted)));

        /// <summary>
        /// Criptografa o array de bytes passado e retorna
        /// </summary>
        /// <param name="buffer">Bytes sem criptografar</param>
        /// <returns>Bytes criptografados</returns>
        public byte[] Encrypt(byte[] buffer) => Transform(buffer, encryptor);

        #endregion Public Methods
    }
}