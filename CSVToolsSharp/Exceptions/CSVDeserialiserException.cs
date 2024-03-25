using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Exceptions
{
    public class CSVDeserialiserException : Exception
    {
        public CSVDeserialiserException(string? message) : base(message)
        {
        }
    }
}
