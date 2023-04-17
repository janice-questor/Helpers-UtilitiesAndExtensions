using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unimake.Primitives
{
    public static class ExceptionHandler
    {
        #region Private Properties

        /// <summary>
        /// <see cref="ILogger"/> utilizado para logar os erros nas chamadas do <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        private static ILogger Logger { get; set; }

        #endregion Private Properties

        #region Private Methods

        private static void LogException(string exception, bool useLogger, string additionalInfo, string category)
        {
            try
            {
                exception += string.IsNullOrWhiteSpace(additionalInfo) ? "" : $"{Environment.NewLine}Additional Info: {additionalInfo}";

                if(useLogger)
                {
                    Logger?.LogError(exception, category);
                }

                Trace.WriteLine(exception, category);

                if(Trace.Listeners?.Count > 0)
                {
                    foreach(TraceListener trace in Trace.Listeners)
                    {
                        trace.WriteLine(exception, category);
                    }
                }
            }
            catch
            {
                //¯\(°_o)/¯
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Faz o log da exceção.
        /// </summary>
        /// <remarks>
        /// O log é sempre exibido em <see cref="Trace.WriteLine(object, string)"/><br/>
        /// Se informado um <see cref="ILogger"/> em <see cref="RegisterLogger(ILogger)"/>
        /// será chamado o método <see cref="ILogger.Log{TState}(LogLevel, EventId, TState, Exception?, Func{TState, Exception?, string})/>
        /// </remarks>
        /// <param name="exception">Exceção lançada</param>
        /// <param name="additionalInfo">Informações adicionais para a exceção</param>
        /// <param name="category">Categoria da exceção</param>
        public static void LogException(this Exception exception, string additionalInfo = "", [CallerMemberName] string category = "") =>
            LogException($"{exception.Message}{Environment.NewLine}StackTrace: {exception.StackTrace}", additionalInfo, category);

        /// <summary>
        /// Faz o log da exceção.
        /// </summary>
        /// <remarks>
        /// O log é sempre exibido em <see cref="Trace.WriteLine(object, string)"/><br/>
        /// Se informado um <see cref="ILogger"/> em <see cref="RegisterLogger(ILogger)"/>
        /// será chamado o método <see cref="ILogger.Log{TState}(LogLevel, EventId, TState, Exception?, Func{TState, Exception?, string})/>
        /// </remarks>
        /// <param name="exception">Exceção lançada</param>
        /// <param name="additionalInfo">Informações adicionais para a exceção</param>
        /// <param name="category">Categoria da exceção</param>
        public static void LogException(string exception, string additionalInfo = "", [CallerMemberName] string category = "") =>
            LogException(exception, true, additionalInfo, category);

        /// <summary>
        /// Registra o <see cref="ILogger"/> para logar as exceções.
        /// <para>Se passar nulo, será ignorado o <see cref="ILogger"/></para>
        /// </summary>
        /// <param name="logger"></param>
        public static void RegisterLogger(ILogger logger) => Logger = logger;

        /// <summary>
        /// Executa a função definida em <paramref name="func"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <typeparam name="TResult">Tipo de objeto que será retornado pelo <paramref name="func"/></typeparam>
        /// <param name="func">Função que será executada nesta chamada</param>
        /// <returns><typeparamref name="TResult"/> Se informada, é chamada após a execução da função <paramref name="func"/>. Ou <see cref="default(TResult)"/> em caso de erro. </returns>
        public static TResult Run<TResult>(Func<TResult> func) => Run(func, null, null);

        /// <summary>
        /// Executa a função definida em <paramref name="func"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <typeparam name="TResult">Tipo de objeto que será retornado pelo <paramref name="func"/></typeparam>
        /// <param name="func">Função que será executada nesta chamada</param>
        /// <param name="catchAction">Se informada, é chamada passando o objeto da exceção</param>
        /// <returns><typeparamref name="TResult"/> Se informada, é chamada após a execução da função <paramref name="func"/>. Ou <see cref="default(TResult)"/> em caso de erro. </returns>

        public static TResult Run<TResult>(Func<TResult> func, Action<Exception> catchAction) => Run(func, catchAction, null);

        /// <summary>
        /// Executa a função definida em <paramref name="func"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <typeparam name="TResult">Tipo de objeto que será retornado pelo <paramref name="func"/></typeparam>
        /// <param name="func">Função que será executada nesta chamada</param>
        /// <param name="catchAction">Se informada, é chamada passando o objeto da exceção</param>
        /// <param name="finallyAction">Se informada, é chamada após a execução da função <paramref name="func"/>. Ou após a função <paramref name="catchAction"/></param>
        /// <returns><typeparamref name="TResult"/> Se informada, é chamada após a execução da função <paramref name="func"/>. Ou <see cref="default(TResult)"/> em caso de erro. </returns>
        public static TResult Run<TResult>(Func<TResult> func, Action<Exception> catchAction, Action finallyAction)
        {
            if(func == null)
            {
                return default;
            }

            try
            {
                return func.Invoke();
            }
            catch(Exception ex)
            {
                ex.TrapException();
                catchAction?.Invoke(ex);

                return default;
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// Executa a ação definida em <paramref name="action"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <param name="action">Ação que será executada nesta chamada</param>
        /// <param name="catchAction">Se informada, é chamada passando o objeto da exceção</param>
        public static void Run(Action action, Action<Exception> catchAction) => Run(action, catchAction, null);

        /// <summary>
        /// Executa a ação definida em <paramref name="action"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <param name="action">Ação que será executada nesta chamada</param>
        /// <param name="catchAction">Se informada, é chamada passando o objeto da exceção</param>
        /// <param name="finallyAction">Se informada, é chamada após a execução da ação <paramref name="action"/>. Ou após a função <paramref name="catchAction"/></param>
        public static void Run(Action action, Action<Exception> catchAction, Action finallyAction)
        {
            if(action == null)
            {
                return;
            }

            try
            {
                action.Invoke();
            }
            catch(Exception ex)
            {
                ex.TrapException();
                catchAction?.Invoke(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// Executa a ação definida em <paramref name="action"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <param name="action">Ação que será executada nesta chamada</param>
        /// <param name="catchAction">Se informada, é chamada passando o objeto da exceção</param>
        public static void Run(Action action, Action catchAction) => Run(action, catchAction, null);

        /// <summary>
        /// Executa a ação definida em <paramref name="action"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <param name="action">Ação que será executada nesta chamada</param>
        /// <param name="catchAction">Se informada, é chamada passando o objeto da exceção</param>
        /// <param name="finallyAction">Se informada, é chamada após a execução da ação <paramref name="action"/>. Ou após a função <paramref name="catchAction"/></param>
        public static void Run(Action action, Action catchAction, Action finallyAction)
        {
            if(action == null)
            {
                return;
            }

            try
            {
                action.Invoke();
            }
            catch(Exception ex)
            {
                ex.TrapException();
                catchAction?.Invoke();
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        /// <summary>
        /// Executa a ação definida em <paramref name="action"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <param name="action">Ação que será executada nesta chamada</param>
        public static void Run(Action action) => Run(action, (Action)null);

        /// <summary>
        /// Executa a ação definida em <paramref name="action"/> e não retorna erro, mas chama o método <see cref="TrapException(Exception, string, string)"/>
        /// </summary>
        /// <param name="action">Ação que será executada nesta chamada</param>
        public static void RunAndForget(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch(Exception ex)
            {
                LogException($"{ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}", false, "", nameof(RunAndForget));
            }
        }

        /// <summary>
        /// Captura a exceção
        /// </summary>
        /// <param name="exception">Exceção lançada</param>
        /// <param name="additionalInfo">Informações adicionais que serão concatenadas junto a mensagem da exceção</param>
        /// <param name="category">Categoria da exceção. Se nada informado, assume o nome do método que fez a chamada.</param>
        public static void TrapException(this Exception exception, string additionalInfo = "", [CallerMemberName] string category = "") =>
                LogException(exception, additionalInfo, category);

        #endregion Public Methods
    }
}