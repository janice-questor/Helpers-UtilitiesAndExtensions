using System.Reflection;

namespace System.Linq.Expressions
{
    public static class LambdaExtensions
    {
        #region Public Methods

        public static MemberExpression GetMemberExpression(this LambdaExpression expression)
        {
            if(expression == null)
            {
                throw new ArgumentNullException($"{nameof(expression)}");
            }

            if(expression.Body is MemberExpression memberExpression)
            {
                return memberExpression;
            }
            else if(expression.Body is UnaryExpression unaryExpression)
            {
                return unaryExpression.Operand as MemberExpression;
            }
            else if(expression.Body is BinaryExpression binaryExpression &&
                     binaryExpression.Left is UnaryExpression unary)
            {
                return unary.Operand as MemberExpression;
            }
            else
            {
                throw new Exception("Não é possível recuperar o membro da expressão");
            }
        }

        public static string GetName<T, U>(this Expression<Func<T, U>> expression) => expression?.GetMemberExpression().Member.Name;

        public static string GetName(this LambdaExpression expression) => expression?.GetMemberExpression().Member.Name;

        public static PropertyInfo GetPropertyInfo(this LambdaExpression expression) => expression?.GetMemberExpression().Member as PropertyInfo;

        #endregion Public Methods
    }
}