using Data.Connections;
using Data.DAO;
using System;

namespace Data.Factories
{
    public class DAOFactory
    {
        private readonly SqlServerConnection connection;

        public DAOFactory(SqlServerConnection connection)
        {
            this.connection = connection;
        }

        public object GetConnection(Type type)
        {
            if (type == typeof(AuthorDAO))
            {
                return AuthorDAO.GetInstance(connection);
            }
            else if (type == typeof(WorkDAO))
            {
                return WorkDAO.GetInstance(connection);
            }
            else if (type == typeof(GenreDAO))
            {
                return GenreDAO.GetInstance(connection);
            }

            return null;
        }
    }
}
