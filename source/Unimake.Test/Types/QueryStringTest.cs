using System;
using System.Http;
using System.Linq;
using Xunit;
using static System.Diagnostics.Debug;
using Assert = Xunit.Assert;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Types
{
    public class QueryStringTest
    {
        #region Public Fields

        public QueryString DummyData = new QueryString
            {
                { "key0", "value"},
                { "key1", 10},
                { "key2", 12m},
                { "key3", 125d}
            };

        #endregion Public Fields

        #region Public Properties

        public QueryString DummyQuery { get; } = new QueryString();

        #endregion Public Properties

        #region Public Methods

        [Fact]
        public void Add()
        {
            var query = new QueryString();
            query.Add("key0", "value");
            query.Add("key1", DateTime.Now);
            query.Add("key2", 12m);
            query.Add("key3", 125d);

            Assert.Equal(4, query.Count);

            var value = query["key2"] = 15;

            Assert.Equal(15, value);
            Assert.Equal(4, query.Count);

            value = query["key4"] = "teste";

            Assert.Equal("teste", value);
            Assert.Equal(5, query.Count);
        }

        [Fact]
        public void AddFromProperty()
        {
            DummyQuery.Add("key0", "value");
            DummyQuery.Add("key1", DateTime.Now);
            DummyQuery.Add("key2", 12m);
            DummyQuery.Add("key3", 125d);

            Assert.Equal(4, DummyQuery.Count);

            var value = DummyQuery.AddOrUpdateValue("key2", 15).Value;

            Assert.Equal(15, value);
            Assert.Equal(4, DummyQuery.Count);

            value = DummyQuery.AddOrUpdateValue("key4", "teste").Value;

            Assert.Equal("teste", value);
            Assert.Equal(5, DummyQuery.Count);
        }

        [Fact]
        public void Contains()
        {
            Assert.Equal(4, DummyData.Count);
            Assert.True(DummyData.Contains("key2"));
            Assert.False(DummyData.Contains("key"));
        }

        [Fact]
        public void Count() => Assert.Equal(4, DummyData.Count);

        [Fact]
        public void GetEnumerator()
        {
            foreach(var item in DummyData.OrderBy(o => o.Key))
            {
                WriteLine(item);
            }

            var value = DummyData.Where(w => w.Key == "key2").First();
            Assert.True(value.Key == "key2");
            WriteLine(value);
        }

        [Fact]
        public void Remove()
        {
            var dataToRemove = new QueryString
            {
                { "key0", "value"},
                { "key1", DateTime.Now},
                { "key2", 12m},
                { "key3", 125d}
            };

            Assert.Equal(4, dataToRemove.Count);
            Assert.True(dataToRemove.Remove("key2"));
            Assert.Equal(3, dataToRemove.Count);
            Assert.False(dataToRemove.Contains("key2"));
            Assert.False(dataToRemove.Remove("key2"));
        }

        [Fact]
        public void ToQueryString()
        {
            var expected = "?key0=value&key1=10&key2=12&key3=125";
            var atual = DummyData.ToString();
            Assert.Equal(expected, atual);

            expected = "?key0=value%key1=10%key2=12%key3=125";
            atual = DummyData.ToString(true, keyValueSeparator: "%");
            Assert.Equal(expected, atual);

            expected = "?key0=value&key1=10%2c00&key2=12%2c00&key3=125%2c00";
            atual = DummyData.ToString(formatValue: obj => string.Format("{0:F2}", obj));
            Assert.Equal(expected, atual);

            expected = "key0=value&key1=10&key2=12&key3=125";
            atual = DummyData.ToString(false);
            Assert.Equal(expected, atual);

            expected = "?key0=value&key1=10,00&key2=12,00&key3=125,00";
            atual = DummyData.ToString(urlEncodeValue: false, formatValue: obj => string.Format("{0:F2}", obj));
            Assert.Equal(expected, atual);

            expected = "?key0=value&key1=10%2c00&key2=12%2c00&key3=125%2c00";
            atual = DummyData.ToString(formatValue: obj => string.Format("{0:F2}", obj));
            Assert.Equal(expected, atual);

            expected = "key0=value&key1=10&key2=12&key3=125";
            atual = DummyData.ToString(false);
            Assert.Equal(expected, atual);

            expected = "?key0=value&key1=10&key2=12&key3=125";
            Assert.Equal(expected, DummyData);
        }

        [Fact]
        public void ValidateEquals()
        {
            Assert.True(DummyData.Equals(DummyData));
            Assert.True(DummyData.Equals(new QueryString
            {
                { "key0", "value"},
                { "key1", 10},
                { "key2", 12m},
                { "key3", 125d}
            }));

            Assert.False(new QueryString().Equals(default(QueryString)));

            Assert.False(DummyData.Equals(new QueryString
            {
                { "key0", "value"},
                { "key1", 11},
                { "key2", 12m},
                { "key3", 125d}
            }));

            Assert.False(DummyData.Equals(null));
            Assert.False(DummyData.Equals(default(QueryString)));
            Assert.False(DummyData.Equals(default));
        }

        [Fact]
        public void ValidateEqualityOperator()
        {
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.True(DummyData == DummyData);
#pragma warning restore CS1718 // Comparison made to same variable

            Assert.True(DummyData == new QueryString
            {
                { "key0", "value"},
                { "key1", 10},
                { "key2", 12m},
                { "key3", 125d}
            });

            Assert.False(new QueryString() == default(QueryString));
            Assert.True(new QueryString() != default(QueryString));

            Assert.False(default(QueryString) == new QueryString());
            Assert.True(default(QueryString) != new QueryString());

            Assert.False(DummyData == new QueryString
            {
                { "key0", "value"},
                { "key1", 11},
                { "key2", 12m},
                { "key3", 125d}
            });

            Assert.False(DummyData == null);
            Assert.False(DummyData == default(QueryString));
            Assert.False(DummyData == default);

            Assert.False(null == DummyData);
            Assert.False(default(QueryString) == DummyData);
            Assert.False(default == DummyData);

            Assert.True(DummyData != null);
            Assert.True(DummyData != default(QueryString));
            Assert.True(DummyData != default);
                   
            Assert.True(null != DummyData);
            Assert.True(default(QueryString) != DummyData);
            Assert.True(default != DummyData);
        }

        [Fact]
        public void ValidateEqualsQueryStringItem()
        {
            var expected = new QueryStringItem("key1", "value1");
            var atual = new QueryStringItem("key1", "value1");
            Assert.Equal(expected, atual);
            Assert.True(expected == atual);

            expected = new QueryStringItem("key1", "value1");
            atual = new QueryStringItem("key1", "value2");
            Assert.NotEqual(expected, atual);
            Assert.True(expected != atual);
            Assert.False(expected == atual);
        }

        [Fact]
        public void ValidateHashCode()
        {
            WriteLine(DummyData.GetHashCode());
        }

        #endregion Public Methods
    }
}