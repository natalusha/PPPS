using Data.Settings;

namespace WpfApp1.Configuration
{
    public static class SqlServerConfiguration
    {
        private const string DataSource = "";
        private const string InitialCatalog = "";
        private const string UserID = "";
        private const string Password = "";

        public static SqlServerConntectionSettings Settings { 
            get
            {
                return new SqlServerConntectionSettings()
                {
                    DataSource = DataSource,
                    InitialCatalog = InitialCatalog,
                    UserID = UserID,
                    Password = Password
                };
            }
        }
    }
}
