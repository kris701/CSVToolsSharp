using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Tests.TestClasses
{
    public class Simple3
    {
        public string Hidden { get; set; } = "yes";
        [CSVColumn("Visible")]
        public string Value { get; set; }

        public Simple3() { }

        public Simple3(string value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Simple3 other)
            {
                if (other.Value != Value) return false;
                return true;
            }
            return false;
        }
    }
}
