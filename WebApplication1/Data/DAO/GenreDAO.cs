using Core.Models;
using Core.Observers;
using Data.Connections;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data.DAO
{
    public class GenreDAO : IObservable
    {
        private static GenreDAO genreDAO;

        private readonly SqlConnection connection;
        private readonly List<IObserver> observers;

        private GenreDAO()
        {
        }

        private GenreDAO(SqlServerConnection connection)
        {
            this.connection = connection.sqlConnection;
            observers = new List<IObserver>();
        }

        public static GenreDAO GetInstance(SqlServerConnection connection)
        {
            return genreDAO ?? (genreDAO = new GenreDAO(connection));
        }

        public IList<Genre> Get(int? id, string name)
        {
            var genres = new List<Genre>();
            var query = $"select Id, Name from Genres where";
            var hasFilter = false;

            if (id != null)
            {
                query += $" Id = {id}";
                hasFilter = true;
            }


            if (name != null)
            {
                if (hasFilter)
                {
                    query += " and";
                }

                query += $" Name like '{name}'";
                hasFilter = true;
            }

            if (!hasFilter)
            {
                return genres;
            }
            else
            {
                query += $";";
            }

            try
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        genres.Add(new Genre()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(2)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return genres;
        }

        public IList<Genre> GetAll()
        {
            var genres = new List<Genre>();
            var query = $"select Id, Name from Genres;";

            try
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var Id = reader.GetInt32(0);
                        var CreatonDate = reader.GetDateTime(1);
                        var Name = reader.GetString(2);

                        genres.Add(new Genre()
                        {
                            Id = reader.GetInt32(0),
                            
                            Name = reader.GetString(2)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return genres;
        }

        public void Add(Genre genre)
        {
            var query = $"insert into Genres ( Name) values ( '{genre.Name}');";

            try
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            Notify(genre);
        }

        public void Delete(int id)
        {
            var query = $"delete from Genres where Id = {id};";

            try
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            Notify(id);
        }

        public void Update(int id, Genre genre)
        {
            var query = $"update Genres set Name = '{genre.Name}' where Id = {id};";

            try
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            Notify(genre);
        }

        public void AddObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify(object obj)
        {
            foreach (var observer in observers)
            {
                observer.Handle(obj);
            }
        }
    }
}
