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
        ///<param name="count">Contagem de registros antes da aplicar a paginação</param>
        /// <param name="recordCount">Quantidade total de registros no banco de dados</param>
        /// <param name="pageNumber">Número de página atual</param>
        /// <param name="pageSize">Tamanho de registros por página</param>
        /// <returns></returns>
        public PagedList(IEnumerable<T> items, int count, long recordCount, int pageNumber, int pageSize)
        {
            Items = new ReadOnlyCollection<T>(items?.ToList() ?? new List<T>());

            PageInfo = new PageInfo
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                ItemsCount = Items.Count
            };

            RecordInfo = new RecordInfo(recordCount, PageInfo);
        }

        #endregion Public Constructors
    }
}