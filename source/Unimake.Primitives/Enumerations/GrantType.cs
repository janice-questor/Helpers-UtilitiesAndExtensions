namespace Unimake.Primitives.Enumerations
{
    /// <summary>
    /// Define o tipo de concessão que deverá ser realizada
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// Concessão por usuário e senha
        /// </summary>
        Password,

        /// <summary>
        /// Concessão por ClientId e Secret
        /// </summary>
        ClientCredentials
    }
}