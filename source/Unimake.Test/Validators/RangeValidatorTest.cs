using System;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Validators
{
    public class RangeValidatorTest
    {
        #region Public Methods

        [Fact]
        public void ValidateDateTimeRange()
        {
            Assert.Equal(true, DateTime.Now.AddTicks((long)12e10).ValidateRange(DateTime.Now.AddTicks((long)3e10), DateTime.Now.AddTicks((long)15e10)));
            Assert.Equal(false, DateTime.Now.AddTicks((long)25e10).ValidateRange(DateTime.Now.AddTicks((long)10), DateTime.Now.AddTicks((long)15e10)));
            Assert.Equal(true, DateTime.Now.AddTicks((long)6e10).ValidateRange(DateTime.Now.AddTicks((long)5e10), DateTime.Now.AddTicks((long)9e10)));
            Assert.Equal(false, DateTime.Now.AddTicks((long)3e10).ValidateRange(DateTime.Now.AddTicks((long)3e10), DateTime.Now.AddTicks((long)3e10)));
            Assert.Equal(true, DateTime.Now.AddTicks((long)19e10).ValidateRange(DateTime.Now.AddTicks((long)3e10), DateTime.Now.AddTicks((long)20e10)));
            Assert.Equal(false, DateTime.Now.AddTicks((long)2e10).ValidateRange(DateTime.Now.AddTicks((long)3e10), DateTime.Now.AddTicks((long)5e10)));
        }

        [Theory]
        [InlineData(12.34, 10, 15, true)]
        [InlineData(10.36, 8, 10, false)]
        [InlineData(19, 10, 20, true)]
        [InlineData(1.28, 1, 1.25, false)]
        [InlineData(0.5, 0.2, 0.6, true)]
        [InlineData(25.36, 10, 15, false)]
        [InlineData(19, 19, 19, true)]
        [InlineData(145.36, 145.36, 145.36, true)]
        public void ValidateDecimalRange(decimal value, decimal minValue, decimal maxValue, bool expected)
        {
            Assert.Equal(expected, value.ValidateRange(minValue, maxValue));
        }

        [Theory]
        [InlineData(12, 10, 15, true)]
        [InlineData(25, 10, 15, false)]
        [InlineData(10, 8, 10, true)]
        [InlineData(3, 1, 1, false)]
        [InlineData(19, 10, 20, true)]
        [InlineData(2, 3, 5, false)]
        public void ValidateIntRange(int value, int minValue, int maxValue, bool expected)
        {
            Assert.Equal(expected, value.ValidateRange(minValue, maxValue));
        }

        #endregion Public Methods
    }
}