namespace Unimake.Formatters
{
    public static class CEPFormatter
    {
        #region Public Methods

        /// <summary>
        /// formata o CEP e retorna
        /// </summary>
        /// <param name="_cep">CEP a ser formatado</param>
        /// <returns>CEP formatado como 00000-000</returns>
        public static string Format(string _cep)
        {
            _cep = UConvert.OnlyNumbers(_cep, "-").ToString();
            _cep = _cep.ToString(null).PadRight(8, '0');
            return _cep.Substring(0, 5) + "-" + _cep.Substring(5, 3);
        }

        #endregion Public Methods
    }
}