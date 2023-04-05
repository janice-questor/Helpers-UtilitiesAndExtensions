using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Unimake.Cryptography.JWT;

namespace EBank.Solutions.Primitives.Security
{
    /// <summary>
    /// Serviço para assinatura e geração de links.
    /// <para>Assina o link e retorna a assinatura no parâmetro code=sign_with_sha256_JWT</para>
    /// </summary>
    public abstract class LinkSigner
    {
        #region Private Constructors

        private LinkSigner()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        private static string EnsureURL(string url, IEnumerable<(string Name, object Value)> queryString)
        {
            url = url.TrimEnd('/');
            url = Uri.EscapeUriString(url + NameValueToQueryString(queryString));
            return url;
        }

        private static long GetEpoch()
        {
            //https://www.rfc-editor.org/rfc/rfc7519#section-4.1.6
            TimeSpan t = DateTime.UtcNow - DateTime.Parse("1970-01-01T00:00:00");
            return (long)t.TotalSeconds;
        }

        private static string NameValueToQueryString(IEnumerable<(string Name, object Value)> values)
        {
            if(values is null)
            {
                return "";
            }

            var sb = new StringBuilder();
            var separator = (string)null;

            foreach(var value in values)
            {
                var paramValue = System.Web.HttpUtility.UrlEncode(value.Value?.ToString() ?? "");
                sb.Append($"{separator}{value.Name}={paramValue}");
                separator = separator ?? (separator = "&");
            }

            sb.Insert(0, "?");

            return sb.ToString();
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Decodifica a URL e retorna um <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="url">URL com uma assinatura. O parâmetro da assinatura é recuperado pelo nome em <paramref name="encodedParamenterName"/></param>
        /// <param name="publicKey">Chave pública utilizada para a geração da assinatura</param>
        /// <param name="encodedParamenterName">Nome do parâmetro na URL que possui a assinatura</param>
        /// <param name="verify">Se verdadeiro, valida a </param>
        /// <returns></returns>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o parâmetro <paramref name="encodedParamenterName"/> não for localizado</exception>
        /// <exception cref="CryptographicException">Se <paramref name="verify"/> for verdadeiro e se a assinatura for inválida</exception>
        public static Dictionary<string, object> Decode(string url, string publicKey, bool verify, string encodedParamenterName = "code")
        {
            var uri = new Uri(url);
            var code = HttpUtility.ParseQueryString(uri.Query)[encodedParamenterName];

            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentOutOfRangeException($"There is no '{encodedParamenterName}' in the QueryString ", default(Exception));
            }

            return JsonWebToken.Decode(code, publicKey, verify);
        }

        /// <summary>
        /// Realiza a assinatura do link com JWT HS512 e retorna no parâmetro <paramref name="encodedParamenterName"/>=_SINGATURE_BASE64_HS512
        /// </summary>
        /// <param name="publicKey">chave pública</param>
        /// <param name="queryString">Parâmetros do link</param>
        /// <param name="encodedParamenterName">Nome do parâmetro retornado na URL</param>
        /// <param name="claims">Dados informativos para inserir junto a assinatura, pode ser usado após a decodificação do link</param>
        /// <param name="issuer">Aquele que emitiu este link</param>
        /// <param name="subject">Assunto/ sobre o que é o link</param>
        /// <returns></returns>
        public static string SignLink(string issuer,
                                      string subject,
                                      IEnumerable<(string Key, object Value)> claims,
                                      string publicKey,
                                      IEnumerable<(string Name, object Value)> queryString = null,
                                      string encodedParamenterName = "code") => SignLink("", issuer, subject, claims, publicKey, queryString, encodedParamenterName).TrimStart('?');

        /// <summary>
        /// Realiza a assinatura do link com JWT HS512 e retorna no parâmetro <paramref name="encodedParamenterName"/>=_SINGATURE_BASE64_HS512
        /// </summary>
        /// <param name="url">URL para assinatura, sem os parâmetros</param>
        /// <param name="publicKey">chave pública</param>
        /// <param name="queryString">Parâmetros do link</param>
        /// <param name="encodedParamenterName">Nome do parâmetro retornado na URL</param>
        /// <param name="claims">Dados informativos para inserir junto a assinatura, pode ser usado após a decodificação do link</param>
        /// <param name="issuer">Aquele que emitiu este link</param>
        /// <param name="subject">Assunto/ sobre o que é o link</param>
        /// <returns></returns>
        public static string SignLink(string url,
                                      string issuer,
                                      string subject,
                                      IEnumerable<(string Key, object Value)> claims,
                                      string publicKey,
                                      IEnumerable<(string Name, object Value)> queryString = null,
                                      string encodedParamenterName = "code")
        {
            var payload = new Dictionary<string, object>();

            payload.Add("sub", subject);
            payload.Add("iss", issuer);
            payload.Add("iat", GetEpoch());

            if(claims != null)
            {
                foreach(var claim in claims)
                {
                    payload.Add(claim.Key, claim.Value);
                }
            }

            payload.Add("path", EnsureURL(url, null));
            payload.Add("query", NameValueToQueryString(queryString));

            var queryList = (queryString ?? new (string Name, object Value)[] { }).ToArray();
            var encoded = JsonWebToken.Encode(payload, publicKey);

            queryList = queryList.Append((encodedParamenterName, (object)encoded)).ToArray();

            return $"{EnsureURL(url, queryList)}";
        }

        /// <summary>
        /// Valida se a URL possui uma assinatura válida e retorna um <see cref="Dictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="url">URL com uma assinatura. O parâmetro da assinatura é recuperado pelo nome em <paramref name="encodedParamenterName"/></param>
        /// <param name="publicKey">Chave pública utilizada para a geração da assinatura</param>
        /// <param name="encodedParamenterName">Nome do parâmetro na URL que possui a assinatura</param>
        /// <returns></returns>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o parâmetro <paramref name="encodedParamenterName"/> não for localizado</exception>
        /// <exception cref="CryptographicException">Se a assinatura for inválida</exception>
        public static Dictionary<string, object> ValidedtAndGetValues(string url, string publicKey, string encodedParamenterName = "code") =>
            Decode(url, publicKey, true, encodedParamenterName);

        #endregion Public Methods
    }
}