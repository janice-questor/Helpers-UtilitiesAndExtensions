using System;

namespace Unimake.Primitives.Collections.Page
{
    /// <summary>
    /// Informações de registros de uma <see cref="PagedList{T}"/>
    /// </summary>
    public sealed class RecordInfo
    {
        #region Internal Constructors

        /// <summary>
        /// Instância
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="pageInfo"></param>
        internal RecordInfo(long recordCount, PageInfo pageInfo)
        {
            PageSize = pageInfo.PageSize;
            RecordCount = recordCount;
            PageCount = (int)Math.Ceiling(RecordCount / (double)PageSize);
            pageInfo.Filtered = pageInfo.TotalCount != recordCount;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Quantidade de páginas totais da <see cref="PagedList{T}"/>
        /// </summary>
        public long PageCount { get; }

        /// <summary>
        /// Tamanho da página retornada pela <see cref="PagedList{T}"/>
        /// </summary>
        public long PageSize { get; }

        /// <summary>
        /// Quantidade total de registros na <see cref="PagedList{T}"/>.
        /// </summary>
        public long RecordCount { get; }

        #endregion Public Properties
    }
}