using System;

namespace Unimake.Validators
{
    public static class ValidatorRange
    {
        #region Public Methods

        /// <summary>
        /// Analisa se o valor passado está dentro do limite permitido.
        /// </summary>
        /// <param name="valorMinimo">Valor mínimo que deve ser respeitado.</param>
        /// <param name="valorMaximo">Valor máximo que deve ser respeitado.</param>
        /// <param name="valor">Valor a ser validado</param>
        /// <returns>Verdadeiro, se o valor for menor que o valorMaximo e maior que o valorMinimo.</returns>
        public static bool ValidateNumeric(int valor, int valorMinimo, int valorMaximo)
        {
            if (valor > valorMaximo || valor < valorMinimo)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Analisa se o valor passado está dentro do limite permitido.
        /// </summary>
        /// <param name="valorMinimo">Valor mínimo que deve ser respeitado.</param>
        /// <param name="valorMaximo">Valor máximo que deve ser respeitado.</param>
        /// <param name="valor">Valor a ser validado</param>
        /// <returns>Verdadeiro, se o valor for menor que o valorMaximo e maior que o valorMinimo.</returns>
        public static bool ValidateNumericDecimal(decimal valor, decimal valorMinimo, decimal valorMaximo)
        {
            if (valor > valorMaximo || valor < valorMinimo)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Analisa se o valor passado está dentro do limite permitido.
        /// </summary>
        /// <param name="dataMinima">Data mínima que deve ser respeitado.</param>
        /// <param name="dataMaxima">Data máxima que deve ser respeitada.</param>
        /// <param name="dataValor">Data a ser validada</param>
        /// <returns>Verdadeiro, se dataValor for menor que a dataMaxima e maior que a dataMinima.</returns>
        public static bool ValidateDateTime(DateTime dataValor, DateTime dataMinima, DateTime dataMaxima)
        {
            if (dataValor > dataMaxima || dataValor < dataMinima)
            {
                return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}