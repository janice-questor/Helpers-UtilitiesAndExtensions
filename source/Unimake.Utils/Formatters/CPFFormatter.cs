namespace Unimake.Formatters
{
    public static class CPFFormatter
    {
        #region Public Methods

        /// <summary>
        /// Formata o CPF
        /// </summary>
        /// <param name="cpf">CPF a ser formatado</param>
        /// <param name="returnOnlyNumbers">Se verdadeiro, retorna apenas os números sem formatação</param>
        /// <returns>cpf formatado</returns>
        public static string Format(string cpf, bool returnOnlyNumbers = false)
        {
            cpf = UConvert.OnlyNumbers(cpf, "-.,/").ToString().PadLeft(11, '0');

            if(returnOnlyNumbers)
            {
                return cpf;
            }

            return StringFormatter.Format(cpf, @"000\.000\.000-00");
        }

        #endregion Public Methods
    }
}