using System;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace DataCompare
{
    public static class ConnectionFactory
    {
        public static DbConnection CreateConnection(ComparisonSource source)
        {
            DbConnection connection = null;

            switch (source.SqlProvider)
            {
                case "MySql":
                    connection = new MySqlConnection(source.ConnectionString);
                    break;
                case "SqlServer":
                    connection = new SqlConnection(source.ConnectionString);
                    break;
                default:
                    throw new NotImplementedException(source.SqlProvider);
            }

            return connection;
        }
    }
}
