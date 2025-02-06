using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SystemUnimake.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        #region Public Methods

        public static IEnumerable<TObject> DistinctBy<TObject>(this IEnumerable<TObject> list, Func<TObject, object> propertySelector) => list.GroupBy(propertySelector).Select(matches => matches.First());

        /// <summary>
        /// Retorna uma lista de itens duplicados que foram encontrados na lista
        /// </summary>
        /// <typeparam name="T">Tipo de item que será percorrido</typeparam>
        /// <param name="values">Valores que serão percorridos</param>
        /// <param name="condition">Condição para saber se o item é duplicado</param>
        /// <returns>Lista com os itens duplicados</returns>
        public static IList<T> FindDuplicates<T>(this IEnumerable<T> values, Func<T, object> condition)
        {
            if(values.IsNullOrEmpty())
            {
                return new List<T>();
            }

            IList<T> result = values
                                  .GroupBy(i => condition(i))
                                  .SelectMany(g => g.Skip(1))
                                  .ToList<T>();
            return result;
        }

        public static void ForEach<T>(this T[] values, Action<T> action)
        {
            if(values == null)
            {
                return;
            }

            foreach(var item in values)
            {
                action?.Invoke(item);
            }
        }

        public static bool IsNullOrEmpty<TObject>(this IEnumerable<TObject> enumerable) => enumerable == null || !enumerable.Any();

        /// <summary>
        /// Verifica se o array é vazio
        /// </summary>
        /// <param name="arr">Array ;</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] arr) => arr == null || arr.Length == 0;

        /// <summary>
        /// Verifica se a lista está vazia.
        /// </summary>
        /// <param name="list">Lista informada;</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> list) => list == null || list.Count == 0;

        /// <summary>
        /// Verifica se a lista está vazia.
        /// </summary>
        /// <param name="values">Lista informada;</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable values) => values == null || !values.Cast<object>().Any();

        /// <summary>
        /// Concatena os valores e utiliza a vírgula como separador.
        /// </summary>
        /// <param name="values">Valores a serem concatenados.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable values) => Join(values, ',');

        /// <summary>
        /// Concatena os valores utilizando separador.
        /// </summary>
        /// <param name="values">Valores a serem concatenados.</param>
        /// <param name="separator">Separador.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable values, char separator) => Join(values, separator.ToString());

        /// <summary>
        /// Concatena os valores utilizando separador.
        /// </summary>
        /// <param name="values">Valores a serem concatenados.</param>
        /// <param name="separator">Separador.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable values, string separator)
        {
            if(values == null)
            {
                return "";
            }

            var result = "";

            foreach(var item in values)
            {
                result += string.Format("{0}{1}", item, separator);
            }

            if(result.Length > 0)
            {
                result = result.Substring(0, result.Length - separator.Length);
            }

            return result;
        }

        /// <summary>
        /// Remove os itens da lista onde o predicado (predicate) retorna verdadeiro
        /// </summary>
        /// <typeparam name="T">Tipo de elemento da listagem</typeparam>
        /// <param name="list">Listagem que contem os itens a serem removidos</param>
        /// <param name="predicate">Se retornar verdadeiro,o item é removido da lista</param>
        public static void RemoveAll<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if(list.IsNullOrEmpty())
            {
                return;
            }

            var index = 0;
            while(index < list.Count)
            {
                if(predicate(list[index]))
                {
                    list.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }

        public static IEnumerable<(TObject item, int index)> WithIndex<TObject>(this IEnumerable<TObject> enumerable) => enumerable.Select((item, index) => (item, index));

        #endregion Public Methods
    }
}