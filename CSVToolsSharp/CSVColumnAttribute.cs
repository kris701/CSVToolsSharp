namespace CSVToolsSharp
{
    /// <summary>
    /// An attribute that describes what column a value should be under.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CSVColumnAttribute : Attribute
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An attribute that describes what column a value should be under.
        /// </summary>
        /// <param name="name"></param>
        public CSVColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
