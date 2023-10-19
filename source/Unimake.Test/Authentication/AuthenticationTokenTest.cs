using System;
using Unimake.Primitives.Security.Credentials;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Authentication
{
    public class AuthenticationTokenTest
    {
        #region Public Methods

        [Fact]
        public void AppId_And_Secret_ArgumentException() =>
            Assert.Throws<ArgumentException>(() => new AuthenticationToken().Validate());

        public void AppId_And_Secret_Ok()
        {
            var token = new AuthenticationToken(Primitives.Enumerations.GrantType.ClientCredentials, clientOrAppId: "marcelo", secret: "123456");
            token.Validate();
        }

        [Fact]
        public void ClientId_And_Secret_ArgumentException() =>
            Assert.Throws<ArgumentException>(() => new AuthenticationToken().Validate());

        [Fact]
        public void ClientId_And_Secret_Ok()
        {
            var token = new AuthenticationToken(Primitives.Enumerations.GrantType.ClientCredentials, clientOrAppId: "marcelo", secret: "123456");
            token.Validate();
        }

        [Fact]
        public void Username_And_Passsword_ArgumentException() =>
            Assert.Throws<ArgumentException>(() => new AuthenticationToken(Primitives.Enumerations.GrantType.Password).Validate());

        [Fact]
        public void Username_And_Password_Ok()
        {
            var token = new AuthenticationToken(Primitives.Enumerations.GrantType.Password, username: "marcelo", password: "123456");
            token.Validate();
        }

        #endregion Public Methods
    }
}