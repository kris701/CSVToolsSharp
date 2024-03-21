using System.IO;

namespace CSVToolsSharp
{
    /// <summary>
    /// Simple CSV manager
    /// </summary>
    public class CSVManager
    {
        public FileInfo CSVFile { get; set; }
        private int _row = 0;
        public int Row {
            get => _row;
            set { 
                _row = value;
                foreach(var key in _csvData.Keys)
                    while (_csvData[key].Count <= _row)
                        _csvData[key].Add("");
            }
        }

        private readonly Dictionary<string, List<string>> _csvData;

        public CSVManager(FileInfo file)
        {
            CSVFile = file;

            _csvData = new Dictionary<string, List<string>>();
            if (file.Exists)
            {
                var lines = File.ReadAllLines(file.FullName);
                var headers = lines[0].Split(',').ToList();
                foreach (var header in headers)
                    _csvData.Add(header, new List<string>());
                foreach (var line in lines.Skip(1))
                {
                    var split = line.Split(',').ToList();
                    for (int i = 0; i < split.Count; i++)
                        _csvData[headers[i]].Add(split[i]);
                }
            }
        }

        /// <summary>
        /// Append a string value to a column in the CSV file.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="value"></param>
        public void Append(string col, string value)
        {
            if (!_csvData.ContainsKey(col))
            {
                _csvData.Add(col, new List<string>(Row + 1));
                while (_csvData[col].Count <= Row)
                    _csvData[col].Add("");
            }
            _csvData[col][Row] = value;
            UpdateCSVFile();
        }

        private void UpdateCSVFile() => File.WriteAllText(CSVFile.FullName, ToString());

        /// <summary>
        /// Outputs the CSV file as a text string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var text = "";
            foreach (var col in _csvData.Keys)
                text += $"{col},";
            text = text.Remove(text.Length - 1);
            text += Environment.NewLine;

            var maxRow = _csvData.Max(x => x.Value.Count);
            for (int i = 0; i < maxRow; i++)
            {
                foreach (var col in _csvData.Keys)
                {
                    if (_csvData[col].Count > i)
                        text += $"{_csvData[col][i]},";
                    else
                        text += ",";
                }
                text = text.Remove(text.Length - 1);
                text += Environment.NewLine;
            }
            return text;
        }
    }
}
