﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Exceptions
{
    /// <summary>
    /// An exception relevant to the <seealso cref="DynamicCSV"/> class
    /// </summary>
    public class DynamicCSVException : Exception
    {
        /// <summary>
        /// An exception relevant to the <seealso cref="DynamicCSV"/> class
        /// </summary>
        /// <param name="message">Exception message</param>
        public DynamicCSVException(string? message) : base(message)
        {
        }
    }
}