using Newtonsoft.Json;
using System;

namespace Unimake.Primitives.Collections.Page
{
    /// <summary>
    /// Informações de registros de uma <see cref="PagedList{T}"/>
    /// </summary>
    public sealed class RecordInfo
    {
        #region Internal Constructors

        internal RecordInfo(long recordCount, PageInfo pageInfo)
        {
            PageSize = pageInfo.PageSize;
            RecordCount = recordCount;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Instância
        /// </summary>
        /// <summary>
        /// Quantidade de páginas totais da <see cref="PagedList{T}"/>
        /// </summary>
        public long PageCount => (int)Math.Ceiling(RecordCount / (double)PageSize);

        /// <summary>
        /// Tamanho da página retornada pela <see cref="PagedList{T}"/>
        /// </summary>
        public long PageSize { get; }

        /// <summary>
        /// Quantidade total de registros na <see cref="PagedList{T}"/>.
        /// </summary>
        public long RecordCount { get; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        [JsonConstructor]
        public RecordInfo(long pageCount,
                          long pageSize,
                          long recordCount)
        {
            PageSize = pageSize;
            RecordCount = recordCount;
        }

        #endregion Public Constructors
    }
}