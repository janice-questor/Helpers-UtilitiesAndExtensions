using EBank.Solutions.Primitives.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Xunit;
using Xunit.Abstractions;

namespace EBank.Solutions.EBoleto.Test.PDF
{
    public class LinkSignerTest
    {
        #region Private Fields

        private static ITestOutputHelper testOutput;

        #endregion Private Fields

        #region Private Methods

        private static void Assert(string key, string link)
        {
            //só decodificar
            var decoded = LinkSigner.Decode(link, key, false);

            DumpObject(decoded);

            //validar o link
            decoded = LinkSigner.ValidedtAndGetValues(link, key);

            DumpObject(decoded);

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                key += "abc123";//só para invalidar a chave
                LinkSigner.ValidedtAndGetValues(link, key);
            });

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                var parts = link.Split(new[] { '.' });
                parts[1] += "abc123";//só para invalidar o header
                LinkSigner.ValidedtAndGetValues(link, key);
            });

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                var parts = link.Split(new[] { '.' });
                parts[2] += "abc123";//só para invalidar o payload
                LinkSigner.ValidedtAndGetValues(link, key);
            });

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                var parts = link.Split(new[] { '.' });
                parts[3] += "abc123";//só para invalidar o hash
                LinkSigner.ValidedtAndGetValues(link, key);
            });
        }

        private static void BuidlLink(out string key, out string link, IEnumerable<(string Name, object Value)> queryString = null)
        {
            key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var url = "https://example.com.br/Download/pdf";
            link = LinkSigner.SignLink(url, "unimake", "pdf", new (string Key, object Value)[]
            {
                ("pdfId", 123),
                ("user", 550),
                ("info", "Info teste"),
            }, key,
            queryString);
        }

        private static void DumpObject(object obj) => testOutput.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));

        #endregion Private Methods

        #region Public Constructors

        public LinkSignerTest(ITestOutputHelper output) => testOutput = output;

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public void SignLinkNoQueryString()
        {
            string key, link;
            BuidlLink(out key, out link);
            DumpObject(link);
            Assert(key, link);
        }

        [Fact]
        public void SignLinkWithQueryString()
        {
            string key, link;
            BuidlLink(out key, out link, new (string, object)[] {
                ("param1", "value1"),
                ("param2", "value2"),
                ("param3", "value3"),
                ("paramN", "valueN"),
            });
            DumpObject(link);
            Assert(key, link);
        }

        #endregion Public Methods
    }
}