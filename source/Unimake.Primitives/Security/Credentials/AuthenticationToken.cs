using Unimake.Primitives.Enumerations;

namespace Unimake.Primitives.Security.Credentials
{
    public struct AuthenticationToken
    {
        #region Public Properties

        /// <summary>
        /// Se o <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, a chave do cliente deve ser informada
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Define como deverá ser usado para realizar a autenticação
        /// </summary>
        public GrantType GrantType { get; set; }

        /// <summary>
        /// Se o <see cref="GrantType"/> for <see cref="GrantType.Password"/>, a senha deve ser informada
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Define o escopo desta autorização
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// Se o <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, o segredo do cliente deve ser informado
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Se o <see cref="GrantType"/> for <see cref="GrantType.Password"/>, o nome de usuário do cliente deve ser informado
        /// </summary>
        public string Username { get; set; }

        #endregion Public Properties
    }
}