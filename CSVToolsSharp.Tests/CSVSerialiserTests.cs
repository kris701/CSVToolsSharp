using CSVToolsSharp.Tests.TestClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSVToolsSharp.Tests
{
    [TestClass]
    public class CSVSerialiserTests
    {
        public static IEnumerable<object[]> Data()
        {
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1")
                },
                $"Column1{Environment.NewLine}test1{Environment.NewLine}",
                typeof(Simple1)
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1"),
                    new Simple1("test2")
                },
                $"Column1{Environment.NewLine}test1{Environment.NewLine}test2{Environment.NewLine}",
                typeof(Simple1)
            };
            yield return new object[]
            {
                new List<dynamic>()
                {
                    new Simple2("test1", 1.5, DateTime.MinValue)
                },
                $"Column1,other Column 2,other Column 3{Environment.NewLine}test1,1.5,{DateTime.MinValue}{Environment.NewLine}",
                typeof(Simple2)
            };
            yield return new object[]
{
                new List<dynamic>()
                {
                    new Simple2("test1", 1.5, DateTime.MinValue),
                    new Simple2("wwe   ", 6429593, DateTime.MinValue.AddDays(1))
                },
                $"Column1,other Column 2,other Column 3{Environment.NewLine}test1,1.5,{DateTime.MinValue}{Environment.NewLine}wwe   ,6429593,{DateTime.MinValue.AddDays(1)}{Environment.NewLine}",
                typeof(Simple2)
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple3("test1")
                },
                $"Visible{Environment.NewLine}test1{Environment.NewLine}",
                typeof(Simple3)
            };
        }

        [TestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Method)]
        public void Can_Serialise(List<dynamic> deserialised, string serialised, Type type)
        {
            // ARRANGE
            // ACT
            var text = CSVSerialiser.Serialise(deserialised);

            // ASSERT
            Assert.AreEqual(serialised,text);
        }

        [TestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Method)]
        public void Can_Deserialise(List<dynamic> deserialised, string serialised, Type type)
        {
            // ARRANGE
            // ACT
            var result = CSVSerialiser.Deserialise(type, serialised);

            // ASSERT
            Assert.IsInstanceOfType(result[0], type);
            Assert.IsTrue(AreEqual(deserialised, result));
        }

        [TestMethod]
        [DynamicData(nameof(Data), DynamicDataSourceType.Method)]
        public void Can_Deserialise_BackAndForth(List<dynamic> deserialised, string serialised, Type type)
        {
            // ARRANGE
            // ACT
            var des = CSVSerialiser.Deserialise(type, serialised);
            var ser = CSVSerialiser.Serialise(type, des);

            // ASSERT
            Assert.IsInstanceOfType(des[0], type);
            Assert.IsTrue(AreEqual(deserialised, des));
            Assert.AreEqual(serialised, ser);
        }

        private bool AreEqual<T>(List<T> one, List<T> other)
        {
            foreach (var item in one)
                if (!other.Contains(item))
                    return false;
            return true;
        }
    }
}
