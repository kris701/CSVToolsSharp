using CSVToolsSharp.Tests.TestClasses;

namespace CSVToolsSharp.Tests
{
    [TestClass]
    public class CSVSerialiserTests
    {
        public static IEnumerable<object[]> NormalData()
        {
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1")
                },
                $"Column1{Environment.NewLine}test1{Environment.NewLine}",
                typeof(Simple1),
                null
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1")
                },
                $"Column1{Environment.NewLine}test1  {Environment.NewLine}",
                typeof(Simple1),
                new CSVSerialiserOptions(){ PrettyOutput = true }
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1test1test1")
                },
                $"Column1        {Environment.NewLine}test1test1test1{Environment.NewLine}",
                typeof(Simple1),
                new CSVSerialiserOptions(){ PrettyOutput = true }
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1")
                },
                $"Column1{Environment.NewLine}test1  {Environment.NewLine}",
                typeof(Simple1),
                new CSVSerialiserOptions(){ PrettyOutput = true }
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple1("test1"),
                    new Simple1("test2")
                },
                $"Column1{Environment.NewLine}test1{Environment.NewLine}test2{Environment.NewLine}",
                typeof(Simple1),
                null
            };
            yield return new object[]
            {
                new List<dynamic>()
                {
                    new Simple2("test1", 1.5, DateTime.MinValue)
                },
                $"Column1,other Column 2,other Column 3{Environment.NewLine}test1,1.5,{DateTime.MinValue}{Environment.NewLine}",
                typeof(Simple2),
                null
            };
            yield return new object[]
{
                new List<dynamic>()
                {
                    new Simple2("test1", 1.5, DateTime.MinValue)
                },
                $"Column1;other Column 2;other Column 3{Environment.NewLine}test1;1.5;{DateTime.MinValue}{Environment.NewLine}",
                typeof(Simple2),
                new CSVSerialiserOptions() { Seperator = ';' }
            };
            yield return new object[]
{
                new List<dynamic>()
                {
                    new Simple2("test1", 1.5, DateTime.MinValue),
                    new Simple2("wwe   a", 6429593, DateTime.MinValue.AddDays(1))
                },
                $"Column1,other Column 2,other Column 3{Environment.NewLine}test1,1.5,{DateTime.MinValue}{Environment.NewLine}wwe   a,6429593,{DateTime.MinValue.AddDays(1)}{Environment.NewLine}",
                typeof(Simple2),
                null
            };
            yield return new object[] {
                new List<dynamic>
                {
                    new Simple3("test1")
                },
                $"Visible{Environment.NewLine}test1{Environment.NewLine}",
                typeof(Simple3),
                null
            };
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Serialise_1(List<dynamic> deserialised, string serialised, Type type, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            var text = CSVSerialiser.Serialise(deserialised, options);

            // ASSERT
            Assert.AreEqual(serialised, text);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Serialise_2(List<dynamic> deserialised, string serialised, Type type, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            var text = CSVSerialiser.Serialise(deserialised, type, options);

            // ASSERT
            Assert.AreEqual(serialised, text);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise(List<dynamic> deserialised, string serialised, Type type, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            var result = CSVSerialiser.Deserialise(serialised, type, options);

            // ASSERT
            Assert.IsInstanceOfType(result[0], type);
            Assert.IsTrue(AreEqual(deserialised, result));
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise_BackAndForth(List<dynamic> deserialised, string serialised, Type type, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            var des = CSVSerialiser.Deserialise(serialised, type, options);
            var ser = CSVSerialiser.Serialise(des, type, options);

            // ASSERT
            Assert.IsInstanceOfType(des[0], type);
            Assert.IsTrue(AreEqual(deserialised, des));
            Assert.AreEqual(serialised, ser);
        }

        private bool AreEqual<T>(List<T> one, List<T> other) where T : notnull
        {
            if (one.Count != other.Count)
                return false;
            for (int i = 0; i < one.Count; i++)
                if (!one[i].Equals(other[i]))
                    return false;
            return true;
        }
    }
}
