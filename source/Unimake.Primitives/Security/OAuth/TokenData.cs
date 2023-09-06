using Newtonsoft.Json;

namespace Unimake.Primitives.Security.OAuth
{
    /// <summary>
    /// Dados do token retornados pelos serviços de autorização
    /// </summary>
    /// <remarks>
    /// <see href="https://www.oauth.com/oauth2-servers/access-tokens/access-token-response/"/>
    /// </remarks>
    public struct TokenData
    {
        #region Public Properties

        /// <summary>
        /// Token de acesso
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// tempo de expiração do token em segundos
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// Política de emissão de token
        /// </summary>
        [JsonProperty("not-before-policy")]
        public long NotBeforePolicy { get; set; }

        /// <summary>
        /// Tempo em que a atualização do token expira
        /// </summary>
        [JsonProperty("refresh_expires_in")]
        public long RefreshExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Escopo de uso do token
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Tipo de token
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Converte este tipo em uma string
        /// </summary>
        /// <param name="rhs">Tipo para converter</param>
        public static implicit operator string(TokenData rhs) => rhs.ToString();

        /// <summary>
        /// Converte em uma string no padrão <see cref="TokenType"/> <see cref="AccessToken"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{TokenType} {AccessToken}";

        #endregion Public Methods
    }
}