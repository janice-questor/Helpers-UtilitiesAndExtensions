using System;
using Unimake.Net;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Net
{
    public class UtilityTest
    {
        #region Public Methods

        [Fact]
        public void GetLocalIPV4Address() => Console.WriteLine(Utility.GetLocalIPAddress());

        [Fact]
        public void GetLocalIPV6Address() => Console.WriteLine(Utility.GetLocalIPAddress(true));

        [Theory]
        [InlineData(01)]
        [InlineData(02)]
        [InlineData(03)]
        [InlineData(04)]
        [InlineData(05)]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(30)]
        public void HasInternetConnection(int timeoutInSeconds) => Assert.True(Unimake.Net.Utility.HasInternetConnection(timeoutInSeconds));

        #endregion Public Methods
    }
}