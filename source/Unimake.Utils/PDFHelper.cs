using System;
using System.IO;
using System.Text;

namespace Unimake
{
    public class PDFHelper
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
        public static void WriteBase64ToPDFFile(string content, string path)
        {
            if(content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            WriteBytesToPDFFile(Convert.FromBase64String(content), path);
        }

        /// <summary>
        /// Escreve os bytes um arquivo PDF.
        /// <para>Os bytes já devem ser um PDF válido. Este método apenas escreve o arquivo</para>
        /// </summary>
        /// <param name="byteArray">Bytes que serão escritos no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde deve ser gravado o PDF</param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteBytesToPDFFile(byte[] byteArray, string path)
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
        /// Escreve uma string base64 em um arquivo PDF.
        /// <para>A string já deve ser um PDF válido. Este método apenas escreve o arquivo</para>
        /// </summary>
        /// <param name="content">Conteúdo que será escrito no arquivo</param>
        /// <param name="path">Pasta e nome do arquivo onde deve ser gravado o PDF</param>
        /// <param name="encoding">Se nulo, será usado <see cref="UTF32Encoding"/></param>
        /// <exception cref="ArgumentNullException">Se o <paramref name="content"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se o <paramref name="path"/> for nulo, vazio ou espaços</exception>
        public static void WriteStringToPDFFile(string content, string path, Encoding encoding = null)
        {
            if(content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if(encoding is null)
            {
                encoding = Encoding.UTF8;
            }

            WriteBytesToPDFFile(encoding.GetBytes(content), path);
        }

        #endregion Public Methods
    }
}