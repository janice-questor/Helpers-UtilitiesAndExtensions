using System;

namespace Unimake.Validators.Exceptions
{
    public class ValidationException : Exception
    {
        #region Public Constructors

        public ValidationException()
        { }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion Public Constructors
    }
}