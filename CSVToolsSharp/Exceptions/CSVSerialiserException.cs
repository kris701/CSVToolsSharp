using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Exceptions
{
    public class CSVSerialiserException : Exception
    {
        public CSVSerialiserException(string? message) : base(message)
        {
        }
    }
}
