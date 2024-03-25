using CSVToolsSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp
{
    /// <summary>
    /// Overall class to serialise and deserialise data into CSV format
    /// </summary>
    public static class CSVSerialiser
    {
        #region 
        /// <summary>
        /// Serialise a list of <typeparamref name="T"/> into a CSV string.
        /// </summary>
        /// <typeparam name="T">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</typeparam>
        /// <param name="data">List of data</param>
        /// <returns>A string in CSV format</returns>
        /// <exception cref="CSVSerialiserException"></exception>
        public static string Serialise<T>(List<T> data) where T : new()
        {
            var type = typeof(T);
            if (type.Name.ToLower() == "object")
            {
                if (data.Count == 0)
                    throw new CSVSerialiserException("Cannot serialise a 'object' type list with zero values! Type cannot be infered.");
                var target = data[0];
                if (target == null)
                    throw new CSVSerialiserException("First item in data list was null!");
                return Serialise(target.GetType(), data.Cast<dynamic>().ToList());
            }

            return Serialise(typeof(T), data.Cast<dynamic>().ToList());
        }

        /// <summary>
        /// Serialise a list of <paramref name="type"/> into a CSV string.
        /// </summary>
        /// <param name="type">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</param>
        /// <param name="data">List of data</param>
        /// <returns>A string in CSV format</returns>
        /// <exception cref="CSVSerialiserException"></exception>
        public static string Serialise(Type type, List<dynamic> data)
        {
            if (!type.GetConstructors().Any(x => x.GetParameters().Length == 0))
                throw new CSVSerialiserException($"Type '{type.Name}' does not contain an empty constructor!");

            var sb = new StringBuilder();
            var propInfo = type.GetProperties();
            var columnProps = GetColumnProperties(type);
            if (columnProps.Count == 0)
                throw new CSVSerialiserException($"Class '{type.Name}' does not have any {nameof(CSVColumnAttribute)} attributes!");

            var headerStr = "";
            foreach (var col in columnProps)
            {
                var attr = col.GetCustomAttribute<CSVColumnAttribute>();
                if (attr != null)
                    headerStr += $"{attr.Name},";
            }
            sb.AppendLine(RemoveTrailingComma(headerStr));

            foreach (var item in data)
            {
                var itemStr = "";
                foreach (var col in columnProps)
                    itemStr += $"{col.GetValue(item)},";
                sb.AppendLine(RemoveTrailingComma(itemStr));
            }

            return sb.ToString();
        }
        #endregion

        #region
        /// <summary>
        /// Deserialise a CSV string into a list of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</typeparam>
        /// <param name="text">A string in CSV format</param>
        /// <returns>List of data</returns>
        /// <exception cref="CSVDeserialiserException"></exception>
        public static List<T> Deserialise<T>(string text) where T : new() => Deserialise(typeof(T), text).Cast<T>().ToList();
        /// <summary>
        /// Deserialise a CSV string into a list of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">A class that has some <seealso cref="CSVColumnAttribute"/> attributes</param>
        /// <param name="text">A string in CSV format</param>
        /// <returns>List of data</returns>
        /// <exception cref="CSVDeserialiserException"></exception>
        public static List<dynamic> Deserialise(Type type, string text)
        {
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
            var cols = split[0].Split(',').ToList();
            cols.RemoveAll(x => x == "");
            if (cols.Count != columnProps.Count)
                throw new CSVDeserialiserException($"{type.Name} column count does not match the given text to deserialise!");

            for (int i = 1; i < split.Count; i++)
            {
                var line = split[i].Split(',').ToList();
                line.RemoveAll(x => x == "");
                var newItem = Activator.CreateInstance(type);
                if (newItem == null)
                    throw new CSVDeserialiserException($"New instance of type '{type.Name}' was null!");
                for (int j = 0; j < line.Count; j++)
                    columnProps[j].SetValue(newItem, Convert.ChangeType(line[j], columnProps[j].PropertyType));
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

        private static string RemoveTrailingComma(string text)
        {
            if (text.EndsWith(','))
                text = text.Substring(0, text.Length - 1);
            return text;
        }
    }
}
