namespace Unimake.Formatters
{
    public abstract class CNPJFormatter
    {
        #region Private Constructors

        private CNPJFormatter()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Formata o CNPJ
        /// </summary>
        /// <param name="cnpj">cnpj a ser formatado</param>
        /// <param name="returnOnlyNumbers">Se verdadeiro, retorna apenas os números sem formatação</param>
        /// <returns>cnpj formatado</returns>
        public static string Format(string cnpj, bool returnOnlyNumbers = false)
        {
            cnpj = UConvert.OnlyNumbers(cnpj, "-.,/").ToString().PadLeft(14, '0');

            if(returnOnlyNumbers)
            {
                return cnpj;
            }

            return StringFormatter.Format(cnpj, @"00\.000\.000/0000-00");
        }

        #endregion Public Methods
    }
}