using System;
using System.ComponentModel;
using Unimake.Primitives.Enumerations;

namespace Unimake.Primitives.Security.Credentials
{
    public struct AuthenticationToken
    {
        #region Private Fields

        private GrantType? _grantType;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Se o <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, a chave da aplicação deve ser informada
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Se o <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, a chave do cliente deve ser informada
        /// </summary>
        public string ClientId
        {
            get => AppId;
            set => AppId = value;
        }

        /// <summary>
        /// Define como deverá ser usado para realizar a autenticação
        /// </summary>
        /// <remarks>
        /// O valor padrão é <see cref="GrantType.ClientCredentials"/>
        /// </remarks>
        [DefaultValue(GrantType.ClientCredentials)]
        public GrantType GrantType
        {
            get => _grantType.GetValueOrDefault(GrantType.ClientCredentials);
            set => _grantType = value;
        }

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

        #region Public Constructors

        /// <summary>
        /// Cria e instancia as propriedades do objeto
        /// </summary>
        /// <param name="grantType">Define o tipo de auntenticação que será realizada</param>
        /// <param name="clientOrAppId">Se o <paramref name="grantType"/> for do tipo <see cref="GrantType.ClientCredentials"/>, este valor deve ser preenchido com o ClientId ou AppId fornecido pela empresa.</param>
        /// <param name="secret">Se o <paramref name="grantType"/> for do tipo <see cref="GrantType.ClientCredentials"/>, este valor deve ser preenchido com o secret fornecido pela empresa.</param>
        /// <param name="username">Se o <paramref name="grantType"/> for do tipo <see cref="GrantType.Password"/>, este valor deve ser preenchido com o nome de usuário fornecido pela empresa.</param>
        /// <param name="password">Se o <paramref name="grantType"/> for do tipo <see cref="GrantType.Password"/>, este valor deve ser preenchido com a senha fornecida pela empresa.</param>
        /// <param name="scope">Define o escopo da autenticação. Informe o escopo definido pela empresa.</param>
        /// <remarks>
        /// A exceção <see cref="ArgumentException"/> será lançada se:<br/>
        /// O tipo <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, e as propriedades <see cref="AppId"/> ou <see cref="ClientId"/> e <see cref="Secret"/> não estiverem preenchidas.<br/>
        /// O tipo <see cref="GrantType"/> for <see cref="GrantType.Password"/>,e as propriedades <see cref="Username"/> ou <see cref="Password"/> não estiverem preenchidas.
        /// </remarks>
        /// <exception cref="ArgumentException">Se o tipo <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, as propriedades <see cref="AppId"/> ou <see cref="ClientId"/> e <see cref="Secret"/> devem ser preenchidas
        /// <para>Se o tipo <see cref="GrantType"/> for <see cref="GrantType.GrantType"/>, as propriedades <see cref="AppId"/> ou <see cref="ClientId"/> devem ser preenchidas</para>
        /// </exception>
        public AuthenticationToken(GrantType grantType,
                                   string clientOrAppId = "",
                                   string secret = "",
                                   string username = "",
                                   string password = "",
                                   string scope = "")
        {
            _grantType = grantType;
            AppId = clientOrAppId;
            Secret = secret;
            Username = username;
            Password = password;
            Scope = scope;

            Validate();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Valida este objeto e retorna erros caso as propriedades sejam inválidas.
        /// </summary>
        /// <remarks>
        /// A exceção <see cref="ArgumentException"/> será lançada se:<br/>
        /// O tipo <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, e as propriedades <see cref="AppId"/> ou <see cref="ClientId"/> e <see cref="Secret"/> não estiverem preenchidas.<br/>
        /// O tipo <see cref="GrantType"/> for <see cref="GrantType.Password"/>,e as propriedades <see cref="Username"/> ou <see cref="Password"/> não estiverem preenchidas.
        /// </remarks>
        /// <exception cref="ArgumentException">Se o tipo <see cref="GrantType"/> for <see cref="GrantType.ClientCredentials"/>, as propriedades <see cref="AppId"/> ou <see cref="ClientId"/> e <see cref="Secret"/> devem ser preenchidas
        /// <para>Se o tipo <see cref="GrantType"/> for <see cref="GrantType.GrantType"/>, as propriedades <see cref="AppId"/> ou <see cref="ClientId"/> devem ser preenchidas</para>
        /// </exception>
        public void Validate()
        {
            if(GrantType == GrantType.ClientCredentials)
            {
                if(string.IsNullOrWhiteSpace(AppId))
                {
                    throw new ArgumentException($"O valor da propriedade '{nameof(ClientId)}' ou '{nameof(AppId)}' deve ser informado quando o '{nameof(GrantType)}' for '{nameof(GrantType.ClientCredentials)}'.");
                }

                if(string.IsNullOrWhiteSpace(Secret))
                {
                    throw new ArgumentException($"O valor da propriedade '{nameof(Secret)}' deve ser informado quando o '{nameof(GrantType)}' for '{nameof(GrantType.ClientCredentials)}'.");
                }

                return;
            }

            if(GrantType == GrantType.Password)
            {
                if(string.IsNullOrWhiteSpace(Username))
                {
                    throw new ArgumentException($"O valor da propriedade '{nameof(Username)}' deve ser informado quando o '{nameof(GrantType)}' for '{nameof(GrantType.Password)}'.");
                }

                if(string.IsNullOrWhiteSpace(Password))
                {
                    throw new ArgumentException($"O valor da propriedade '{nameof(Password)}' deve ser informado quando o '{nameof(GrantType)}' for '{nameof(GrantType.Password)}'.");
                }

                return;
            }
        }

        #endregion Public Methods
    }
}