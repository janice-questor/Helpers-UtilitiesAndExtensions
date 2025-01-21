using System;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Validators
{
    public class Base64Test
    {
        #region Public Methods

        [Fact]
        public void IsBase64()
        {
            var base64 = "SGVsbG8gV29ybGQ=";
            Assert.True(base64.IsBase64());

            base64 = "LVT5DvkYQEDudrs9dvrd";
            Assert.False(base64.IsBase64());//false, a string LVT5DvkYQEDudrs9dvrd retorna verdade, quando não deveria
        }

        #endregion Public Methods
    }
}