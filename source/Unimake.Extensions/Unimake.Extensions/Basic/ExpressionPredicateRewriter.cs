using System.Linq.Expressions;

namespace Unimake.Utility.Basic
{
    public class ExpressionPredicateRewriter : ExpressionVisitor
    {
        #region Private Fields

        private readonly ParameterExpression _parameter = null;

        #endregion Private Fields

        #region Protected Methods

        protected override Expression VisitParameter(ParameterExpression node) => _parameter;

        #endregion Protected Methods

        #region Public Constructors

        public ExpressionPredicateRewriter(ParameterExpression parameterExpression) => _parameter = parameterExpression;

        #endregion Public Constructors
    }
}