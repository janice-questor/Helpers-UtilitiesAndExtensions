using System.Diagnostics;
using Unimake.Formatters;
using Unimake.Validators;
using Xunit;

namespace Unimake.Test.Formatters
{
    public class TelefoneTest
    {
        #region Public Methods

        [Fact]
        public void TelefoneAllowEmptyValid()
        {
            Assert.Equal(true, PhoneValidator.Validate(""));
        }

        [Fact]
        public void TelefoneFormat()
        {
            Debug.WriteLine(PhoneFormatter.Format("12345678"));
            Debug.WriteLine(PhoneFormatter.Format("1212345678"));
            Debug.WriteLine(PhoneFormatter.Format("08001234567"));
            Debug.WriteLine(PhoneFormatter.Format("0151112345678"));
            Debug.WriteLine(PhoneFormatter.Format("551112345678"));
            Debug.WriteLine(PhoneFormatter.Format("5511989601260"));
            Debug.WriteLine(PhoneFormatter.Format("+5511989601260"));
        }

        [Fact]
        public void TelefoneInvalid()
        {
            Assert.Equal(false, PhoneValidator.Validate("3265"));
        }

        [Fact]
        public void TelefoneNotAllowEmptyValid()
        {
            Assert.Equal(false, PhoneValidator.Validate("", false));
        }

        #endregion Public Methods
    }
}