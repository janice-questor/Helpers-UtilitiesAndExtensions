namespace Unimake.Validators.Enumerations
{
    /// <summary>
    /// Determina a comparação entre as datas
    /// </summary>
    public enum DateTimeComparison
    {
        /// <summary>
        /// Compara o tipo DateTime completo
        /// </summary>
        Full,

        /// <summary>
        /// Compara apenas a data
        /// </summary>
        OnlyDate,

        /// <summary>
        /// Compara apenas as horas
        /// </summary>
        OnlyTime
    }
}