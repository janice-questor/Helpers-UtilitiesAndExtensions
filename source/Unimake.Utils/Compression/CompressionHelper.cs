using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace Unimake.Compression
{
    /// <summary>
    /// Utilitário de compressão e descompressão de strings e XMLs
    /// </summary>
    public class CompressionHelper
    {
        #region Private Constructors

        /// <summary>
        /// Impede a criação da instância
        /// </summary>
        private CompressionHelper()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Comprimir o conteúdo em <paramref name="xmlDocument"/> no padrão GZIP
        /// <para><see cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Headers/Content-Encoding"/></para>
        /// </summary>
        /// <param name="xmlDocument">Valor para compressão</param>
        /// <returns>Valor comprimido no padrão GZIP
        /// <para><see cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Headers/Content-Encoding"/></para>
        /// </returns>
        public static string GZIPCompress(XmlDocument xmlDocument) => GZIPCompress(xmlDocument.InnerXml);

        /// <summary>
        /// Comprimir a string no padrão GZIP
        /// <para><see cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Headers/Content-Encoding"/></para>
        /// </summary>
        /// <param name="value">Valor para compressão</param>
        /// <returns>Valor comprimido no padrão GZIP
        /// <para><see cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Headers/Content-Encoding"/></para>
        /// </returns>
        public static string GZIPCompress(string value)
        {
            var buffer = Encoding.UTF8.GetBytes(value);
            var ms = new MemoryStream();
            using(var zip = new GZipStream(ms, CompressionMode.Compress))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            return Convert.ToBase64String(ms.GetBuffer());
        }

        /// <summary>
        /// Descomprimir a string no padrão GZIP
        /// <para><see cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Headers/Content-Encoding"/></para>
        /// </summary>
        /// <param name="value">Valor para descompressão</param>
        /// <returns>Valor descomprimido, retorna ao estado original
        /// <para><see cref="https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Headers/Content-Encoding"/></para>
        /// <para>Se o valor definido em <paramref name="input"/> for nulo ou vazio, retorna <see cref="string.Empty"/></para>
        /// </returns>
        public static string GZIPDecompress(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var encodedDataAsBytes = Convert.FromBase64String(input);
            using(Stream comp = new MemoryStream(encodedDataAsBytes))
            {
                using(Stream decomp = new GZipStream(comp, CompressionMode.Decompress, false))
                {
                    using(var sr = new StreamReader(decomp))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        #endregion Public Methods
    }
}