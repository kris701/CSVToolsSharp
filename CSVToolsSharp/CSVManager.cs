using System.IO;
using System.Reflection.PortableExecutable;

namespace CSVToolsSharp
{
    /// <summary>
    /// Simple CSV manager
    /// </summary>
    public class CSVManager
    {
        /// <summary>
        /// The active CSV file
        /// </summary>
        public FileInfo CSVFile { get; set; }
        /// <summary>
        /// Context that contains the CSV data
        /// </summary>
        public CSVContext Context { get; set; }

        public CSVManager(FileInfo file)
        {
            CSVFile = file;
            Context = new CSVContext();
        }

        /// <summary>
        /// Loads a CSV file into the CSV context
        /// </summary>
        /// <param name="file"></param>
        public void LoadFromFile(FileInfo file) => Context.LoadFromString(File.ReadAllText(file.FullName));

        /// <summary>
        /// Append a string value to a column in the CSV file.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void AppendToFile(string col, string value)
        {
            Context.Append(col, value);
            File.WriteAllText(CSVFile.FullName, Context.ToString());
        }
    }
}
