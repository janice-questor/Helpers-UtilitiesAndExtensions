namespace Unimake.Formatters
{
    public abstract class CEPFormatter
    {
        #region Private Constructors

        private CEPFormatter()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// formata o CEP e retorna
        /// </summary>
        /// <param name="cep">CEP a ser formatado</param>
        /// <returns>CEP formatado como 00000-000</returns>
        public static string Format(string cep)
        {
            cep = UConvert.OnlyNumbers(cep, "-").ToString();
            cep = cep.ToString(null).PadRight(8, '0');
            return cep.Substring(0, 5) + "-" + cep.Substring(5, 3);
        }

        #endregion Public Methods
    }
}