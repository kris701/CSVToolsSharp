namespace CSVToolsSharp.Tests
{
    [TestClass]
    public class CSVContextTests
    {
        [TestMethod]
        [DataRow("TestData/file1.csv", 1, 1)]
        [DataRow("TestData/file2.csv", 2, 2)]
        [DataRow("TestData/file3.csv", 4, 3)]
        public void Can_LoadFromString(string file, int expectedCols, int expectedRows)
        {
            // ARRANGE
            var context = new CSVContext();

            // ACT
            context.LoadFromString(File.ReadAllText(file).Replace("\r", ""));

            // ASSERT
            Assert.AreEqual(expectedCols, context.Columns);
            Assert.AreEqual(expectedRows, context.Rows);
        }

        [TestMethod]
        [DataRow("TestData/file1.csv")]
        [DataRow("TestData/file2.csv")]
        [DataRow("TestData/file3.csv")]
        public void Can_Reset(string file)
        {
            // ARRANGE
            var context = new CSVContext();
            context.LoadFromString(File.ReadAllText(file).Replace("\r", ""));
            Assert.AreNotEqual(0, context.Columns);
            Assert.AreNotEqual(0, context.Rows);

            // ACT
            context.Reset();

            // ASSERT
            Assert.AreEqual(0, context.Columns);
            Assert.AreEqual(0, context.Rows);
        }

        [TestMethod]
        [DataRow("col")]
        [DataRow("col   a")]
        [DataRow("")]
        public void Can_AddColumns(string col)
        {
            // ARRANGE
            var context = new CSVContext();
            Assert.AreEqual(0, context.Columns);

            // ACT
            context.AddColumn(col);

            // ASSERT
            context.Get(col);
        }

        [TestMethod]
        [DataRow("col", "value")]
        [DataRow("col", "value   2")]
        [DataRow("col", "")]
        public void Can_Append_1(string col, string value)
        {
            // ARRANGE
            var context = new CSVContext();
            Assert.AreEqual(0, context.Columns);

            // ACT
            context.Append(col, value);

            // ASSERT
            Assert.AreEqual(value, context.Get(col));
        }

        [TestMethod]
        [DataRow("col", "value")]
        [DataRow("col", "value   2")]
        [DataRow("col", " ")]
        public void Can_Append_2(string col, string value)
        {
            // ARRANGE
            var context = new CSVContext();
            context.Append("newCol", "some other value");
            context.Row = 1;

            // ACT
            context.Append(col, value);

            // ASSERT
            context.Row = 0;
            Assert.AreNotEqual(value, context.Get(col));
            context.Row = 1;
            Assert.AreEqual(value, context.Get(col));
        }

        [TestMethod]
        [DataRow("TestData/file1.csv")]
        [DataRow("TestData/file2.csv")]
        [DataRow("TestData/file3.csv")]
        public void Can_ToString(string file)
        {
            // ARRANGE
            var context = new CSVContext();
            var fileText = File.ReadAllText(file).Replace("\r","");
            context.LoadFromString(fileText);

            // ACT
            var result = context.ToString();

            // ASSERT
            Assert.AreEqual(result, fileText);
        }
    }
}