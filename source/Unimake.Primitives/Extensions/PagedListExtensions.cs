using System.Linq;
using Unimake.Primitives.Collections.Page;

namespace System.Collections.Generic
{
    public static class PagedListExtensions
    {
        #region Public Methods

        /// <summary>
        /// Converte uma consulta em uma lista paginada
        /// </summary>
        /// <typeparam name="T">Tipo de entidade</typeparam>
        /// <param name="items">Itens já paginados que serão exibidos</param>
        ///<param name="totalCount">Contagem de registros antes da aplicar a paginação</param>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageNumber">Número de página atual</param>
        /// <param name="pageSize">Tamanho de registros por página</param>
        /// <returns>Lista paginada</returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> items, int totalCount, long recordCount, int pageNumber, int pageSize) =>
            new PagedList<T>(items, totalCount, recordCount, pageNumber, pageSize, totalCount != recordCount);

        /// <summary>
        /// Converte uma consulta em uma lista paginada
        /// </summary>
        /// <typeparam name="T">Tipo de entidade</typeparam>
        /// <param name="source">Consulta para paginação</param>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageNumber">Número de página atual</param>
        /// <param name="pageSize">Tamanho de registros por página</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, long recordCount, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return ToPagedList(items, count, recordCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Converte para uma lista paginada
        /// </summary>
        /// <typeparam name="T">Tipo de entidade</typeparam>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageNumber">Número de página atual</param>
        /// <param name="pageSize">Tamanho de registros por página</param>
        /// <returns>Lista paginada</returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, long recordCount, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return ToPagedList(items, count, recordCount, pageNumber, pageSize);
        }

        #endregion Public Methods
    }
}