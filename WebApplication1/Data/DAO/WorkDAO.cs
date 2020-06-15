using Core.Models;
using Core.Observers;
using Data.Connections;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.DAO
{
    public class WorkDAO : IObservable
    {
        private static WorkDAO itemDAO;

        private readonly SqlConnection connection;
        private readonly List<IObserver> observers;

        private WorkDAO()
        {
        }

        private WorkDAO(SqlServerConnection connection)
        {
            this.connection = connection.sqlConnection;
            observers = new List<IObserver>();
        }

        public static WorkDAO GetInstance(SqlServerConnection connection)
        {
            return itemDAO ?? (itemDAO = new WorkDAO(connection));
        }

        public IList<Work> Get(int? id, string name, Genre genre, Author author)
        {
            var works = new List<Work>();
            var query1 = $"select  Works.Id,Works.Name, Genres.Id as GenreId, Name from Works FULL OUTER join Genres on Works.Genre_id = Genres.Id;";
            var query2 = "select Authors.Id, Authors.Name from WorkAuthor join Authors on Authors.Id = WorkAuthor.Author_id where WorkAuthor.Work_id = {0};";

            try
            {
                connection.Open();

                var command = new SqlCommand(query1, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0))
                        {
                            continue;
                        }

                        var work = new Work();

                        work.Id = reader.GetInt32(0);
                        work.Name = reader.GetString(4);
                        work.Genre = new Genre();

                        work.Genre.Id = reader.GetInt32(1);
                      
                        work.Genre.Name = reader.GetString(3);

                        works.Add(work);
                    }
                }

                connection.Close();

                for (var i = 0; i < works.Count; i++)
                {
                    var authors = new List<Author>();
                    var command2 = new SqlCommand(string.Format(query2, works[i].Id), connection);

                    connection.Open();

                    using (var reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            authors.Add(new Author()
                            {
                                Id = reader2.GetInt32(0),
                                Name = reader2.GetString(1)
                            });
                        }
                    }

                    works[i].Authors = authors;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            if (id != null)
            {
                works = works.Where(i => i.Id == id).ToList();
            }
            
            if (genre != null)
            {
                works = works.Where(i => i.Genre.Id == genre.Id).ToList();
            }

            if (author != null)
            {
                works = works.Where(i => i.Authors.FirstOrDefault(c => c.Id == author.Id) != null).ToList();
            }

            return works;
        }

        public IList<Work> GetAll()
        {
            var works = new List<Work>();
            var query1 = $"select  Works.Id, Works.Name, Genres.Id as GenreId, CreationDate, Name from Works FULL OUTER join Genres on Works.Genre_id = Genres.Id;";
            var query2 = "select Authors.Id, Authors.Name from WorkAuthor join Authors on Authors.Id = WorkAuthor.Author_id where WorkAuthor.Work_id = {0};";

            try
            {
                connection.Open();

                var command = new SqlCommand(query1, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0))
                        {
                            continue;
                        }

                        var work = new Work();

                        work.Id = reader.GetInt32(0);
                        work.Name = reader.GetString(4);
                        work.Genre = new Genre();

                        work.Genre.Id = reader.GetInt32(1);
                       
                        work.Genre.Name = reader.GetString(3);

                        works.Add(work);
                    }
                }

                connection.Close();

                for (var i = 0; i < works.Count; i++)
                {
                    var authors = new List<Author>();
                    var command2 = new SqlCommand(string.Format(query2, works[i].Id), connection);

                    connection.Open();

                    using (var reader2 = command2.ExecuteReader())
                    {
                        while (reader2.Read())
                        {
                            authors.Add(new Author()
                            {
                                Id = reader2.GetInt32(0),
                                Name = reader2.GetString(1)
                            });
                        }
                    }

                    works[i].Authors = authors;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return works;
        }

        public void Add(Work work)
        {
            var query1 = $"insert into Works (Genre_Id) values ({work.Genre.Id});SELECT SCOPE_IDENTITY();";
            var query2 = "insert into WorkAuthor (Work_id, Author_Id) values ({0}, {1});";

            try
            {
                connection.Open();

                var command = new SqlCommand(query1, connection);
                var id = (int)(decimal)command.ExecuteScalarAsync().Result;

                foreach (var author in work.Authors)
                {
                    command = new SqlCommand(string.Format(query2, id, author.Id), connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            Notify(work);
        }

        public void Delete(int id)
        {
            var query1 = $"delete from WorkAuthor where Work_id = {id};";
            var query2 = $"delete from Works where Id = {id};";

            try
            {
                connection.Open();

                var command = new SqlCommand(query1, connection);
                command.ExecuteNonQuery();

                command = new SqlCommand(query2, connection);
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

        public void Update(int id, Work work)
        {
            var query1 = $"update Works set Genre_id = {work.Genre.Id} where Id = {id};";
            var query2 = $"delete from WorkAuthor where Work_id = {id};";
            var query3 = "insert into WorkAuthor (Work_id, Author_Id) values ({0}, {1});";

            try
            {
                connection.Open();

                var command = new SqlCommand(query1, connection);
                command.ExecuteNonQuery();

                command = new SqlCommand(query2, connection);
                command.ExecuteNonQuery();

                foreach (var author in work.Authors)
                {
                    command = new SqlCommand(string.Format(query3, id, author.Id), connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            Notify(work);
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
