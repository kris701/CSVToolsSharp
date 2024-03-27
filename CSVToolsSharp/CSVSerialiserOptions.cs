using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp
{
    /// <summary>
    /// Optional settings to give to the CSV serialiser/deserialiser
    /// </summary>
    public class CSVSerialiserOptions
    {
        /// <summary>
        /// If it should try and serialise nicely
        /// </summary>
        public bool PrettyOutput { get; set; } = false;

        /// <summary>
        /// What seperator to expect
        /// </summary>
        public char Seperator { get; set; } = ',';
    }
}
