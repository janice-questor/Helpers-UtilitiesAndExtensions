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
        public void ConvertFrom_Newtonsoft_Json()
        {
            var json = "{\"grantType\":\"Password\",\"appId\":\"user@test.com\",\"secret\":\"123456\"}";
            var token = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthenticationToken>(json);
            Assert.NotNull(token);

            json = "{\"grantType\":\"0\",\"appId\":\"user@test.com\",\"secret\":\"123456\"}";
            token = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthenticationToken>(json);
            Assert.NotNull(token);
        }

        [Fact]
        public void ConvertFrom_System_Text_Json()
        {
            var json = "{\"grantType\":\"Password\",\"appId\":\"user@test.com\",\"secret\":\"123456\"}";
            var token = System.Text.Json.JsonSerializer.Deserialize<AuthenticationToken>(json);
            Assert.NotNull(token);

            json = "{\"grantType\":\"0\",\"appId\":\"user@test.com\",\"secret\":\"123456\"}";
            token = System.Text.Json.JsonSerializer.Deserialize<AuthenticationToken>(json);
            Assert.NotNull(token);
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