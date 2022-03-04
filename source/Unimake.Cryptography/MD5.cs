using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Unimake.Cryptography
{
    /// <summary>
    /// Classe criada para salgar hashs MD5 afim de evitar ataques de colisão, rainbow tables, etc.
    /// </summary>
    public class MD5
    {
        #region Public Properties

        /// <summary>
        /// Hash salgada pela classe.
        /// </summary>
        public string Hash { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Construtor que irá salgar o texto.
        /// </summary>
        /// <param name="text">Texto que será salgado.</param>
        /// <param name="loop">Número de vezes que será criptografado.</param>
        /// <param name="salt">Texto que será utilizado para salgar a criptografia.</param>
        public MD5(string text, int loop, string salt = "")
        {
            if(string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("The text cannot be null, empty or be a white space string.");
            }

            if(loop <= 0)
            {
                throw new ArgumentException("The loop argument cannot be negative or zero");
            }

            using(var md5 = System.Security.Cryptography.MD5.Create())
            {
                Hash = Hash = BitConverter.ToString(md5.ComputeHash(ASCIIEncoding.Default.GetBytes(salt + text))).Replace("-", "").ToLower();

                if(loop > 0)
                {
                    for(var i = -1; i < loop; i++)
                    {
                        Hash = Hash = BitConverter.ToString(md5.ComputeHash(ASCIIEncoding.Default.GetBytes(Hash))).Replace("-", "").ToLower();
                    }
                }
            }
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Retorna verdadeiro se o valor está criptogrado com MD5
        /// </summary>
        /// <param name="value">Valor para verificação</param>
        /// <returns>Verdadeiro se está criptografado com MD5</returns>
        public static bool IsEncrypted(string value) => new Regex("^[0-9a-fA-F]{32}$").IsMatch(value);

        /// <summary>
        /// Cria um texto criptografado em MD5
        /// </summary>
        /// <param name="text">Texto que será salgado.</param>
        /// <param name="loop">Número de vezes que será criptografado.</param>
        /// <param name="salt">Chave do texto que será utilizada para salgar a criptografia.</param>
        public static string Salt(string text, int loop, string salt = "") => new MD5(text, loop, salt).Hash;

        #endregion Public Methods
    }
}