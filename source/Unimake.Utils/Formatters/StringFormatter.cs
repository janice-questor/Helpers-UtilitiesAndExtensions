using System;
using System.ComponentModel;

namespace Unimake.Formatters
{
    public class StringFormatter
    {
        #region Public Methods

        /// <summary>
        /// Formata o objeto passado de acordo com a máscara informada
        /// </summary>
        /// <param name="value2Format">Objeto a ser formatado</param>
        /// <param name="mask">Máscara informada</param>
        /// <returns></returns>
        public static string Format(object value2Format, string mask) => Format(value2Format, mask, out _);

        /// <summary>
        /// Formata o objeto passado de acordo com a máscara informada
        /// </summary>
        /// <param name="value2Format">Objeto a ser formatado</param>
        /// <param name="mask">Máscara informada</param>
        /// <param name="wasFormatted">Se true, foi formatada</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Se mask for nulo, vazio ou espaços em branco</exception>
        public static string Format(object value2Format, string mask, out bool wasFormatted)
        {
            if(string.IsNullOrWhiteSpace(mask))
            {
                throw new ArgumentException($"'{nameof(mask)}' cannot be null or whitespace.", nameof(mask));
            }

            wasFormatted = false;

            if(value2Format == null)
            {
                return "";
            }

            var mascara = new MaskedTextProvider(mask);
            var v2Format = value2Format.ToString();

            mascara.Set(v2Format, out _, out var resultHint);

            if(resultHint == MaskedTextResultHint.UnavailableEditPosition)
            {
                return value2Format.ToString();
            }

            if(resultHint == MaskedTextResultHint.Success)
            {
                wasFormatted = true;
                return mascara.ToDisplayString();
            }

            return "";
        }

        #endregion Public Methods
    }
}