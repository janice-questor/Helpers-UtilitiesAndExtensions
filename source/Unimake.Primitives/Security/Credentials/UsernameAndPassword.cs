namespace Unimake.Primitives.Security.Credentials
{
    /// <summary>
    /// Nome de usuário e senha para acesso ao aplicativo
    /// </summary>
    public struct UsernameAndPassword
    {
        #region Public Properties

        /// <summary>
        /// Senha cadastrada pelo usuário no momento da criação da conta no ERP.net
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Nome de usuário, e-mail, informado no momento da criação da conta no ERP.net
        /// </summary>
        public string Username { get; set; }

        #endregion Public Properties
    }
}