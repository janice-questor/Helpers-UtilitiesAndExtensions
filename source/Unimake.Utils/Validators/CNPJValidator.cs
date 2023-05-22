using System.Linq;
using System.Text.RegularExpressions;

namespace Unimake.Validators
{
    public abstract class CNPJValidator
    {
        #region Private Constructors

        private CNPJValidator()
        {
        }

        #endregion Private Constructors

        #region Private Methods

        private static bool Validate(string cnpj)
        {
            var Cnpj_1 = cnpj.Substring(0, 12);
            var Cnpj_2 = cnpj.Substring(cnpj.Length - 2);
            var Mult = "543298765432";
            var Controle = string.Empty;
            var Digito = 0;

            for(var j = 1; j < 3; j++)
            {
                var Soma = 0;

                for(var i = 0; i < 12; i++)
                {
                    Soma += int.Parse(Cnpj_1.Substring(i, 1)) * int.Parse(Mult.Substring(i, 1));
                }

                if(j == 2)
                {
                    Soma += (2 * Digito);
                }

                Digito = ((Soma * 10) % 11);

                if(Digito == 10)
                {
                    Digito = 0;
                }

                Controle += Digito.ToString();
                Mult = "654329876543";
            }

            return Controle == Cnpj_2;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Analisa o CNPJ informado e verifica se é válido.
        /// <para>Se válido, atualiza o parâmetro <paramref name="cnpj"/> para o valor válido e sem formatação.</para>
        /// <para>Para formatar, pode-se usar o Unimake.Formatters.CNPJFormatter</para>
        /// <para>Este método remove qualquer caractere diferente de número para validar e devolve o CNPJ válido no parâmetro <paramref name="cnpj"/></para>
        /// </summary>
        /// <param name="cnpj">CNPJ para validação.</param>
        /// <param name="allowNullOrEmpty">Se true, permite nulo ou em branco</param>
        /// <param name="formatted">Se verdadeiro e o <paramref name="cnpj"/>  for válido, devolvo o <paramref name="cnpj"/> formatado.</param>
        /// <returns>Verdadeiro, se o CNPJ for válido ou se <paramref name="allowNullOrEmpty"/>
        /// for verdadeiro e o o valor informado em <paramref name="cpf"/> for nulo ou vazio</returns>
        public static bool Validate(ref string cnpj, bool allowNullOrEmpty = true, bool formatted = false)
        {
            if(string.IsNullOrWhiteSpace(cnpj))
            {
                if(allowNullOrEmpty)
                {
                    cnpj = "";
                    return true;
                }

                return false;
            }

            cnpj = Regex.Replace(cnpj, "[^0-9]", "").ToString();

            //se tamanho for diferente de 14, é falso
            if(cnpj.Length != 14)
            {
                return false;
            }

            var count = cnpj[0];
            //Se todos os números forem iguais, isso indica que o CNPJ é inválido
            if(cnpj.Count(w => w == count) == cnpj.Length)
            {
                return false;
            }

            if(Validate(cnpj))
            {
                if(formatted)
                {
                    cnpj = Formatters.CNPJFormatter.Format(cnpj);
                }

                return true;
            }

            return false;
        }

        #endregion Public Methods
    }
}