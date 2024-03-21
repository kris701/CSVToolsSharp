using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Tests
{
    [TestClass]
    public class CSVManagerTests
    {
        [TestMethod]
        [DataRow("TestData/file1.csv")]
        [DataRow("TestData/file2.csv")]
        [DataRow("TestData/file3.csv")]
        public void Can_LoadFromFile(string file)
        {
            // ARRANGE
            var manager = new CSVManager(new FileInfo("temp"));

            // ACT
            manager.LoadFromFile(new FileInfo(file));

            // ASSERT
            Assert.AreNotEqual(0, manager.Context.Columns);
            Assert.AreNotEqual(0, manager.Context.Rows);
        }

        [TestMethod]
        [DataRow("TestData/file1.csv")]
        [DataRow("TestData/file2.csv")]
        [DataRow("TestData/file3.csv")]
        public void Can_AppendToFile(string file)
        {
            // ARRANGE
            if (File.Exists("temp"))
                File.Delete("temp");
            var manager = new CSVManager(new FileInfo("temp"));
            manager.LoadFromFile(new FileInfo(file));
            var columns = manager.Context.Columns;

            // ACT
            manager.AppendToFile("more data", "newVal");

            // ASSERT
            Assert.IsTrue(File.Exists("temp"));
            Assert.AreNotEqual(columns, manager.Context.Columns);
        }
    }
}
