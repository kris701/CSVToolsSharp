using CSVToolsSharp.Exceptions;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CSVToolsSharp
{
    /// <summary>
    /// Overall class to serialise and deserialise data into CSV format
    /// </summary>
    public static class CSVSerialiser
    {
        #region Dynamic CSV

        /// <summary>
        /// Serialise a <seealso cref="DynamicCSV"/> object to CSV format
        /// </summary>
        /// <param name="data">A dynamic CSV object</param>
        /// <param name="options">Settings for the serialisation</param>
        /// <returns>A string in CSV format</returns>
        public static string Serialise(DynamicCSV data, CSVSerialiserOptions? options = null)
        {
            if (options == null)
                options = new CSVSerialiserOptions();

            var sb = new StringBuilder();
            var spacings = Enumerable.Repeat(0, data._data.Keys.Count).ToList();
            sb.AppendLine(SerialiseLine(data._data.Keys.ToList(), spacings, options));
            for (int i = 0; i < data.Rows; i++)
            {
                var line = new List<string>();
                foreach (var key in data._data.Keys)
                    line.Add(data._data[key][i]);
                sb.AppendLine(SerialiseLine(line, spacings, options));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Deserialise a CSV string into a <seealso cref="DynamicCSV"/> object.
        /// </summary>
        /// <param name="text">A string in CSV format</param>
        /// <param name="options">Settings for the deserialisation</param>
        /// <returns>A new instance of a <seealso cref="DynamicCSV"/> object</returns>
        public static DynamicCSV Deserialise(string text, CSVSerialiserOptions? options = null)
        {
            if (options == null)
                options = new CSVSerialiserOptions();
            text = text.Replace("\r", "");

            var data = new Dictionary<string, List<string>>();

            var lines = text.Split('\n').ToList();
            lines.RemoveAll(x => x == "");
            var headers = lines[0].Split(options.Seperator).ToList();
            headers.RemoveAll(x => x == "");
            foreach (var header in headers)
                data.Add(header, new List<string>());

            for(int i = 1; i < lines.Count; i++)
            {
                var lineData = lines[i].Split(options.Seperator).ToList();
                lineData.RemoveAll(x => x == "");
                for (int j = 0; j < data.Keys.Count; j++)
                    data[data.Keys.ElementAt(j)].Add(lineData[j]);
            }
            
            return new DynamicCSV(data);
        }

        #endregion

        #region Serialise
        /// <summary>
        /// Serialise a list of <typeparamref name="T"/> into a CSV string.
        /// </summary>
        /// <typeparam name="T">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</typeparam>
        /// <param name="data">List of data</param>
        /// <param name="options">Settings for the serialisation</param>
        /// <returns>A string in CSV format</returns>
        /// <exception cref="CSVSerialiserException"></exception>
        public static string Serialise<T>(List<T> data, CSVSerialiserOptions? options = null) where T : new()
        {
            var type = typeof(T);
            if (type.Name.ToLower() == "object")
            {
                if (data.Count == 0)
                    throw new CSVSerialiserException("Cannot serialise a 'object' type list with zero values! Type cannot be infered.");
                var target = data[0];
                if (target == null)
                    throw new CSVSerialiserException("First item in data list was null!");
                return Serialise(data.Cast<dynamic>().ToList(), target.GetType(), options);
            }

            return Serialise(data.Cast<dynamic>().ToList(), typeof(T), options);
        }

        /// <summary>
        /// Serialise a list of <paramref name="type"/> into a CSV string.
        /// </summary>
        /// <param name="type">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</param>
        /// <param name="data">List of data</param>
        /// <param name="options">Settings for the serialisation</param>
        /// <returns>A string in CSV format</returns>
        /// <exception cref="CSVSerialiserException"></exception>
        public static string Serialise(List<dynamic> data, Type type, CSVSerialiserOptions? options = null)
        {
            if (options == null)
                options = new CSVSerialiserOptions();

            if (!type.GetConstructors().Any(x => x.GetParameters().Length == 0))
                throw new CSVSerialiserException($"Type '{type.Name}' does not contain an empty constructor!");

            var propInfo = type.GetProperties();
            var columnProps = GetColumnProperties(type);
            if (columnProps.Count == 0)
                throw new CSVSerialiserException($"Class '{type.Name}' does not have any {nameof(CSVColumnAttribute)} attributes!");

            if (options.PrettyOutput == true)
                return SerializePretty(data, columnProps, options);
            return SerializeNormal(data, columnProps, Enumerable.Repeat(0, columnProps.Count).ToList(), options);
        }

        private static string SerializeNormal(List<dynamic> data, List<PropertyInfo> columnProps, List<int> spacings, CSVSerialiserOptions options)
        {
            var sb = new StringBuilder();
            var headers = new List<string>();
            for(int i = 0; i < columnProps.Count; i++)
            {
                var attr = columnProps[i].GetCustomAttribute<CSVColumnAttribute>();
                if (attr != null)
                    headers.Add(attr.Name);
            }
            sb.AppendLine(SerialiseLine(headers, spacings, options));

            foreach (var item in data)
            {
                var lineData = new List<string>();
                for (int i = 0; i < columnProps.Count; i++)
                    lineData.Add($"{columnProps[i].GetValue(item)}");
                sb.AppendLine(SerialiseLine(lineData, spacings, options));
            }

            return sb.ToString();
        }

        private static string SerialiseLine(List<string> data, List<int> spacings, CSVSerialiserOptions options)
        {
            var headerStr = "";
            for (int i = 0; i < data.Count; i++)
                headerStr += $"{data[i]}".PadRight(spacings[i]) + options.Seperator;
            headerStr = RemoveTrailingSeperator(headerStr, options.Seperator);
            return headerStr;
        }

        private static string SerializePretty(List<dynamic> data, List<PropertyInfo> columnProps, CSVSerialiserOptions options)
        {
            var spacings = new List<int>();
            foreach (var col in columnProps)
            {
                var attr = col.GetCustomAttribute<CSVColumnAttribute>();
                if (attr != null)
                    spacings.Add(attr.Name.Length);
            }

            foreach (var item in data)
            {
                for (int i = 0; i < columnProps.Count; i++)
                {
                    var str = $"{columnProps[i].GetValue(item)}";
                    if (str.Length > spacings[i])
                        spacings[i] = str.Length;
                }
            }
            return SerializeNormal(data, columnProps, spacings, options);
        }
        #endregion

        #region Deserialise
        /// <summary>
        /// Deserialise a CSV string into a list of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</typeparam>
        /// <param name="text">A string in CSV format</param>
        /// <param name="options">Settings for the deserialisation</param>
        /// <returns>List of data</returns>
        /// <exception cref="CSVDeserialiserException"></exception>
        public static List<T> Deserialise<T>(string text, CSVSerialiserOptions? options = null) where T : new() => Deserialise(text, typeof(T), options).Cast<T>().ToList();
        /// <summary>
        /// Deserialise a CSV string into a list of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</param>
        /// <param name="text">A string in CSV format</param>
        /// <param name="options">Settings for the deserialisation</param>
        /// <returns>List of data</returns>
        /// <exception cref="CSVDeserialiserException"></exception>
        public static List<dynamic> Deserialise(string text, Type type, CSVSerialiserOptions? options = null)
        {
            if (options == null)
                options = new CSVSerialiserOptions();

            if (!type.GetConstructors().Any(x => x.GetParameters().Length == 0))
                throw new CSVDeserialiserException($"Type '{type.Name}' does not contain an empty constructor!");
            text = text.Replace("\r", "");

            var retList = new List<dynamic>();
            var propInfo = type.GetProperties();
            var columnProps = GetColumnProperties(type);

            var split = text.Split('\n').ToList();
            split.RemoveAll(x => x == "");
            if (split.Count == 0)
                return new List<dynamic>();
            var cols = split[0].Split(options.Seperator).ToList();
            cols.RemoveAll(x => x == "");
            if (cols.Count != columnProps.Count)
                throw new CSVDeserialiserException($"{type.Name} column count does not match the given text to deserialise!");

            for (int i = 1; i < split.Count; i++)
            {
                var line = split[i].Split(options.Seperator).ToList();
                line.RemoveAll(x => x == "");
                var newItem = Activator.CreateInstance(type);
                if (newItem == null)
                    throw new CSVDeserialiserException($"New instance of type '{type.Name}' was null!");
                if (line.Count != columnProps.Count)
                    throw new CSVDeserialiserException($"Line {i} should have {columnProps.Count} fields but it had {line.Count}");
                for (int j = 0; j < line.Count; j++)
                {
                    var targetProp = columnProps.First(x => x.GetCustomAttribute<CSVColumnAttribute>()?.Name == cols[j].Trim());
                    if (targetProp != null)
                        targetProp.SetValue(newItem, Convert.ChangeType(line[j].Trim(), targetProp.PropertyType));
                }
                retList.Add(newItem);
            }

            return retList;
        }
        #endregion

        private static List<PropertyInfo> GetColumnProperties(Type type)
        {
            var propInfo = type.GetProperties();
            var columnProps = new List<PropertyInfo>();
            foreach (var prop in propInfo)
            {
                var attr = prop.GetCustomAttributes(false);
                if (attr.Any(x => x is CSVColumnAttribute))
                    columnProps.Add(prop);
            }
            return columnProps;
        }

        private static string RemoveTrailingSeperator(string text, char seperator)
        {
            if (text.EndsWith(seperator))
                text = text.Substring(0, text.Length - 1);
            return text;
        }
    }
}
