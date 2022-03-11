using System.Linq;
using System.Text.RegularExpressions;

namespace Unimake.Validators
{
    public static class CPFValidator
    {
        #region Private Methods

        private static bool Validate(string cpf)
        {
            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            if(cpf.Length != 11)
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for(var i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for(var i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Analisa o CPF informado e verifica se é válido.
        /// <para>Se válido, atualiza o parâmetro <paramref name="cpf"/> para o valor válido e sem formatação.</para>
        /// <para>Para formatar, pode-se usar o Unimake.Formatters.CPFFormatter</para>
        /// <para>Este método remove qualquer caractere diferente de número para validar e devolve o CPF válido no parâmetro <paramref name="cpf"/></para>
        /// </summary>
        /// <param name="cpf">CPF para validação.</param>
        /// <param name="allowNullOrEmpty">Se true, permite nulo ou em branco</param>
        /// <param name="formatted">Se verdadeiro e o <paramref name="cpf"/>  for válido, devolvo o <paramref name="cpf"/> formatado.</param>
        /// <returns>Verdadeiro, se o CPF for válido ou se <paramref name="allowNullOrEmpty"/>
        /// for verdadeiro e o o valor informado em <paramref name="cpf"/> for nulo ou vazio</returns>
        public static bool Validate(ref string cpf, bool allowNullOrEmpty = true, bool formatted = false)
        {
            if(string.IsNullOrWhiteSpace(cpf))
            {
                if(allowNullOrEmpty)
                {
                    cpf = "";
                    return true;
                }

                return false;
            }

            cpf = Regex.Replace(cpf, "[^0-9]", "").ToString();

            //se tamanho for diferente de 11, é falso
            if(cpf.Length != 11)
            {
                return false;
            }

            var count = cpf[0];
            //Se todos os números forem iguais, isso indica que o CPF é inválido
            if(cpf.Count(w => w == count) == cpf.Length)
            {
                return false;
            }

            if(Validate(cpf))
            {
                if(formatted)
                {
                    cpf = Formatters.CPFFormatter.Format(cpf);
                }

                return true;
            }

            return false;
        }

        #endregion Public Methods
    }
}