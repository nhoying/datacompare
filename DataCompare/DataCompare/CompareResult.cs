using System;
using System.Collections.Generic;
using System.Text;

namespace DataCompare
{
    public class CompareResult
    {
        public CompareResult()
        {
        }

        public bool FullMatch { get; set; }

        public StringBuilder StringBuffer { get; set; }

        public Dictionary<string, string> ErrorCollection { get; set; }
    }
}
