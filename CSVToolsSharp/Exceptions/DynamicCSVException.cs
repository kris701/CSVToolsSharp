namespace CSVToolsSharp.Exceptions
{
    /// <summary>
    /// An exception relevant to the <seealso cref="DynamicCSV"/> class
    /// </summary>
    public class DynamicCSVException : Exception
    {
        /// <summary>
        /// Data of the internal CSV document
        /// </summary>
        public Dictionary<string, List<string>> CSVData { get; set; }

        /// <summary>
        /// An exception relevant to the <seealso cref="DynamicCSV"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public DynamicCSVException(string? message, Dictionary<string, List<string>> cSVData) : base(message)
        {
            CSVData = cSVData;
        }
    }
}
