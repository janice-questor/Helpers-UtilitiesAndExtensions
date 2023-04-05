using System;
using System.Collections.Generic;
using System.Text;

namespace Unimake.Primitives.UDebug
{
    /// <summary>
    /// Objeto de estado de debug
    /// </summary>
    public sealed class DebugStateObject
    {
        #region Public Properties

        /// <summary>
        /// Endereço do serviço de autenticação para homologação e depuração
        /// </summary>
        public string AuthServerUrl { get; set; }

        /// <summary>
        /// Endereço de outro servidor que será utilizado para depuração
        /// </summary>
        public string AnotherServerUrl { get; set; }

        /// <summary>
        /// Qualquer objeto de estado em debug
        /// </summary>
        public object State { get; set; }

        #endregion Public Properties
    }
}