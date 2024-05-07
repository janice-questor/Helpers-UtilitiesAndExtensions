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

        [Fact]
        [Trait("Utility", "Net")]
        public void HasInternetConnectionTest()
        {
            string[] testUrls = new string[] {
                "http://clients3.google.com/generate_204",
                "http://www.microsoft.com",
                "http://www.cloudflare.com",
                "http://www.amazon.com",
                "http://www.unimake.com.br"
            };

            string[] urls;

            foreach (string url in testUrls)
            {
                urls = new string[] { url };

                Assert.True(Unimake.Net.Utility.HasInternetConnection(null, 3, urls));
            }

            urls = new string[] { "http://www.unimake.com" };

            Assert.False(Unimake.Net.Utility.HasInternetConnection(null, 3, urls));

            Assert.True(Unimake.Net.Utility.HasInternetConnection(3));
        }

        #endregion Public Methods
    }
}