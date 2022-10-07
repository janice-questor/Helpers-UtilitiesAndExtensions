using Unimake.Validators.Enumerations;

namespace System
{
    public static class RangeValidator
    {
        #region Private Methods

        private static bool EnsureRangeArguments(bool greater)
        {
            if(greater)
            {
                return true;
            }

            throw new ArgumentOutOfRangeException("Maximum value must be greater than or equal to minimum value");
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Analisa se o valor passado em <paramref name="value"/> está dentro do limite permitido.
        /// </summary>
        /// <param name="minValue">Valor mínimo que deve ser respeitado.</param>
        /// <param name="maxValue">Valor máximo que deve ser respeitado.</param>
        /// <param name="value">Valor a ser validado</param>
        /// <returns>Verdadeiro, se o valor em <paramref name="value"/> for menor que o valor máximo em <paramref name="maxValue"/> e maior que o valor mínimo em <paramref name="minValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor máximo informado em <paramref name="maxValue"/> for menor que o valor informado em <paramref name="minValue"/></exception>
        public static bool ValidateRange(this int value, int minValue, int maxValue) => EnsureRangeArguments(maxValue > minValue) ? value >= minValue && value <= maxValue : false;

        /// <summary>
        /// Analisa se o valor passado em <paramref name="value"/> está dentro do limite permitido.
        /// </summary>
        /// <param name="minValue">Valor mínimo que deve ser respeitado.</param>
        /// <param name="maxValue">Valor máximo que deve ser respeitado.</param>
        /// <param name="value">Valor a ser validado</param>
        /// <returns>Verdadeiro, se o valor em <paramref name="value"/> for menor que o valor máximo em <paramref name="maxValue"/> e maior que o valor mínimo em <paramref name="minValue"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor máximo informado em <paramref name="maxValue"/> for menor que o valor informado em <paramref name="minValue"/></exception>
        public static bool ValidateRange(this decimal value, decimal minValue, decimal maxValue) => EnsureRangeArguments(maxValue > minValue) ? value >= minValue && value <= maxValue : false;

        /// <summary>
        /// Analisa se a data passada em <paramref name="value"/> está dentro do limite permitido.
        /// </summary>
        /// <param name="minDate">Data mínima que deve ser respeitado.</param>
        /// <param name="maxDate">Data máxima que deve ser respeitada.</param>
        /// <param name="value">Data a ser validada</param>
        /// <returns>Verdadeiro, se a data passada em <paramref name="value"/>  for menor que a data máxima passada em <paramref name="maxDate"/> e menor que a data mínima passada em <paramref name="minDate"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Lançada quando o valor máximo informado em <paramref name="maxValue"/> for menor que o valor informado em <paramref name="minValue"/></exception>
        public static bool ValidateRange(this DateTime value, DateTime minDate, DateTime maxDate, DateTimeComparison comparison = DateTimeComparison.Full)
        {
            switch(comparison)
            {
                case DateTimeComparison.OnlyDate:
                    EnsureRangeArguments(maxDate.Date > minDate.Date);
                    return value.Date >= minDate.Date && value.Date <= maxDate.Date;

                case DateTimeComparison.OnlyTime:
                    EnsureRangeArguments(maxDate.TimeOfDay > minDate.TimeOfDay);
                    return value.TimeOfDay >= minDate.TimeOfDay && value.TimeOfDay <= maxDate.TimeOfDay;

                case DateTimeComparison.Full:
                default:
                    EnsureRangeArguments(maxDate > minDate);
                    return value >= minDate && value <= maxDate;
            }
        }

        #endregion Public Methods
    }
}