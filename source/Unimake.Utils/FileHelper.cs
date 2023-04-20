using System;
using System.IO;
using System.Text;

namespace Unimake
{
    public abstract class FileHelper
    {
        #region Private Constructors

        /// <summary>
        /// Impede de criar a instância
        /// </summary>
        private FileHelper()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Converte um arquivo em Base64
        /// </summary>
        /// <param name="path">Caminho completo do arquivo</param>
        /// <returns></returns>
        public static string ConvertFileToBase64(string path) => Convert.ToBase64String(File.ReadAllBytes(path));

        /// <summary>
        /// Escreve uma string base64 em um arquivo.
        /// </summary>
        /// <param name="content">Conteúdo que será escrito no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde serão gravados os dados definidos em <paramref name="content"/> </param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteBase64ToFile(string content, string path)
        {
            if(content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            WriteBytesToFile(Convert.FromBase64String(content), path);
        }

        /// <summary>
        /// Escreve os bytes um arquivo.
        /// </summary>
        /// <param name="byteArray">Bytes que serão escritos no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde serão gravados os bytes definidos em <paramref name="byteArray"/> </param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteBytesToFile(byte[] byteArray, string path)
        {
            if(byteArray is null)
            {
                throw new ArgumentNullException(nameof(byteArray));
            }

            if(string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            }

            var fi = new FileInfo(path);

            if(!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            if(fi.Exists)
            {
                fi.Delete();
            }

            File.WriteAllBytes(fi.FullName, byteArray);
        }

        /// <summary>
        /// Escreve uma string base64 em um arquivo.
        /// </summary>
        /// <param name="content">Conteúdo que será escrito no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde devem ser gravados o conteúdo definido em <paramref name="content"/></param>
        /// <param name="encoding">Se nulo, será usado <see cref="Convert.FromBase64String(string)"/> para leitura dos bytes</param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteStringToFile(string content, string path, Encoding encoding = null)
        {
            if(content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var bytes = encoding is null ? Convert.FromBase64String(content) : Encoding.UTF8.GetBytes(content);

            WriteBytesToFile(bytes, path);
        }

        #endregion Public Methods
    }
}