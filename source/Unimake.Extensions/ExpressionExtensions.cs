namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        #region Private Methods

        private static Expression<TObject> Compose<TObject>(this Expression<TObject> left, Expression<TObject> right, Func<Expression, Expression, Expression> merge)
        {
            var rewrittenRight = new ExpressionPredicateRewriter(left.Parameters.First()).Visit(right.Body);

            return Expression.Lambda<TObject>(merge(left.Body, rewrittenRight), left.Parameters);
        }

        #endregion Private Methods

        #region Public Methods

        public static Expression<Func<TObject, bool>> And<TObject>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right) => left.Compose(right, Expression.AndAlso);

        public static Expression<Func<TObject, bool>> AndIf<TObject>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right, bool clause) => clause ? left.And(right) : left;

        public static Expression<Func<TObject, bool>> AndIfNotNull<TObject, TValue>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right, TValue? value)
            where TValue : struct => left.AndIf(right, value.HasValue);

        public static Expression<Func<TObject, bool>> Or<TObject>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right) => left.Compose(right, Expression.OrElse);

        public static Expression<Func<TObject, bool>> OrIf<TObject>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right, bool clause) => clause ? left.Or(right) : left;

        public static Expression<Func<TObject, bool>> OrIfNotEmpty<TObject>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right, string value) => left.OrIf(right, !string.IsNullOrEmpty(value));

        public static Expression<Func<TObject, bool>> OrIfNotNull<TObject, TValue>(this Expression<Func<TObject, bool>> left, Expression<Func<TObject, bool>> right, TValue? value)
            where TValue : struct => left.OrIf(right, value.HasValue);

        #endregion Public Methods
    }
}