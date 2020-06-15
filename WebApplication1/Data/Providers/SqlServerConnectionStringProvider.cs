using Data.Constants;
using Data.Settings;

namespace Data.Providers
{
    public class SqlServerConnectionStringProvider : IConnectionStringProvider
    {
        private readonly SqlServerConntectionSettings settigns;

        public SqlServerConnectionStringProvider(SqlServerConntectionSettings settigns)
        {
            this.settigns = settigns;
        }

        public string GetConnectionString()
        {
            return string.Format(SqlServerConnectionConstants.ConnectionString, settigns.DataSource, settigns.InitialCatalog, settigns.UserID, settigns.Password);
        }
    }
}
