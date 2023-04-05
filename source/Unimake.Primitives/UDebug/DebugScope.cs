using System;

namespace Unimake.Primitives.UDebug
{
    /// <summary>
    /// Escopo para definição de configurações e dados entre classes, entidades e diferentes DLLs e serviços.
    /// <br/>Esta classe não é ThreadSafe, a última instância criada será sempre a válida.
    /// </summary>
    /// <typeparam name="TObject">Objeto que será trafegado entre as DLLs, serviços, classes. Mantem o estado do objeto</typeparam>
    public sealed class DebugScope<TObjectState> : IDisposable
    {
        #region Private Destructors

        ~DebugScope()
        {
            Dispose();
        }

        #endregion Private Destructors

        #region Public Properties

        public static DebugScope<TObjectState> Instance { get; private set; }
        public bool Disposed { get; private set; }
        public TObjectState ObjectState { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Instancia um novo escopo
        /// </summary>
        /// <param name="objectState"></param>
        /// <param name="singleInstance"></param>
        public DebugScope(TObjectState objectState, bool singleInstance = true)
        {
            ObjectState = objectState;
            Instance = this;
        }

        #endregion Public Constructors

        #region Public Methods

        public static bool IsDefined() => Instance != null;

        public void Dispose()
        {
            if(Disposed)
            {
                return;
            }

            Instance = null;
            Disposed = true;

            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}