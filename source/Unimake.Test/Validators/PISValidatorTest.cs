using Unimake.Validators;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Validators
{
    public class PISValidatorTest
    {
        #region Public Methods

        [Theory]
        [InlineData("")]
        [InlineData("          ")]
        [InlineData("    12443 2")]
        [InlineData("36323549758")]
        [InlineData("698261878846")]
        [InlineData("363292")]
        [InlineData("11111111111")]
        [InlineData("111111111")]
        [InlineData("11111RGT1111")]
        [InlineData("3632FS9QWDA#.,D659")]
        [InlineData("LGKSJFHDGEOPFE")]
        public void Invalid(string pis) =>
            Assert.False(PISValidator.Validate(ref pis, false));

        [Theory]
        [InlineData("577.63450.47-6")]
        [InlineData("27 0. 8 s8548.82- 3")]
        [InlineData("043.65606.46-7")]
        [InlineData("383.96157.78-2")]
        [InlineData("519.08495.49-2")]
        [InlineData("341.94003.18-8")]
        [InlineData("392.70769.83-3")]
        public void Valid(string pis) =>
            Assert.True(PISValidator.Validate(ref pis, false));

        [Theory]
        [InlineData("577.63450.47-6")]
        [InlineData("     ")]
        [InlineData("043.6   560 6 . 46-7")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("   341.94003.18-8    ")]
        [InlineData("39  2.7 0  769. 8  3-3  ")]
        public void ValidAndEmpty(string pis) =>
           Assert.True(PISValidator.Validate(ref pis, true));

        [Theory]
        [InlineData("577.63450.47-6")]
        [InlineData("043.65606.46-7")]
        [InlineData("341.94003.18-8")]
        [InlineData("392.70769.83-3")]
        public void ValidAndFormatted(string pis)
        {
            Assert.True(PISValidator.Validate(ref pis, true, true));
            Assert.Equal(Formatters.PISFormatter.Format(pis), pis);
        }

        #endregion Public Methods
    }
}