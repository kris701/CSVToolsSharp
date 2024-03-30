using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Tests
{
    [TestClass]
    public class DynamicCSVTests
    {
        public static IEnumerable<object[]> NormalData()
        {
            yield return new object[] {
                new DynamicCSV(new Dictionary<string, List<string>>()
                {
                    { "Column1", new List<string>(){ "test1" } }
                }),
                $"Column1{Environment.NewLine}test1{Environment.NewLine}",
                new CSVSerialiserOptions()
            };
            yield return new object[] {
                new DynamicCSV(new Dictionary<string, List<string>>()
                {
                    { "Column1", new List<string>(){ "test1" } },
                    { "Column5", new List<string>(){ "test2" } }
                }),
                $"Column1,Column5{Environment.NewLine}test1,test2{Environment.NewLine}",
                new CSVSerialiserOptions()
            };
            yield return new object[] {
                new DynamicCSV(new Dictionary<string, List<string>>()
                {
                    { "Column1", new List<string>(){ "test1", "other1" } },
                    { "Column5", new List<string>(){ "test2", "other2" } }
                }),
                $"Column1,Column5{Environment.NewLine}test1,test2{Environment.NewLine}other1,other2{Environment.NewLine}",
                new CSVSerialiserOptions()
            };
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Serialise(DynamicCSV deserialised, string serialised, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            var text = CSVSerialiser.DynamicSerialise(deserialised, options);

            // ASSERT
            Assert.AreEqual(serialised, text);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise(DynamicCSV deserialised, string serialised, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            var result = CSVSerialiser.DynamicDeserialise(serialised, options);

            // ASSERT
            Assert.AreEqual(deserialised, result);
        }
    }
}
