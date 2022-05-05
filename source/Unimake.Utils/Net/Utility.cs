using System;
using System.Net;

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

        private static bool HasInternetConnection(HttpWebRequest client)
        {
            if(client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            try
            {
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
            var proxy = autoDetect ? WebRequest.GetSystemWebProxy() : WebRequest.DefaultWebProxy;

            if(proxy == null)
            {
                return default;
            }

            if(!String.IsNullOrEmpty(user) &&
               !String.IsNullOrEmpty(password))
                proxy.Credentials = new NetworkCredential(user, password);

            return proxy;
        }

        /// <summary>
        /// Verifica a conexão com a internet e retorna verdadeiro se conectado com sucesso
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>

        public static bool HasInternetConnection(IWebProxy proxy)
        {
            var client = WebRequest.Create(InternetURL) as HttpWebRequest;

            if(proxy != null)
            {
                client.Proxy = proxy;
            }

            try
            {
                return HasInternetConnection(client);
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
        public static bool HasInternetConnection()
        {
            return HasInternetConnection(WebRequest.Create(InternetURL) as HttpWebRequest);
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