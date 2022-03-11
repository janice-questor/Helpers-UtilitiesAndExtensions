using System.Linq;
using System.Text.RegularExpressions;

namespace Unimake.Validators
{
    public static class PISValidator
    {
        #region Private Methods

        private static bool Validate(string pis)
        {
            var multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            soma = 0;

            for(var i = 0; i < 10; i++)
            {
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];
            }

            resto = soma % 11;

            resto = resto < 2 ? 0 : 11 - resto;

            return pis.EndsWith(resto.ToString());
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Analisa o PIS informado e verifica se é válido.
        /// <para>Se válido, atualiza o parâmetro <paramref name="pis"/> para o valor válido e sem formatação.</para>
        /// <para>Para formatar, pode-se usar o Unimake.Formatters.PISFormatter</para>
        /// <para>Este método remove qualquer caractere diferente de número para validar e devolve o PIS válido no parâmetro <paramref name="pis"/></para>
        /// </summary>
        /// <param name="pis">PIS para validação.</param>
        /// <param name="allowNullOrEmpty">Se true, permite nulo ou em branco</param>
        /// <param name="formatted">Se verdadeiro e o <paramref name="pis"/>  for válido, devolvo o <paramref name="pis"/> formatado.</param>
        /// <returns>Verdadeiro, se o PIS for válido ou se <paramref name="allowNullOrEmpty"/>
        /// for verdadeiro e o o valor informado em <paramref name="pis"/> for nulo ou vazio</returns>
        public static bool Validate(ref string pis, bool allowNullOrEmpty = true, bool formatted = false)
        {
            if(string.IsNullOrWhiteSpace(pis))
            {
                if(allowNullOrEmpty)
                {
                    pis = "";
                    return true;
                }

                return false;
            }

            pis = Regex.Replace(pis, "[^0-9]", "").ToString();

            //se tamanho for diferente de 11, é falso
            if(pis.Length != 11)
            {
                return false;
            }

            var count = pis[0];
            //Se todos os números forem iguais, isso indica que o PIS é inválido
            if(pis.Count(w => w == count) == pis.Length)
            {
                return false;
            }

            if(Validate(pis))
            {
                if(formatted)
                {
                    pis = Formatters.PISFormatter.Format(pis);
                }

                return true;
            }

            return false;
        }

        #endregion Public Methods
    }
}