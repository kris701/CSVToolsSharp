using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace CSVToolsSharp
{
    public class DynamicCSV
    {
        public int Columns => _data.Keys.Count;
        public int Rows => _data.Keys.Max(x => _data[x].Count);
        internal Dictionary<string, List<string>> _data;

        public DynamicCSV(Dictionary<string, List<string>> data)
        {
            _data = data;
        }

        public void Insert(string column, int row, string data)
        {
            if (!_data.ContainsKey(column))
            {
                _data.Add(column, new List<string>());
                while (_data[column].Count < Rows)
                    _data[column].Add("");
            }
            if (row >= Rows)
            {
                foreach(var key in  _data.Keys)
                    while (_data[key].Count < Rows)
                        _data[key].Add("");
            }
            _data[column][row] = data;
        }

        public void RemoveColumn(string column) => _data.Remove(column);

        public void RemoveRow(int row)
        {
            foreach (var column in _data.Keys)
                _data[column].RemoveAt(row);
        }

        public override bool Equals(object? obj)
        {
            if (obj is DynamicCSV other)
            {
                if (Columns != other.Columns) return false;
                if (Rows != other.Rows) return false;
                for (int i = 0; i < _data.Keys.Count; i++)
                {
                    if (_data.Keys.ElementAt(i) != other._data.Keys.ElementAt(i))
                        return false;
                    if (_data[_data.Keys.ElementAt(i)].Count != other._data[_data.Keys.ElementAt(i)].Count)
                        return false;
                    for (int j = 0; j < _data[_data.Keys.ElementAt(i)].Count; j++)
                        if (_data[_data.Keys.ElementAt(i)][j] != other._data[_data.Keys.ElementAt(i)][j])
                            return false;
                }
                return true;
            }
            return false;
        }
    }
}
