using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Unimake.Net
{
    /// <summary>
    /// Utilitários de rede
    /// </summary>
    public sealed class Utility
    {
        #region Private Fields

        private const string InternetURL = "http://clients3.google.com/generate_204";

        #endregion Private Fields

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
        /// Verifica se existe conexão com internet
        /// </summary>
        /// <param name="client">Cliente usado para verificação </param>
        /// <param name="timeoutInSeconds">Tempo de espera para resposta</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static bool HasInternetConnection(HttpWebRequest client, int timeoutInSeconds = 3)
        {
            if(client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if(timeoutInSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException($"O valor  do parâmetro '{nameof(timeoutInSeconds)}' deve ser maior que zero.");
            }

            try
            {
                timeoutInSeconds *= 1000;
                client.Timeout = timeoutInSeconds;
                client.ReadWriteTimeout = timeoutInSeconds;
                var response = client.GetResponse() as HttpWebResponse;
                return response.StatusCode == HttpStatusCode.NoContent;
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
        /// <param name="timeoutInSeconds">Tempo de espera para resposta</param>
        /// <param name="proxy"></param>
        /// <returns></returns>

        public static bool HasInternetConnection(IWebProxy proxy, int timeoutInSeconds = 3)
        {
            var client = WebRequest.Create(InternetURL) as HttpWebRequest;

            if(proxy != null)
            {
                client.Proxy = proxy;
            }

            try
            {
                return HasInternetConnection(client, timeoutInSeconds);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica a conexão com a internet e retorna verdadeiro se conectado com sucesso
        /// </summary>
        /// <returns></returns>
        /// <param name="timeoutInSeconds">Tempo de espera para resposta</param>
        public static bool HasInternetConnection(int timeoutInSeconds = 3) => HasInternetConnection(WebRequest.Create(InternetURL) as HttpWebRequest, timeoutInSeconds);

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