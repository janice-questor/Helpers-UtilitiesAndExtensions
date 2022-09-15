using Newtonsoft.Json;

namespace Unimake.Primitives.Collections.Page
{
    /// <summary>
    /// Informações dos itens paginados de uma <see cref="PagedList{T}"/>
    /// </summary>
    public sealed class PageInfo
    {
        /// <summary>
        /// Número da página atual
        /// </summary>

        #region Internal Constructors

        internal PageInfo()
        {
        }

        #endregion Internal Constructors

        #region Public Properties

        [JsonProperty(Order = 0)]
        public int CurrentPage { get; internal set; }

        /// <summary>
        /// Indica que os registros exibidos pela <see cref="PagedList{T}"/> estão filtrados
        /// </summary>
        public bool Filtered { get; internal set; }

        /// <summary>
        /// Se verdadeiro, existem mais itens além destes paginados
        /// </summary>
        [JsonProperty(Order = 4)]
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// Se verdadeiro, existem itens anteriores, normalmente não estão na página 1
        /// </summary>
        [JsonProperty(Order = 5)]
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// Quantidade de itens desta lista
        /// </summary>
        [JsonProperty(Order = 3)]
        public int ItemsCount { get; internal set; }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        [JsonProperty(Order = 1)]
        public int PageSize { get; internal set; }

        /// <summary>
        /// Quantidade total de itens não paginados
        /// </summary>
        [JsonProperty(Order = 5)]
        public int TotalCount { get; internal set; }

        /// <summary>
        /// Quantidade de páginas desta lista
        /// </summary>
        [JsonProperty(Order = 2)]
        public int TotalPages { get; internal set; }

        #endregion Public Properties
    }
}