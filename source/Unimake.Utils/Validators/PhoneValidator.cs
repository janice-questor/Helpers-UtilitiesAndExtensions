using System;

namespace Unimake.Validators
{
    public abstract class PhoneValidator
    {
        #region Private Constructors

        private PhoneValidator()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Analisa o telefone informado e verifica se é válido.
        /// </summary>
        /// <param name="phoneNumber">Telefone informado</param>
        /// <param name="allowEmpty">Se verdadeiro, permite que seja informado um valor vazio</param>
        /// <param name="minLength">Tamanho mínimo do número de telefone, o padrão é 8 números</param>
        /// <returns>Verdadeiro se satisfaz as condições de validação. Caso contrário, falso.</returns>
        public static bool Validate(string phoneNumber, bool allowEmpty = true, short minLength = 8)
        {
            phoneNumber = UConvert.OnlyNumbers(phoneNumber, ".-", false).ToString();

            if(string.IsNullOrWhiteSpace(phoneNumber))
            {
                return allowEmpty;
            }

            //vamos assumir que o número de telefone deverá ter no mínimo (minLength) números
            if(phoneNumber.Length < minLength)
            {
                return false;
            }

            return true;
        }

        #endregion Public Methods
    }
}