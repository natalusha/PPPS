using System;
using System.Data.SqlClient;

namespace Data.Connections
{
    public class SqlServerConnection
    {
        private static SqlServerConnection connection;

        public SqlConnection sqlConnection { get; private set; }

        private SqlServerConnection()
        {
        }

        private SqlServerConnection(string connectionString)
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlServerConnection GetInstance(string connectionString)
        {
            return connection ?? (connection = new SqlServerConnection(connectionString));
        }
    }
}
