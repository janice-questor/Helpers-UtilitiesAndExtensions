using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml
{
    public static class XmlHelper
    {
        #region Private Classes

        private class Utf8StringWriter : StringWriter
        {
            #region Public Properties

            public override Encoding Encoding => Encoding.UTF8;

            #endregion Public Properties
        }

        #endregion Private Classes

        #region Public Methods

        /// <summary>
        /// Converter o XML em <paramref name="xml"/> no tipo <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Tipo de objeto esperado após a conversão</typeparam>
        /// <param name="xml">String com um XML válido</param>
        /// <returns>String XML em <paramref name="xml"/> no tipo <typeparamref name="T"/></returns>
        public static T Deserialize<T>(string xml)
            where T : new()
        {
            if(string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentException($"'{nameof(xml)}' cannot be null or whitespace.", nameof(xml));
            }

            var serializer = new XmlSerializer(typeof(T));
            var stream = new StringReader(xml);
            return (T)serializer.Deserialize(stream);
        }

        public static T Deserialize<T>(this XmlDocument doc)
                    where T : new()
        {
            if(doc is null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            return Deserialize<T>(doc.OuterXml);
        }

        /// <summary>
        /// Lê o conteúdo da tag retorna.
        /// <para>Se a tag não existir no XML retorna uma string vazia.</para>
        /// </summary>
        /// <param name="xmlElement">Elemento que possivelmente contem a tag definida em <paramref name="tagName"/></param>
        /// <param name="tagName">Nome da tag existente em <paramref name="xmlElement"/></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="xmlElement"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se <paramref name="tagName"/> for um nome vazio, nulo ou espaços em branco.</exception>
        public static string ReadTagValue(this XmlElement xmlElement, string tagName)
        {
            if(xmlElement is null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            if(string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentException($"'{nameof(tagName)}' cannot be null or whitespace.", nameof(tagName));
            }

            if(!xmlElement.TagExist(tagName))
            {
                return "";
            }

            return xmlElement.GetElementsByTagName(tagName)[0].InnerText;
        }

        /// <summary>
        /// Converte o objeto do tipo <typeparamref name="T"/> em um <see cref="XmlDocument"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Objeto para conversão</param>
        /// <param name="namespaces">Namespaces definidos no XML, padrão é nulo</param>
        /// <returns>Objeto do tipo <typeparamref name="T"/> em um <see cref="XmlDocument"/></returns>
        public static XmlDocument Serialize<T>(T obj, List<(string Namespace, string Prefix)> namespaces = null)
        {
            if(ReferenceEquals(obj, null))
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var ns = new XmlSerializerNamespaces();

            namespaces?.ForEach(n =>
            {
                ns.Add(n.Prefix, n.Namespace);
            });

            var xmlSerializer = new XmlSerializer(obj.GetType());
            var doc = new XmlDocument();

            using(StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, obj, ns);
                doc.LoadXml(textWriter.ToString());
            }

            return doc;
        }

        /// <summary>
        /// Retorna verdadeiro se a tag definida em <paramref name="tagName"/> existir, se não falso.
        /// </summary>
        /// <param name="xmlElement"></param>
        /// <param name="tagName"></param>
        /// <returns>verdadeiro se a tag definida em <paramref name="tagName"/> existir, se não falso.</returns>
        /// <exception cref="ArgumentNullException">Se <paramref name="xmlElement"/> for nulo</exception>
        /// <exception cref="ArgumentException">Se <paramref name="tagName"/> for nulo, vazio ou espaços em brancos. </exception>
        public static bool TagExist(this XmlElement xmlElement, string tagName)
        {
            if(xmlElement is null)
            {
                throw new ArgumentNullException(nameof(xmlElement));
            }

            if(string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentException($"'{nameof(tagName)}' cannot be null or whitespace.", nameof(tagName));
            }

            return xmlElement.GetElementsByTagName(tagName).Count != 0;
        }

        #endregion Public Methods
    }
}