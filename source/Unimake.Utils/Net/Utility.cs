using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace Unimake.Net
{
    /// <summary>
    /// Utilitários de rede
    /// </summary>
    public sealed class Utility
    {
        #region Private Constructors

        private Utility()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        private static string GetLocalIPV4Address(bool ignoreLoopback = true)
        {
            var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                               .Where(w => w.AddressFamily == AddressFamily.InterNetwork);

            foreach(var ip in addresses)
            {
                if(ignoreLoopback && IPAddress.IsLoopback(ip))
                {
                    continue;
                }

                return ip.ToString();
            }

            return null;
        }

        private static string GetLocalIPV6Address(bool ignoreLoopback = true)
        {
            var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                               .Where(w => w.AddressFamily == AddressFamily.InterNetworkV6);

            foreach(var ip in addresses)
            {
                if(ignoreLoopback && IPAddress.IsLoopback(ip))
                {
                    continue;
                }

                return ip.ToString();
            }

            return null;
        }

        /// <summary>
        /// Testar conexão HTTP ou HTTPS
        /// </summary>
        /// <param name="url">URL a ser testada</param>
        /// <param name="certificate">Certificado a ser utilizado para conexões https</param>
        /// <param name="proxy">Configuração de proxy, caso exista</param>
        /// <param name="method">GET ou POST (Tem URLs que deve ser testado através do GET, outras o POST)</param>
        /// <returns>Se a URL está respondendo, ou não</returns>
        public static bool TestHttpConnection(string url, X509Certificate2 certificate = null, int timeoutInSeconds = 3, IWebProxy proxy = null, string method = "GET")
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                if(proxy != null)
                {
                    httpWebRequest.Proxy = proxy;
                }

                httpWebRequest.Method = method;

                if(certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add(certificate);
                }

                httpWebRequest.Timeout = timeoutInSeconds * 1000;
                httpWebRequest.ReadWriteTimeout = timeoutInSeconds * 1000;

                var statusCode = (httpWebRequest.GetResponse() as HttpWebResponse).StatusCode;
                return statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent;
            }
            catch
            {
                return false;
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Retorna o número de IP local.
        /// </summary>
        /// <param name="returnIPV6">Se verdadeiro, retorna o IPV6 ao invés do IPV4</param>
        /// <param name="ignoreLoopback">Ignora o IP ::1 ou 127.0.0.1</param>
        /// <param name="returnErrorIfNotFound">Se verdadeiro, retorna erro se não for encontrado o IP</param>
        /// <returns></returns>
        /// <exception cref="Exception">Se <paramref name="returnErrorIfNotFound"/> for verdadeiro, é lançado um erro ao não encontrar um IP</exception>
        public static string GetLocalIPAddress(bool returnIPV6 = false, bool ignoreLoopback = true, bool returnErrorIfNotFound = true) =>
            (returnIPV6 ? GetLocalIPV6Address(ignoreLoopback) : GetLocalIPV4Address(ignoreLoopback)) ??
                (returnErrorIfNotFound ? throw new Exception($"No network adapters with an {(returnIPV6 ? "IPv6" : "IPv4")} address in the system!") : "");

        /// <summary>
        /// Cria um novo objeto de proxy com base nos parâmetros passados
        /// </summary>
        /// <param name="server">Endereço de servidor do proxy</param>
        /// <param name="user">Usuário do proxy</param>
        /// <param name="password">Senha de acesso do proxy</param>
        /// <param name="port">Porta</param>
        /// <param name="autoDetect">Se verdadeiro, utiliza o proxy padrão do sistema</param>
        /// <returns></returns>
        public static IWebProxy GetProxy(string server,
                                         string user,
                                         string password,
                                         int port,
                                         bool autoDetect = false)
        {
            //Discards ...
            _ = server;
            _ = port;

            var proxy = autoDetect ? WebRequest.GetSystemWebProxy() : WebRequest.DefaultWebProxy;

            if(proxy == null)
            {
                return default;
            }

            if(!string.IsNullOrEmpty(user) &&
               !string.IsNullOrEmpty(password))
                proxy.Credentials = new NetworkCredential(user, password);

            return proxy;
        }


        /// <summary>
        /// Verifica a conexão com a internet e retorna verdadeiro se conectado com sucesso
        /// </summary>
        /// <param name="proxy">Proxy a ser utilizado para testar a conexão</param>
        /// <param name="timeoutInSeconds">Tempo para tentativa de conexão em segundos</param>
        /// <param name="testUrls">URLs a serem testadas, se não informada o método utilizará 5 URLs para o teste, se uma delas funcionar, vai retornar que a conexão está ok</param>
        /// <returns>true = Tem conexão com a internet</returns>
        public static bool HasInternetConnection(IWebProxy proxy, int timeoutInSeconds = 3, string[] testUrls = null)
        {
            if(timeoutInSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException("O valor  do parâmetro 'timeoutInSeconds' deve ser maior que zero.");
            }

            if(testUrls == null)
            {
                testUrls = new string[] {
                    "http://clients3.google.com/generate_204",
                    "8.8.8.8", //Servidor Primário de DNS do Google
                    "8.8.4.4", //Servidor Secundário de DNS do Google
                    "http://www.microsoft.com",
                    "http://www.cloudflare.com",
                    "1.1.1.1", //Servidor Primário de DNS do Cloudfare
                    "1.0.0.1",  //Servidor Secundário de DNS do Cloudfare
                    "http://www.amazon.com",
                    "9.9.9.9", //Servidor Primário de DNS do Quad 9
                    "149.112.112.112" //Servidor Secundário de DNS do Quad 9
                };
            }

            var retorno = true;
            var timeoutMilleSeconds = (timeoutInSeconds * 1000);

            foreach(var url in testUrls)
            {
                if(url.Substring(0, 7).Equals("http://"))
                {
                    try
                    {
                        retorno = TestHttpConnection(url, null, timeoutInSeconds, proxy);
                    }
                    catch
                    {
                        retorno = false;
                    }
                }
                else
                {
                    // Testar conexão com IP direto do Google
                    retorno = PingHost(url, timeoutInSeconds);
                }

                if(retorno)
                {
                    break;
                }
            }

            return retorno;
        }

        /// <summary>
        /// Executa PING no HOST informado
        /// </summary>
        /// <param name="ipAddress">HOST (IP ou DNS)</param>
        /// <param name="timeoutInSeconds">Tempo máximo para ter uma resposta (timeout)</param>
        /// <returns>Se teve sucesso no PING</returns>
        public static bool PingHost(string ipAddress, int timeoutInSeconds)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(ipAddress, timeoutInSeconds * 1000);
                return (reply.Status == IPStatus.Success);
            }
            catch(PingException)
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica a conexão com a internet e retorna verdadeiro se conectado com sucesso
        /// </summary>
        /// <param name="timeoutInSeconds">Tempo para tentativa de conexão em segundos</param>
        /// <returns>true = Tem conexão com a internet</returns>
        public static bool HasInternetConnection(int timeoutInSeconds = 3)
        {
            return HasInternetConnection(null, timeoutInSeconds, null);
        }

        /// <summary>
        /// Verifica se a URL é alcançável.
        /// </summary>
        /// <param name="url">URL pra verificação</param>
        /// <returns></returns>
        public static WebExceptionStatus IsReachable(string url)
        {
            try
            {
                using(var client = new WebClient())
                {
                    using(client.OpenRead(url))
                    {
                        return WebExceptionStatus.Success;
                    }
                }
            }
            catch(WebException ex)
            {
                return ex.Status;
            }
            catch(Exception)
            {
                return WebExceptionStatus.UnknownError;
            }
        }

        #endregion Public Methods
    }
}