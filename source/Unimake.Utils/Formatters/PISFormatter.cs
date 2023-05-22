namespace Unimake.Formatters
{
    public abstract class PISFormatter
    {
        #region Private Constructors

        private PISFormatter()
        {
        }

        #endregion Private Constructors

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