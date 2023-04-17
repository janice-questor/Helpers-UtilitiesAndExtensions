using System;

namespace Unimake.Cryptography.Exceptions
{
    /// <summary>
    /// Lançada quando o token expirar
    /// </summary>
    public sealed class SecurityTokenExpiredException : Exception
    {
        #region Public Constructors

        /// <summary>
        /// Nova instância da exceção
        /// </summary>
        public SecurityTokenExpiredException()
        {
        }

        /// <summary>
        /// Nova instancia da exceção
        /// </summary>
        /// <param name="message">Mensagem de erro</param>
        public SecurityTokenExpiredException(string message)
            : base(message)
        {
        }

        #endregion Public Constructors
    }
}