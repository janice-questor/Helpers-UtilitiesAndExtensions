using System;
using System.Text;

namespace Unimake
{
    public abstract class PDFHelper
    {
        #region Private Constructors

        /// <summary>
        /// Impede de criar a instância
        /// </summary>
        private PDFHelper()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Escreve uma string base64 em um arquivo PDF.
        /// <para>A string já deve ser um PDF válido. Este método apenas escreve o arquivo</para>
        /// </summary>
        /// <param name="content">Conteúdo que será escrito no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde deve ser gravado o PDF</param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteBase64ToPDFFile(string content, string path) =>
            FileHelper.WriteBase64ToFile(content, path);

        /// <summary>
        /// Escreve os bytes um arquivo PDF.
        /// <para>Os bytes já devem ser um PDF válido. Este método apenas escreve o arquivo</para>
        /// </summary>
        /// <param name="byteArray">Bytes que serão escritos no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde deve ser gravado o PDF</param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteBytesToPDFFile(byte[] byteArray, string path) =>
            FileHelper.WriteBytesToFile(byteArray, path);

        /// <summary>
        /// Escreve uma string base64 em um arquivo PDF.
        /// <para>A string já deve ser um PDF válido. Este método apenas escreve o arquivo</para>
        /// </summary>
        /// <param name="content">Conteúdo que será escrito no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde deve ser gravado o PDF</param>
        /// <param name="encoding">Se nulo, será usado <see cref="Convert.FromBase64String(string)"/> para leitura dos bytes</param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteStringToPDFFile(string content, string path, Encoding encoding = null) =>
            FileHelper.WriteStringToFile(content, path, encoding);

        #endregion Public Methods
    }
}