using System;
using System.Text;

namespace SystemUnimake
{
    /// <summary>
    /// Extensões de métodos para as exceções
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Public Methods

        public static string GetAllMessages(this Exception exception)
        {
            if(exception is null)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            while(exception != null)
            {
                sb.Append(exception.Message);
                exception = exception.InnerException;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gera um código com base no tipo da exceção com um HashCode estável.
        /// Isso garante que será sempre o mesmo código
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="considerMessage">Se verdadeiro, considera a mensagem na geração do código de erro</param>
        /// <returns></returns>
        public static int GetErrorCode(this Exception exception, bool considerMessage = false)
        {
            if(exception is null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return $"{exception.GetType().FullName}{(considerMessage ? exception.Message : "")}".GetStableHashCode();
        }

        /// <summary>
        /// Retorna a última exceção da cadeia de exceções
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetLastException(this Exception ex)
        {
            var result = ex;

            if(ex.InnerException != null)
            {
                return GetLastException(ex.InnerException);
            }

            return result;
        }

        #endregion Public Methods
    }
}