namespace Unimake.Formatters
{
    public static class PISFormatter
    {
        #region Public Methods

        /// <summary>
        /// Formata o PIS no padrão ###.#####.##-#
        /// </summary>
        /// <param name="pis">PIS a ser formatado</param>
        /// <param name="returnOnlyNumbers">Se verdadeiro, retorna apenas os números sem formatação</param>
        /// <returns>PIS formatado</returns>
        public static string Format(string pis, bool returnOnlyNumbers = false)
        {
            pis = UConvert.OnlyNumbers(pis, "-.,/").ToString().PadLeft(11, '0');

            if(returnOnlyNumbers)
            {
                return pis;
            }

            return StringFormatter.Format(pis, @"000\.00000.00-0");
        }

        #endregion Public Methods
    }
}