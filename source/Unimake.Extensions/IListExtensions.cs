using System.Linq;

namespace System.Collections.Generic
{
    public static class IListExtensions
    {
        #region Public Methods

        public static IList<TObject> GetInstanceOrEmpty<TObject>(this IList<TObject> list)
            => list ?? Enumerable.Empty<TObject>().ToList();

        #endregion Public Methods
    }
}