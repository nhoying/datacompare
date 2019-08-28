using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DataCompare
{
    public class DataCompare : IDisposable
    {
        public DataCompare(ComparisonSource left, ComparisonSource right)
        {
            this.Left = left;
            this.Right = right;

            this.LeftConnection = ConnectionFactory.CreateConnection(this.Left);
            this.RightConnection = ConnectionFactory.CreateConnection(this.Right);

            LeftCommand = this.LeftConnection.CreateCommand();
            LeftCommand.CommandText = Left.Sql;

            RightCommand = this.RightConnection.CreateCommand();
            RightCommand.CommandText = Right.Sql;
            
        }

        private ComparisonSource Left;
        private ComparisonSource Right;

        private DbConnection LeftConnection;
        private DbConnection RightConnection;

        private DbCommand LeftCommand;
        private DbCommand RightCommand;

        public void Dispose()
        {
            LeftCommand.Dispose();
            RightCommand.Dispose();

            LeftConnection.Dispose();
            RightConnection.Dispose();
        }

        public CompareResult CompareSources()
        {
            CompareResult result = new CompareResult();

            DataTable leftTable = new DataTable();
            leftTable.Load(LeftCommand.ExecuteReader());

            DataTable rightTable = new DataTable();
            rightTable.Load(RightCommand.ExecuteReader());

            Dictionary<string, string> leftHashes = GetHashDictionary(leftTable);

            Dictionary<string, string> rightHashes = GetHashDictionary(rightTable);



            return result;
        }

        private Dictionary<string, string> GetHashDictionary(DataTable table)
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            using (MD5 md5Hash = MD5.Create())
            {
                foreach (DataRow item in table.Rows)
                {
                    hashes.Add(item[0].ToString(), ConvertRowToMd5Hash(md5Hash, item));

                }
            }
            return hashes;
        }

        private string ConvertRowToMd5Hash(MD5 md5Hash, DataRow row)
        {
            byte[] data = md5Hash.ComputeHash(ConvertRowToByteArray(row));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private byte[] ConvertRowToByteArray(DataRow row)
        {
            byte[] byteArray = new byte[0];
            foreach(var item in row.ItemArray)
            {
                byteArray.Concat(Encoding.UTF8.GetBytes(item.ToString()));
            }
            return byteArray;
        }


    }
}
