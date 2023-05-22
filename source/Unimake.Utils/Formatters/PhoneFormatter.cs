namespace Unimake.Formatters
{
    /// <summary>
    /// Faz a formatação de números de telefone.
    /// </summary>
    public abstract class PhoneFormatter
    {
        #region Private Constructors

        private PhoneFormatter()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Retorna um número de telefone formatado.
        /// </summary>
        /// <remarks>
        /// Remove todo carácter especial ou não numérico informado.
        /// </remarks>
        /// <param name="phone">Número de telefone que será formatado</param>
        /// <returns></returns>
        public static string Format(string phone)
        {
            if(string.IsNullOrWhiteSpace(phone))
            {
                return "";
            }

            var value = UConvert.OnlyNumbers(phone.Trim(), "-.").ToString();

            string mascara;
            if(value.StartsWith("0800") ||
               value.StartsWith("0300") ||
               value.StartsWith("0500") ||
               value.StartsWith("0900"))
            {
                mascara = @"0000\-000\-0000";
            }
            else if(phone.StartsWith("+"))
            {
                mascara = @"+00\(00\)00000\-0000";
            }
            else if(value.Length == 7)
            {
                mascara = @"000\-0000";
            }
            else if(value.Length == 8)
            {
                mascara = @"0000\-0000";
            }
            else if(value.Length == 10)
            {
                mascara = @"\(00\)0000\-0000";
            }
            else if(value.Length == 11)
            {
                mascara = @"\(000\)0000\-0000";
            }
            else if(value.Length == 12)
            {
                mascara = @"00\(00\)0000\-0000";
            }
            else if(value.Length == 13)
            {
                mascara = @"00\(00\)00000\-0000";
            }
            else
            {
                mascara = "0".PadLeft(value.Length, '0');
            }

            return StringFormatter.Format(value, mascara);
        }

        #endregion Public Methods
    }
}