using CSVToolsSharp.Exceptions;

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
        public void Can_AddColumn_1(string col)
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
        public void Can_AddColumn_2(string col)
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>()
            {
                { $"not-{col}", new List<string>(){ "val", "val2", "val3" } }
            });
            Assert.IsFalse(csv.GetColumns().Contains(col));
            Assert.IsTrue(csv.GetColumns().Contains($"not-{col}"));
            Assert.AreEqual(3, csv.Rows);

            // ACT
            csv.AddColumn(col);

            // ASSERT
            Assert.IsTrue(csv.GetColumns().Contains(col));
            foreach (var column in csv.GetColumns())
                Assert.AreEqual(3, csv.GetColumn(column).Count());
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

        [TestMethod]
        public void Can_GetRow()
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());
            csv.AddColumn("col1");
            csv.AddColumn("col2");
            csv.Insert("col1", 0, "123");
            csv.Insert("col2", 0, "456");
            csv.Insert("col1", 1, "abc");
            csv.Insert("col2", 1, "def");
            Assert.AreEqual(2, csv.Rows);
            Assert.AreEqual(2, csv.Columns);

            // ACT
            var data = csv.GetRow(0);

            // ASSERT
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("123", data[0]);
            Assert.AreEqual("456", data[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(DynamicCSVException))]
        [DataRow(1)]
        [DataRow(-50)]
        [DataRow(6942)]
        public void Cant_GetRow_IfOutOfRange(int row)
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());

            // ACT
            csv.GetRow(row);
        }

        [TestMethod]
        public void Can_GetColumn()
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());
            csv.AddColumn("col1");
            csv.AddColumn("col2");
            csv.Insert("col1", 0, "123");
            csv.Insert("col2", 0, "456");
            csv.Insert("col1", 1, "abc");
            csv.Insert("col2", 1, "def");
            Assert.AreEqual(2, csv.Rows);
            Assert.AreEqual(2, csv.Columns);

            // ACT
            var data = csv.GetColumn("col2");

            // ASSERT
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("456", data[0]);
            Assert.AreEqual("def", data[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(DynamicCSVException))]
        [DataRow("something")]
        [DataRow("")]
        [DataRow("123  aa")]
        public void Cant_GetColumn_IfNonExistent(string col)
        {
            // ARRANGE
            var csv = new DynamicCSV(new Dictionary<string, List<string>>());

            // ACT
            csv.GetColumn(col);
        }

        [TestMethod]
        public void Can_Equals_True()
        {
            // ARRANGE
            var csv1 = new DynamicCSV(new Dictionary<string, List<string>>());
            csv1.AddColumn("col1");
            csv1.AddColumn("col2");
            csv1.Insert("col1", 0, "123");
            csv1.Insert("col2", 0, "456");
            csv1.Insert("col1", 1, "abc");
            csv1.Insert("col2", 1, "def");
            Assert.AreEqual(2, csv1.Rows);
            Assert.AreEqual(2, csv1.Columns);

            var csv2 = new DynamicCSV(new Dictionary<string, List<string>>());
            csv2.AddColumn("col1");
            csv2.AddColumn("col2");
            csv2.Insert("col1", 0, "123");
            csv2.Insert("col2", 0, "456");
            csv2.Insert("col1", 1, "abc");
            csv2.Insert("col2", 1, "def");
            Assert.AreEqual(2, csv2.Rows);
            Assert.AreEqual(2, csv2.Columns);

            // ACT
            var result = csv1.Equals(csv2);

            // ASSERT
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_Equals_False()
        {
            // ARRANGE
            var csv1 = new DynamicCSV(new Dictionary<string, List<string>>());
            csv1.AddColumn("col1");
            csv1.AddColumn("col2");
            csv1.Insert("col1", 0, "123");
            csv1.Insert("col2", 0, "456");
            csv1.Insert("col1", 1, "abc");
            csv1.Insert("col2", 1, "def");
            Assert.AreEqual(2, csv1.Rows);
            Assert.AreEqual(2, csv1.Columns);

            var csv2 = new DynamicCSV(new Dictionary<string, List<string>>());
            csv2.AddColumn("col1");
            csv2.AddColumn("col2");
            csv2.Insert("col1", 0, "123");
            csv2.Insert("col2", 0, "46");
            csv2.Insert("col1", 1, "abc");
            csv2.Insert("col2", 1, "def");
            Assert.AreEqual(2, csv2.Rows);
            Assert.AreEqual(2, csv2.Columns);

            // ACT
            var result = csv1.Equals(csv2);

            // ASSERT
            Assert.IsFalse(result);
        }
    }
}
