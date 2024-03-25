using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Tests.TestClasses
{
    public class Simple1
    {
        [CSVColumn("Column1")]
        public string Value { get; set; }

        public Simple1()
        {
            Value = "";
        }

        public Simple1(string value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Simple1 other)
            {
                if (other.Value != Value) return false;
                return true;
            }
            return false;
        }
    }
}
