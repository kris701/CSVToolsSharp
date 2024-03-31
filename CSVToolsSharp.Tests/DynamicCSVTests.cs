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
        #region Serialisation and Deserialisation
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
            string text = CSVSerialiser.Serialise(deserialised, options);

            // ASSERT
            Assert.AreEqual(serialised, text);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise(DynamicCSV deserialised, string serialised, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            DynamicCSV result = CSVSerialiser.Deserialise(serialised, options);

            // ASSERT
            Assert.AreEqual(deserialised, result);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise_BackAndForth(DynamicCSV deserialised, string serialised, CSVSerialiserOptions options)
        {
            // ARRANGE
            // ACT
            DynamicCSV des = CSVSerialiser.Deserialise(serialised, options);
            string ser = CSVSerialiser.Serialise(des, options);

            // ASSERT
            Assert.AreEqual(deserialised, des);
            Assert.AreEqual(serialised, ser);
        }
        #endregion

        [TestMethod]
        [DataRow("col name")]
        [DataRow("")]
        [DataRow("q1t0dsvWAåpb")]
        public void Can_AddColumn(string col)
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());
            Assert.IsFalse(csv.GetColumns().Contains(col));

            // ACT
            csv.AddColumn(col);

            // ASSERT
            Assert.IsTrue(csv.GetColumns().Contains(col));
        }

        [TestMethod]
        [DataRow("col name")]
        [DataRow("")]
        [DataRow("q1t0dsvWAåpb")]
        public void Can_RemoveColumn(string col)
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());
            csv.AddColumn(col);
            Assert.IsTrue(csv.GetColumns().Contains(col));

            // ACT
            csv.RemoveColumn(col);

            // ASSERT
            Assert.IsFalse(csv.GetColumns().Contains(col));
        }

        [TestMethod]
        public void Can_AddRow()
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());
            csv.AddColumn("a");
            Assert.AreNotEqual(1, csv.Rows);

            // ACT
            csv.AddRow();

            // ASSERT
            Assert.AreEqual(1, csv.Rows);
        }

        [TestMethod]
        public void Can_RemoveRow()
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());
            csv.AddColumn("a");
            csv.AddRow();
            Assert.AreEqual(1, csv.Rows);

            // ACT
            csv.RemoveRow(0);

            // ASSERT
            Assert.AreNotEqual(1, csv.Rows);
        }

        [TestMethod]
        [DataRow("col", 0, "value")]
        [DataRow("col", 1, "value")]
        [DataRow("more", 42945, "value")]
        public void Can_Insert(string col, int row, string data)
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());

            // ACT
            csv.Insert(col, row, data);

            // ASSERT
            Assert.AreEqual(data, csv.GetCell(col, row));
        }

    }
}
