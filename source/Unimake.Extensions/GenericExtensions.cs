using System.Linq;

namespace System
{
    /// <summary>
    /// </summary>
    public static class GenericExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retorna verdadeiro se o valor informado está contido na lista de valores
        /// </summary>
        /// <typeparam name="T">Tipo de objeto que deverá ser verificado</typeparam>
        /// <param name="value">Valor que pode ou não estar contido na lista de valores.</param>
        /// <param name="values">Lista de valores para verificação</param>
        /// <returns></returns>
        public static bool In<T>(this T value, params T[] values) where T : struct => values.Contains(value);

        /// <summary>
        /// Retorna verdadeiro se o valor informado está contido na lista de valores
        /// </summary>
        /// <typeparam name="T">Tipo de objeto que deverá ser verificado</typeparam>
        /// <param name="value">Valor que pode ou não estar contido na lista de valores.</param>
        /// <param name="values">Lista de valores para verificação</param>
        /// <returns></returns>
        public static bool In<T>(this T? value, params T[] values) where T : struct => value == null ? false : values.Contains(value.GetValueOrDefault());

        #endregion Public Methods
    }
}