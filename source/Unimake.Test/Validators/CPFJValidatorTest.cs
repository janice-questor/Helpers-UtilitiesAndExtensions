using Unimake.Validators;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Validators
{
    public class CPFValidatorTest
    {
        #region Public Methods

        [Theory]
        [InlineData("")]
        [InlineData("          ")]
        [InlineData("    12443 2")]
        [InlineData("36323549758")]
        [InlineData("69826152446")]
        [InlineData("363292")]
        [InlineData("11111111111")]
        [InlineData("111111111")]
        [InlineData("11111RGT1111")]
        [InlineData("3632FS9QWDA#.,D659")]
        [InlineData("LGKSJFHDGEOPFE")]
        public void Invalid(string cpf) =>
            Assert.False(CPFValidator.Validate(ref cpf, false));

        [Theory]
        [InlineData("062.587.420-09")]
        [InlineData("06 2  . 587. 4 2 0 -0 9")]
        [InlineData("062.587.s420-s09")]
        [InlineData("696.010.070-40")]
        [InlineData("144.859.520-70")]
        [InlineData("065.396.030-10")]
        [InlineData("337.413.050-06")]
        public void Valid(string cpf) =>
            Assert.True(CPFValidator.Validate(ref cpf, false));

        [Theory]
        [InlineData("062.587.420-09")]
        [InlineData("06 2  . 587. 4 2 0 -0 9")]
        [InlineData("062.587.s420-s09")]
        [InlineData("696.010.070-40")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("      ")]
        public void ValidAndEmpty(string cpf) =>
           Assert.True(CPFValidator.Validate(ref cpf, true));

        [Theory]
        [InlineData("062.587.420-09")]
        [InlineData("696.010.070-40")]
        [InlineData("144.859.520-70")]
        [InlineData("065.396.030-10")]
        [InlineData("337.413.050-06")]
        public void ValidAndFormatted(string cpf)
        {
            Assert.True(CPFValidator.Validate(ref cpf, true, true));
            Assert.Equal(Formatters.CPFFormatter.Format(cpf), cpf);
        }

        #endregion Public Methods
    }
}