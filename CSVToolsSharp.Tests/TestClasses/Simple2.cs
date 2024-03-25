using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToolsSharp.Tests.TestClasses
{
    public class Simple2
    {
        [CSVColumn("Column1")]
        public string Value { get; set; }

        [CSVColumn("other Column 2")]
        public double Value2 { get; set; }

        [CSVColumn("other Column 3")]
        public DateTime Value3 { get; set; }

        public Simple2() { }

        public Simple2(string value, double value2, DateTime value3)
        {
            Value = value;
            Value2 = value2;
            Value3 = value3;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Simple2 other)
            {
                if (other.Value != Value) return false;
                if (other.Value2 != Value2) return false;
                if (other.Value3 != Value3) return false;
                return true;
            }
            return false;
        }
    }
}
