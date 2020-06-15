using Data.Connections;
using Data.Providers;
using Data.Settings;
using System;

namespace Data.Factories
{
    public class ConnectionFactory
    {
        private readonly string connectionString;

        public ConnectionFactory(SqlServerConntectionSettings conntectionSettings)
        {
            connectionString = new SqlServerConnectionStringProvider(conntectionSettings).GetConnectionString();
        }

        public SqlServerConnection GetConnection(Type type)
        {
            if (type == typeof(SqlServerConnection))
            {
                return SqlServerConnection.GetInstance(connectionString);
            }

            return null;
        }
    }
}
