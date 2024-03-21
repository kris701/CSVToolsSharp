using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp
{
    /// <summary>
    /// Internal data context for a CSV file
    /// </summary>
    public class CSVContext
    {
        private int _row = 0;
        /// <summary>
        /// The row to append data to
        /// </summary>
        public int Row
        {
            get => _row;
            set
            {
                _row = value;
                foreach (var key in _csvData.Keys)
                    while (_csvData[key].Count <= _row)
                        _csvData[key].Add("");
            }
        }

        /// <summary>
        /// Amount of columns in the context
        /// </summary>
        public int Columns => _csvData.Count;
        /// <summary>
        /// Amount of rows in the context
        /// </summary>
        public int Rows
        {
            get
            {
                if (_csvData.Count == 0) return 0;
                return _csvData[_csvData.Keys.First()].Count;
            }
        }

        private Dictionary<string, List<string>> _csvData = new Dictionary<string, List<string>>();

        /// <summary>
        /// Load data into the CSV context.
        /// </summary>
        /// <param name="text"></param>
        public void LoadFromString(string text)
        {
            Reset();
            var lines = text.Split('\n');
            var headers = lines[0].Split(',').ToList();
            foreach (var header in headers)
                _csvData.Add(header, new List<string>());
            foreach (var line in lines.Skip(1))
            {
                var split = line.Split(',').ToList();
                for (int i = 0; i < split.Count; i++)
                    _csvData[headers[i]].Add(split[i]);
            }
            _row = lines.Length - 1;
        }

        /// <summary>
        /// Reset the data context
        /// </summary>
        public void Reset()
        {
            _csvData = new Dictionary<string, List<string>>();
            _row = 0;
        }

        /// <summary>
        /// Adds a blank column to the CSV context
        /// </summary>
        /// <param name="col"></param>
        public void AddColumn(string col)
        {
            _csvData.Add(col, new List<string>());
            while (_csvData[col].Count <= _row)
                _csvData[col].Add("");
        }

        /// <summary>
        /// Append a string value to a column in the CSV context.
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
        }

        /// <summary>
        /// Get the data in a column
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public string Get(string col) => _csvData[col][Row];

        /// <summary>
        /// Outputs the CSV context as a text string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var text = "";
            foreach (var col in _csvData.Keys)
                text += $"{col},";
            text = text.Remove(text.Length - 1);
            text += '\n';

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
                if (i != maxRow - 1)
                    text += '\n';
            }
            return text;
        }
    }
}
