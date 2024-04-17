using EBank.Solutions.Primitives.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Unimake.Cryptography.Enumerations;
using Unimake.Cryptography.Exceptions;
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
            decoded = LinkSigner.ValidateAndGetValues(link, key);

            DumpObject(decoded);

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                key += "abc123";//só para invalidar a chave
                LinkSigner.ValidateAndGetValues(link, key);
            });

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                var parts = link.Split(new[] { '.' });
                parts[1] += "abc123";//só para invalidar o header
                LinkSigner.ValidateAndGetValues(link, key);
            });

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                var parts = link.Split(new[] { '.' });
                parts[2] += "abc123";//só para invalidar o payload
                LinkSigner.ValidateAndGetValues(link, key);
            });

            Xunit.Assert.Throws<CryptographicException>(() =>
            {
                var parts = link.Split(new[] { '.' });
                parts[3] += "abc123";//só para invalidar o hash
                LinkSigner.ValidateAndGetValues(link, key);
            });
        }

        private static void BuidlLink(out string key,
                                      out string link,
                                      IEnumerable<(string Name, object Value)> queryString = null,
                                      long? iat = null)
        {
            key = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var url = "https://example.com.br/Download/pdf";
            link = LinkSigner.SignLink(url, "unimake", "pdf", new (string Key, object Value)[]
            {
                ("pdfId", 123),
                ("user", 550),
                ("info", "Info teste"),
            }, key,
            queryString,
            iat: iat);
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
            BuidlLink(out var key, out var link);
            DumpObject(link);
            Assert(key, link);
        }

        [Fact]
        public void SignLinkNoURL()
        {
            var link = LinkSigner.SignLink("unimake", "pdf", new (string Key, object Value)[]
            {
                ("pdfId", 123),
                ("user", 550),
                ("info", "Info teste"),
            }, "Public_Key");

            DumpObject(link);
        }

        [Fact]
        public void SignLinkWithQueryString()
        {
            BuidlLink(out var key, out var link, new (string, object)[] {
                ("param1", "value1"),
                ("param2", "value2"),
                ("param3", "value3"),
                ("paramN", "valueN"),
            });
            DumpObject(link);
            Assert(key, link);
        }

        [Theory]
        [InlineData(ExpirationInterval.Seconds, 10)]
        [InlineData(ExpirationInterval.Minutes, 10)]
        [InlineData(ExpirationInterval.Hours, 10)]
        [InlineData(ExpirationInterval.Days, 10)]
        public void ValidateIatExpirationTest(ExpirationInterval expirationInterval, int interval)
        {
            //- interval para ter certeza que vai dar erro
            var iat = LinkSigner.GetEpoch() - interval;
            BuidlLink(out var key, out var link, new (string, object)[] {
                ("param1", "value1"),
                ("param2", "value2"),
                ("param3", "value3"),
                ("paramN", "valueN"),
            }, iat);

            DumpObject(link);

            var decoded = LinkSigner.Decode(link, key, false);
            Xunit.Assert.Throws<SecurityTokenExpiredException>(() => LinkSigner.ValidateIatExpiration(decoded, expirationInterval, 0));
        }

        [Fact]
        public void ValidateIat30SecondsExpirationTest()
        {
            BuidlLink(out var key, out var link, new (string, object)[] {
                ("param1", "value1"),
                ("param2", "value2"),
                ("param3", "value3"),
                ("paramN", "valueN"),
            });

            DumpObject(link);

            var seconds = 30;
            var decoded = LinkSigner.Decode(link, key, false);
            var start = DateTime.UtcNow.AddSeconds(seconds);
            var elapsed = DateTime.UtcNow - start;

            while(elapsed.TotalSeconds <= -1)
            {
                //não tem que dar erro
                LinkSigner.ValidateIatExpiration(decoded, ExpirationInterval.Seconds, 30);
                elapsed = DateTime.UtcNow - start;
                System.Diagnostics.Trace.WriteLine(elapsed.ToString());
            }

            //aguarda 5 segundos
            Thread.Sleep(5000);

            //aqui tem que dar erro
            Xunit.Assert.Throws<SecurityTokenExpiredException>(() => LinkSigner.ValidateIatExpiration(decoded, ExpirationInterval.Seconds, seconds));
        }

        #endregion Public Methods
    }
}