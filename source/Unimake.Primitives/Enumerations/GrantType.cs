using System.ComponentModel;

namespace Unimake.Primitives.Enumerations
{
    /// <summary>
    /// Define o tipo de concessão que deverá ser realizada
    /// </summary>
    [DefaultValue(GrantType.ClientCredentials)]
    public enum GrantType : short
    {
        /// <summary>
        /// Concessão por usuário e senha
        /// </summary>
        Password = 0,

        /// <summary>
        /// Concessão por ClientId ou AppId ou Secret
        /// </summary>
        ClientCredentials = 1
    }
}