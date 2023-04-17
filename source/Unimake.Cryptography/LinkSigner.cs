using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Unimake.Cryptography.Enumerations;
using Unimake.Cryptography.Exceptions;
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
#pragma warning disable SYSLIB0013 // Type or member is obsolete
            //O novo método sugerido dá erro ao validar a URL
            url = Uri.EscapeUriString(url + NameValueToQueryString(queryString));
#pragma warning restore SYSLIB0013 // Type or member is obsolete
            return url;
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

        #region Public Fields

        public static readonly DateTime UnixEpoch;

        #endregion Public Fields

        #region Public Constructors

        static LinkSigner() => UnixEpoch = DateTime.Parse("1970-01-01T00:00:00");

        #endregion Public Constructors

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
        /// Retorna a data/hora no padrão EPOCH. <see href="https://www.rfc-editor.org/rfc/rfc7519#section-4.1.6"/>
        /// </summary>
        /// <returns></returns>
        public static long GetEpoch()
        {
            var t = DateTime.UtcNow - UnixEpoch;
            return (long)t.TotalSeconds;
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
        /// <param name="subject">Assunto sobre o que é o link</param>
        /// <param name="iat">Issued At, data/hora padrão EPOCH em que a assinatura foi gerada, se nada passado, assume
        /// <code>LinkSigner.GetEpoch();</code></param>
        /// <returns></returns>
        public static string SignLink(string url,
                                      string issuer,
                                      string subject,
                                      IEnumerable<(string Key, object Value)> claims,
                                      string publicKey,
                                      IEnumerable<(string Name, object Value)> queryString = null,
                                      string encodedParamenterName = "code",
                                      long? iat = null)
        {
            var payload = new Dictionary<string, object>();

            payload.Add("sub", subject);
            payload.Add("iss", issuer);
            payload.Add("iat", iat ?? GetEpoch());

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
        public static Dictionary<string, object> ValidateAndGetValues(string url, string publicKey, string encodedParamenterName = "code") =>
            Decode(url, publicKey, true, encodedParamenterName);

        /// <summary>
        /// Valida se o tempo de expiração da emissão da chave não ultrapassou o limite informado em <paramref name="interval"/>
        /// </summary>
        /// <param name="decode">Valores decodificados</param>
        /// <param name="interval">Limite, em horas, para validação do tempo de expiração.</param>
        /// <param name="expirationInterval">Intervalo de expiração</param>
        /// <param name="throwError">Se verdadeiro, é lançada a exceção <see cref="SecurityTokenExpiredException"/> </param>
        /// <exception cref="ArgumentNullException">Se <paramref name="decode"/> for nulo</exception>
        /// <exception cref="SecurityTokenExpiredException">Se o token estiver expirado e <paramref name="throwError"/> for verdadeiro</exception>
        /// <returns>Verdadeiro, se o token é valido. Ou falso, se for inválido se o parâmetro <paramref name="throwError"/> for falso.
        /// Caso contrário lança a exceção <see cref="SecurityTokenExpiredException"/></returns>
        public static bool ValidateIatExpiration(Dictionary<string, object> decode,
                                                 ExpirationInterval expirationInterval = ExpirationInterval.Hours,
                                                 int interval = 48,
                                                 bool throwError = true)
        {
            if(decode is null)
            {
                throw new ArgumentNullException(nameof(decode));
            }

            //epoch
            var epoch = UnixEpoch;
            //valor do IAT
            var iat = epoch.AddSeconds(Convert.ToInt64(decode["iat"]));
            //atual
            var current = DateTime.UtcNow;
            //diferença
            var diff = current - iat;
            //se a diferença for maior que "expiration". Estourou o limite de tempo do token

            var expired = ((bool Expired, string Description))(false, "");

            switch(expirationInterval)
            {
                case ExpirationInterval.Seconds:
                    expired.Expired = ((double)diff.Ticks / TimeSpan.TicksPerSecond) > interval;
                    expired.Description = "Segundos";
                    break;

                case ExpirationInterval.Minutes:
                    expired.Expired = ((double)diff.Ticks / TimeSpan.TicksPerMinute) > interval;
                    expired.Description = "Minutos";
                    break;

                case ExpirationInterval.Days:
                    expired.Expired = ((double)diff.Ticks / TimeSpan.TicksPerDay) > interval;
                    expired.Description = "Dias";
                    break;

                case ExpirationInterval.Hours:
                default:
                    expired.Expired = ((double)diff.Ticks / TimeSpan.TicksPerHour) > interval;
                    expired.Description = "Horas";
                    break;
            }

            if(expired.Expired)
            {
                var message = $"O token informado tem mais de {interval} {expired.Description} e não é mais válido.";

                System.Diagnostics.Trace.WriteLine(message);

                if(!throwError)
                {
                    return false;
                }

                throw new SecurityTokenExpiredException(message);
            }

            return true;
        }

        #endregion Public Methods
    }
}