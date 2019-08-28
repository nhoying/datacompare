using System;
namespace DataCompare
{
    public class ComparisonSource
    {
        public ComparisonSource()
        {
        }

        /// <summary>
        /// Gets or sets sql provider
        /// </summary>
        public string SqlProvider { get; set; }

        public string ConnectionString { get; set; }

        public string Sql { get; set; }
    }
}
