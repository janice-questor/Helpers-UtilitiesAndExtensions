using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Unimake.Primitives.Collections.Page
{
    public class PagedList<T>
    {
        #region Public Properties

        /// <summary>
        /// Itens desta página
        /// </summary>
        [JsonProperty(Order = 1)]
        public ReadOnlyCollection<T> Items { get; }

        /// <summary>
        /// Informações de página dos dados retornados nesta lista
        /// </summary>
        [JsonProperty(Order = 2)]
        public PageInfo PageInfo { get; private set; }

        /// <summary>
        /// Informações de registros no banco de dados
        /// </summary>
        [JsonProperty(Order = 3)]
        public RecordInfo RecordInfo { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Inicia  a lista com os itens desta página
        /// </summary>
        /// <param name="items">Itens já paginados que serão exibidos</param>
        ///<param name="totalCount">Contagem de registros antes da aplicar a paginação</param>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageNumber">Número de página atual</param>
        /// <param name="pageSize">Tamanho de registros por página</param>
        /// <param name="filtered">Se verdadeiro, o registro está filtrado</param>
        /// <returns></returns>
        public PagedList(IEnumerable<T> items, int totalCount, long recordCount, int pageNumber, int pageSize, bool filtered)
            : this(items, recordCount, new PageInfo(pageNumber, filtered, items.Count(), pageSize, totalCount))
        {
        }

        /// <summary>
        /// Inicia  a lista com os itens desta página
        /// </summary>
        /// <param name="items">Itens já paginados que serão exibidos</param>
        ///<param name="totalCount">Contagem de registros antes da aplicar a paginação</param>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageNumber">Número de página atual</param>
        /// <param name="pageSize">Tamanho de registros por página</param>
        /// <returns></returns>
        public PagedList(IEnumerable<T> items, int totalCount, long recordCount, int pageNumber, int pageSize)
            : this(items, recordCount, new PageInfo(pageNumber, totalCount != recordCount, items.Count(), pageSize, totalCount))
        {
        }

        /// <summary>
        /// Inicia  a lista com os itens desta página
        /// </summary>
        /// <param name="items">Itens já paginados que serão exibidos</param>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageInfo">Informações da página de registros</param>
        /// <returns></returns>
        public PagedList(IEnumerable<T> items, long recordCount, PageInfo pageInfo)
        {
            PageInfo = pageInfo ?? throw new ArgumentNullException(nameof(pageInfo));
            Items = new ReadOnlyCollection<T>(items?.ToList() ?? new List<T>());
            RecordInfo = new RecordInfo(recordCount, PageInfo);
        }

        #endregion Public Constructors
    }
}