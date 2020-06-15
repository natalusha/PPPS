using Core.Models;
using Core.Momento;
using Core.Observers;
using Data.Connections;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.DAO
{
    public class AuthorDAO : IObservable
    {
        private static AuthorDAO authorDAO;

        private readonly SqlConnection connection;
        private readonly List<IObserver> observers;
        private readonly IDictionary<int, Stack<CareTaker>> careTakers;

        private AuthorDAO()
        {
        }

        private AuthorDAO(SqlServerConnection connection)
        {
            this.connection = connection.sqlConnection;
            observers = new List<IObserver>();
            careTakers = new Dictionary<int, Stack<CareTaker>>();
        }

        public static AuthorDAO GetInstance(SqlServerConnection connection)
        {
            return authorDAO ?? (authorDAO = new AuthorDAO(connection));
        }

        public IList<Author> Get(int? id, string name)
        {
            var authors = new List<Author>();
            var query = $"select Id, Name from Authors where";
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
                return authors;
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
                        authors.Add(new Author()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
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

            return authors;
        }

        public IList<Author> GetAll()
        {
            var authors = new List<Author>();
            var query = $"select Id, Name from Authors;";


            try
            {
                connection.Open();

                var command = new SqlCommand(query, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        authors.Add(new Author()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
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

            return authors;
        }

        public void Add(Author author)
        {
            var query = $"insert into Authors (Name) values ('{author.Name}');";

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

            Notify(author);
        }

        public void Delete(int id)
        {
            try
            {
                careTakers.Remove(id);
            }
            catch
            {
            }

            var query = $"delete from Authors where Id = {id};";

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

        public void Update(int id, Author author)
        {
            if (!AddMomento(id))
            {
                return;
            }

            UpdateCreator(id, author);

            Notify(author);
        }

        private void UpdateCreator(int id, Author author)
        {
            var query = $"update Authors set Name = '{author.Name}' where Id = {id};";

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
        }

        private bool AddMomento(int id)
        {
            var oldAuthor = Get(id, null).FirstOrDefault();

            if (oldAuthor == null)
            {
                return false;
            }

            var careTaker = new CareTaker()
            {
                Momento = new Momento(oldAuthor.Id, oldAuthor.Name)
            };

            try
            {
                careTakers[id].Push(careTaker);
            }
            catch
            {
                careTakers.Add(id, new Stack<CareTaker>(new List<CareTaker>()
                {
                    careTaker
                }));
            }

            return true;
        }

        public void Restore(int id)
        {
            try
            {
                if (careTakers[id].Count > 0)
                {
                    var careTaker = careTakers[id].Pop();
                    var author = new Author();
                    author.SetMomento(careTaker.Momento);

                    UpdateCreator(id, author);

                    Notify(id);
                }
            }
            catch
            {
            }
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
