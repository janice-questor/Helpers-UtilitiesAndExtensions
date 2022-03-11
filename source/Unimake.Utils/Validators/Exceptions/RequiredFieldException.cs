namespace Unimake.Validators.Exceptions
{
    public class RequiredFieldException : ValidationException
    {
        #region Public Properties

        public string FieldName { get; }

        #endregion Public Properties

        #region Public Constructors

        public RequiredFieldException(string fieldName)
            : base(string.Format("O campo '{0}' é requerido mas não foi informado.", fieldName)) =>
            FieldName = fieldName;

        #endregion Public Constructors
    }
}