using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SystemUnimake
{
    /// <summary>
    /// Contém extensões úteis para trabalhar com tipos de enumeração.
    /// </summary>
    public static class EnumExtensions
    {
        #region Private Methods

        /// <summary>
        /// Verifica se o número fornecido é uma potência de dois.
        /// </summary>
        /// <param name="x">Número a ser verificado</param>
        /// <param name="ignoreZeroFlagged">Determina se o valor zero deve ser ignorado (opcional, padrão: true)</param>
        /// <returns>Retorna <c>true</c> se o número for uma potência de dois, caso contrário, <c>false</c>.</returns>
        private static bool IsPowerOfTwo(int x, bool ignoreZeroFlagged = true)
        {
            if(ignoreZeroFlagged)
            {
                return x != 0 && ((x & (x - 1)) == 0);
            }

            return (x & (x - 1)) == 0;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Obtém a descrição de um valor de enumeração com base no atributo <see cref="DescriptionAttribute"/>.
        /// Caso não haja descrição, retorna o nome do valor do enum.
        /// </summary>
        /// <param name="value">Valor do enum para o qual a descrição será retornada</param>
        /// <returns>A descrição associada ao valor do enum ou o nome do valor caso a descrição não exista</returns>
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if(name != null)
            {
                var field = type.GetField(name);

                if(field != null)
                {
                    if(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Obtém todos os valores de flags definidos em um enum com base no valor fornecido.
        /// </summary>
        /// <param name="value">Valor do enum para verificar quais flags estão ativas</param>
        /// <param name="ignoreZeroFlagged">Se <c>true</c>, valores flag com zero serão ignorados</param>
        /// <typeparam name="T">Tipo de enum</typeparam>
        /// <returns>Lista de valores de flags ativas no enum</returns>
        public static T[] GetFlags<T>(T value, bool ignoreZeroFlagged = true)
            where T : Enum
        {
            var result = new List<T>();

            foreach(var eValue in Enum.GetValues(typeof(T)))
            {
                var eValueAsInt = (int)Convert.ChangeType(eValue, typeof(int));
                var enumValue = (T)Enum.Parse(typeof(T), eValue.ToString());

                if(value.HasFlag(enumValue) && IsPowerOfTwo(eValueAsInt, ignoreZeroFlagged))
                {
                    result.Add(enumValue);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Verifica se o valor de um enum tem a flag correspondente ativada.
        /// Só funciona se o enum estiver marcado com o atributo <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="enumValue">Valor do enum a ser verificado</param>
        /// <typeparam name="T">Tipo de enum</typeparam>
        /// <returns>Retorna <c>true</c> se o valor do enum tiver flag ativa, caso contrário, <c>false</c></returns>
        public static bool HasFlag<T>(this T enumValue) where T : Enum
        {
            var type = typeof(T);

            // Verifica se o enum possui o atributo [Flags], caso contrário, não pode ser tratado como flag
            if(!Attribute.IsDefined(type, typeof(FlagsAttribute)))
            {
                return false;
            }

            var value = Convert.ToInt64(enumValue);
            return (value & (value - 1)) != 0;
        }

        /// <summary>
        /// Verifica se o valor fornecido é um valor válido do enum.
        /// </summary>
        /// <param name="value">Valor a ser verificado</param>
        /// <returns>Retorna <c>true</c> se o valor for um valor válido do enum, caso contrário, <c>false</c></returns>
        public static bool IsValid(this Enum value)
        {
            return value != null && Enum.IsDefined(value.GetType(), value);
        }

        /// <summary>
        /// Retorna uma string contendo todos os valores de um enum como inteiros, separados por um caractere delimitador.
        /// </summary>
        /// <param name="value">Valor único do enum</param>
        /// <param name="separator">Caractere separador, por padrão é a vírgula (<c>,</c>)</param>
        /// <param name="ignoreZeroFlagged">Determina se valores com zero flag devem ser ignorados (opcional)</param>
        /// <typeparam name="T">Tipo do enum</typeparam>
        /// <returns>Uma string com os valores inteiros do enum separados pelo caractere especificado</returns>
        /// <exception cref="ArgumentException">Lançada caso o tipo de enum não seja válido</exception>
        /// <exception cref="ArgumentNullException">Lançada caso o separador seja nulo ou vazio</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lançada caso o separador tenha mais de um caractere</exception>
        public static string JoinAsInteger<T>(this T value, bool ignoreZeroFlagged = true, char separator = ',') where T : Enum
            => JoinAsInteger(new T[] { value }, ignoreZeroFlagged, separator);

        /// <summary>
        /// Retorna uma string contendo todos os valores de um conjunto de valores de enum como inteiros, separados por um caractere delimitador.
        /// </summary>
        /// <param name="values">Coleção de valores do enum</param>
        /// <param name="separator">Caractere separador, por padrão é a vírgula (<c>,</c>)</param>
        /// <param name="ignoreZeroFlagged">Determina se valores com zero flag devem ser ignorados (opcional)</param>
        /// <typeparam name="T">Tipo do enum</typeparam>
        /// <returns>Uma string com os valores inteiros do enum separados pelo caractere especificado</returns>
        /// <exception cref="ArgumentException">Lançada caso o tipo de enum não seja válido</exception>
        /// <exception cref="ArgumentNullException">Lançada caso o separador seja nulo ou vazio</exception>
        public static string JoinAsInteger<T>(this IEnumerable<T> values, bool ignoreZeroFlagged = true, char separator = ',')
            where T : Enum
        {
            if(string.IsNullOrEmpty(separator.ToString()))
            {
                throw new ArgumentNullException(nameof(separator), "O separador não pode ser nulo ou vazio.");
            }

            var result = new List<string>();

            foreach(var value in values)
            {
                if(!value.GetType().IsEnum)
                {
                    throw new ArgumentException("O tipo fornecido não é um enum válido.", nameof(value));
                }

                result.AddRange(value.HasFlag()
                                    ? GetFlags<T>(value, ignoreZeroFlagged).Select(flag => Convert.ToInt64(flag).ToString())
                                    : new string[] { Convert.ToInt64(value).ToString() });
            }

            return string.Join(separator.ToString(), result);
        }

        #endregion Public Methods
    }
}