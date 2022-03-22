using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xunit;

namespace Unimake.Helpers_UtilitiesAndExtensions.Test.Types
{
    public class DateTimeRangeTest
    {
        #region Private Classes

        private class DateTimeRangeGenerator : IEnumerable<object[]>
        {
            #region Public Methods

            public IEnumerator<object[]> GetEnumerator()
            {
                foreach(var item in new object[][]
                {
                    new object[]{
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(5)),
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)),
                        false
                    },
                    new object[]{
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)),
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)),
                        true
                    },
                    new object[]{
                        null,
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)),
                        false
                    },
                    new object[]{
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(12)),
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(7)),
                        false
                    },
                    new object[]{
                        new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(5)),
                        null,
                        false
                    },
                    new object[]{
                        null,
                        null,
                        true
                    }
                })
                {
                    yield return item;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            #endregion Public Methods
        }

        #endregion Private Classes

        #region Public Methods

        [Theory]
        [ClassData(typeof(DateTimeRangeGenerator))]
        public void EqualOperatorTest(DateTimeRange start, DateTimeRange end, bool expected)
        {
            Assert.Equal(start == end, expected);
            Assert.Equal(start.Equals(end), expected);
            Assert.Equal(end.Equals(start), expected);
        }

        [Fact]
        public void JsonTest()
        {
            foreach(var item in new DateTimeRangeGenerator().Where(w => w[0] != null && w[1] != null))
            {
                var range = (DateTimeRange)item[0];
                var json = JsonConvert.SerializeObject(range);
                range = JsonConvert.DeserializeObject<DateTimeRange>(json);
                Console.WriteLine(json);
                Console.WriteLine($"{range}");
            }
        }

        [Fact]
        public void ToStringTest()
        {
            foreach(var item in new DateTimeRangeGenerator().Where(w => w[0] != null && w[1] != null))
            {
                var range = (DateTimeRange)item[0];
                Console.WriteLine(range.ToString());
                Console.WriteLine(range.ToString("", null));
                Console.WriteLine(range.ToShortDateString());
            }
        }

        [Fact]
        public void XmlTest()
        {
            foreach(var item in new DateTimeRangeGenerator().Where(w => w[0] != null && w[1] != null))
            {
                var range = (DateTimeRange)item[0];
                var xml = XmlHelper.Serialize(range);
                range = XmlHelper.Deserialize<DateTimeRange>(xml);
                Console.WriteLine(xml);
                Console.WriteLine($"{range}");
            }
        }

        #endregion Public Methods
    }
}