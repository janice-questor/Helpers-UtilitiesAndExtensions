using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Unimake.Primitives.Converters;

namespace Unimake.Primitives.CommonTypes
{
    /// <summary>
    /// tipo Renavam
    /// </summary>
    [TypeConverter(typeof(RenavamTypeConverter))]
    public class Renavam : ICloneable<Renavam>
    {
        #region Private Fields

        private readonly string value;

        #endregion Private Fields

        #region Private Methods

        private static string Format(string renavam)
        {
            renavam = Utils.OnlyNumbers(renavam);
            return Utils.Format(renavam, @"0000\.000000-0");
        }

        #endregion Private Methods

        #region Public Constructors

        /// <summary>
        /// Inicializa um renavam vazio
        /// </summary>
        public Renavam()
            : this("")
        {
        }

        /// <summary>
        /// Inicializa um renavam com o valor passado no parâmetro
        /// </summary>
        /// <param name="renavam">Número do renanvam</param>
        public Renavam(string renavam) => value = renavam;

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Retorna o número do renavam implicitamente convertido em string
        /// </summary>
        /// <param name="rhs">Objeto do tipo Renanvam que será retornado</param>
        /// <returns></returns>
        public static implicit operator long(Renavam rhs)
        {
            if(rhs == null)
            {
                return 0;
            }

            return rhs;
        }

        /// <summary>
        /// Retorna uma objeto Renavam a partir de uma string implicitamente
        /// </summary>
        /// <param name="rhs">Objeto do tipo string</param>
        /// <returns></returns>
        public static implicit operator Renavam(string rhs) => new Renavam(rhs);

        /// <summary>
        /// Retorna uma objeto Renavam a partir de um long implicitamente
        /// </summary>
        /// <param name="rhs">Objeto do tipo string</param>
        /// <returns></returns>
        public static implicit operator Renavam(long rhs) => new Renavam(rhs.ToString());

        /// <summary>
        /// Retorna o número do renavam implicitamente convertido em string
        /// </summary>
        /// <param name="rhs">Objeto do tipo Renanvam que será retornado</param>
        /// <returns></returns>
        public static implicit operator string(Renavam rhs)
        {
            if(rhs == null)
            {
                return "";
            }

            return rhs.ToString();
        }

        /// <summary>
        /// Operador de desigualdade
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Renavam lhs, Renavam rhs) => !lhs.Equals(rhs);

        /// <summary>
        /// Operador de igualdade
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Renavam lhs, Renavam rhs)
        {
            if(System.Object.ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if(((object)lhs == null) || ((object)rhs == null))
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        /// <summary>
        /// valida o Renavam
        /// </summary>
        /// <param name="renavam">número do renava</param>
        /// <param name="allowNullOrEmpty">Se verdadeiro permite o renavam como nulo</param>
        /// <returns>true se for um Renavam válido</returns>
        public static bool Validate(string renavam, bool allowNullOrEmpty)
        {
            if(string.IsNullOrWhiteSpace(renavam))
            {
                if(allowNullOrEmpty)
                {
                    return true;
                }

                throw new ArgumentException($"'{nameof(renavam)}' cannot be null or whitespace.", nameof(renavam));
            }

            var soNumero = Regex.Replace(renavam, "[^0-9]", string.Empty);
            if(string.IsNullOrEmpty(soNumero))
            {
                return allowNullOrEmpty;
            }

            var d = new int[11];
            var sequencia = "3298765432";
            if(new string(soNumero[0], soNumero.Length) == soNumero)
            {
                return false;
            }

            soNumero = Convert.ToInt64(soNumero).ToString("00000000000");

            var v = 0;

            for(var i = 0; i < 11; i++)
            {
                d[i] = Convert.ToInt32(soNumero.Substring(i, 1));
            }

            for(var i = 0; i < 10; i++)
            {
                v += d[i] * Convert.ToInt32(sequencia.Substring(i, 1));
            }

            v = (v * 10) % 11;
            v = (v != 10) ? v : 0;
            return (v == d[10]);
        }

        /// <summary>
        /// Retorna uma cópia deste objeto
        /// </summary>
        /// <returns></returns>
        public Renavam Clone() => (Renavam)MemberwiseClone();

        /// <summary>
        /// Retorna verdadeiro se este objeto for igual ao passado por parâmetro.
        /// <para>Compara apenas o valor e tipo</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ToString().Equals(obj.ToString());
        }

        /// <summary>
        /// Retorna o Hashcode deste objeto
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => value.GetHashCode();

        /// <summary>
        /// Converte para string.
        /// </summary>
        /// <returns>Retorna uma string formatada para o Renavam</returns>
        public override string ToString() => Format(value);

        /// <summary>
        /// Verifica se o número do Renanvam é válido, em caso positivo, retorna verdadeiro, ou falso
        /// </summary>
        /// <param name="allowNullOrEmpty">Se verdadeiro permite o renavam como nulo</param>
        /// <returns></returns>
        public bool Validate(bool allowNullOrEmpty = true) => Validate(this, allowNullOrEmpty);

        /// <summary>
        /// Retorna uma cópia deste objeto
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone() => MemberwiseClone();

        #endregion Public Methods
    }
}