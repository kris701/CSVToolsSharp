using CSVToolsSharp.Exceptions;

namespace CSVToolsSharp
{
    /// <summary>
    /// A class representing a dynamic CSV document
    /// </summary>
    public class DynamicCSV
    {
        /// <summary>
        /// Amount of columns in the CSV object
        /// </summary>
        public int Columns => _data.Keys.Count;
        /// <summary>
        /// Amount of rows in the CSV object
        /// </summary>
        public int Rows { 
            get {
                if (_data.Keys.Count == 0)
                    return 0;
                return _data.Keys.Max(x => _data[x].Count);
            } 
        }
        internal Dictionary<string, List<string>> _data;

        /// <summary>
        /// A class representing a dynamic CSV document
        /// </summary>
        /// <param name="data">Initial dynamic data.</param>
        public DynamicCSV(Dictionary<string, List<string>> data)
        {
            _data = data;
            BufferRows(Rows);
        }

        private void BufferRows(int toRow)
        {
            foreach (var key in _data.Keys)
                while (_data[key].Count < toRow)
                    _data[key].Add("");
        }

        /// <summary>
        /// Get data from a specific cell
        /// </summary>
        /// <param name="column">Name of the column</param>
        /// <param name="row">Index of the row</param>
        /// <returns>Data in the cell</returns>
        /// <exception cref="DynamicCSVException"></exception>
        public string GetCell(string column, int row)
        {
            if (!_data.ContainsKey(column))
                throw new DynamicCSVException($"Attempted to get data from a non-existent column: '{column}'! Object has the following columns: {GetAllColumnNames()}");
            if (row > Rows)
                throw new DynamicCSVException($"Row value must be within row count! Attempted to get data fror row {row} but the CSV document only has {Rows} rows.");
            return _data[column][row];
        }

        /// <summary>
        /// Gets all the current columns in the CSV object
        /// </summary>
        /// <returns>A list of column names</returns>
        public List<string> GetColumns() => _data.Keys.ToList();

        /// <summary>
        /// Gets all the data from a single row in the CSV object
        /// </summary>
        /// <param name="row">Index of the row</param>
        /// <returns>A list of data for the given row</returns>
        /// <exception cref="DynamicCSVException"></exception>
        public List<string> GetRow(int row)
        {
            if (row > Rows)
                throw new DynamicCSVException($"Row value must be within row count! Attempted to get data fror row {row} but the CSV document only has {Rows} rows.");

            var data = new List<string>();

            foreach (var key in _data.Keys)
                data.Add(_data[key][row]);

            return data;
        }

        /// <summary>
        /// Adds an empty column to the CSV object
        /// </summary>
        /// <param name="column">Name of the column</param>
        public void AddColumn(string column)
        {
            if (!_data.ContainsKey(column))
            {
                _data.Add(column, new List<string>());
                while (_data[column].Count < Rows)
                    _data[column].Add("");
            }
        }

        /// <summary>
        /// Adds an empty row to the end of the CSV object
        /// </summary>
        public void AddRow()
        {
            foreach (var key in _data.Keys)
                _data[key].Add("");
        }

        /// <summary>
        /// Inserts a new piece of data into the CSV object
        /// </summary>
        /// <param name="column">Column to insert data into</param>
        /// <param name="row">Row to insert data into</param>
        /// <param name="data">Data to insert</param>
        public void Insert(string column, int row, string data)
        {
            if (!_data.ContainsKey(column))
                AddColumn(column);
            if (row >= Rows)
                BufferRows(row + 1);
            _data[column][row] = data;
        }

        /// <summary>
        /// Remove a given column from the CSV document
        /// </summary>
        /// <param name="column">Name of the column</param>
        /// <exception cref="DynamicCSVException"></exception>
        public void RemoveColumn(string column)
        {
            if (!_data.ContainsKey(column))
                throw new DynamicCSVException($"Attempted to remove a non-existent column: '{column}'! Object has the following columns: {GetAllColumnNames()}");
            _data.Remove(column);
        }

        private string GetAllColumnNames()
        {
            var columns = "";
            foreach (var col in _data.Keys)
                columns += $"{col}, ";
            return columns;
        }

        /// <summary>
        /// Remove a row from the CSV object
        /// </summary>
        /// <param name="row">Index of the row</param>
        /// <exception cref="DynamicCSVException"></exception>
        public void RemoveRow(int row)
        {
            if (row > Rows)
                throw new DynamicCSVException($"Row value must be within row count! Attempted to remove row {row} but the CSV document only has {Rows} rows.");
            foreach (var column in _data.Keys)
                _data[column].RemoveAt(row);
        }

        /// <summary>
        /// Gets a hashcode for the content of the CSV object
        /// </summary>
        /// <returns>A hash value</returns>
        public override int GetHashCode() => HashCode.Combine(_data);

        /// <summary>
        /// Checks if a given <seealso cref="DynamicCSV"/> object is equivalent to another.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if the object is equivalent to the current one</returns>
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
