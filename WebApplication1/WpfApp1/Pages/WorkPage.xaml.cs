using Core.Models;
using Data.Connections;
using Data.DAO;
using Data.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Configuration;
using System.Linq;
using WpfApp1.ViewModels;
using Core.Observers;

namespace WpfApp1.Pages
{
    /// <summary>
  
    /// </summary>
    public partial class WorkPage : Page
    {
        private readonly WorkDAO itemDAO;
        private readonly SqlServerConnection connection;
        private readonly Observer observer;

        public WorkPage()
        {
            InitializeComponent();

            connection = new ConnectionFactory(SqlServerConfiguration.Settings).GetConnection(typeof(SqlServerConnection));
            itemDAO = new DAOFactory(connection).GetConnection(typeof(WorkDAO)) as WorkDAO;

            observer = new Observer(GetAll_Click);
            itemDAO.AddObserver(observer);

            Loaded += Page_Load;
            Get_.Click += Get_Click;
            GetAll.Click += GetAll_Click;
            Add.Click += Add_Click;
            Update.Click += Update_Click;
            Delete.Click += Delete_Click;
        }

        private void Page_Load(object sender, RoutedEventArgs e)
        {
            var authors = (new DAOFactory(connection).GetConnection(typeof(AuthorDAO)) as AuthorDAO).GetAll();
            var genres = (new DAOFactory(connection).GetConnection(typeof(GenreDAO)) as GenreDAO).GetAll();

            foreach (var author in authors)
            {
                Authors.Items.Add(author);
            }

            foreach (var genre in genres)
            {
                Genres.Items.Add(genre);
            }
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {
            int? id = null;

            if (string.IsNullOrWhiteSpace(Id.Text))
            {
                id = null;
            }
            else
            {
                int idValue;
                var isNum = int.TryParse(Id.Text, out idValue);

                if (isNum)
                {
                    id = idValue;
                }
            }

            var works = itemDAO.Get(id,NamesW.SelectedItem as string,  Genres.SelectedItem as Genre, Authors.SelectedItem as Author);

            Works.Items.Clear();

            foreach (var work in works)
            {
                var authors = new StringBuilder();

                foreach (var author in work.Authors)
                {
                    authors.Append($"{author.Name}, ");
                }

                Works.Items.Add(new WorkViewModel()
                {
                    Id = work.Id,
                    Name=work.Name,
                    Genre = work.Genre,
                    Authors = authors.ToString()
                });
            }
        }

        private void GetAll_Click(object sender, EventArgs e)
        {
            var works = itemDAO.GetAll();
            Works.Items.Clear();

            foreach (var work in works)
            {
                var authors = new StringBuilder();

                foreach (var author in work.Authors)
                {
                    authors.Append($"{author.Name}, ");
                }

                Works.Items.Add(new WorkViewModel()
                {
                    Id = work.Id,
                    Name=work.Name,
                    Genre = work.Genre,
                    Authors = authors.ToString()
                });
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var work = new Work()
            {
                Genre = Genres.SelectedItem as Genre,
                Authors = new List<Author>()
            };
            var authorsList = Authors.SelectedItems;

            foreach (var author in authorsList)
            {
                work.Authors.Add(author as Author);
            }

            itemDAO.Add(work);

            GetAll_Click(sender, e);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Id.Text))
            {
                int id;

                if (int.TryParse(Id.Text, out id))
                {
                    var authors = new List<Author>();

                    foreach (var author in Authors.SelectedItems)
                    {
                        authors.Add(author as Author);
                    }

                    var work = new Work()
                    {
                        Genre = Genres.SelectedItem as Genre,
                        Authors = authors
                    };

                    itemDAO.Update(id, work);

                    GetAll_Click(sender, e);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Id.Text))
            {
                int id;

                if (int.TryParse(Id.Text, out id))
                {
                    itemDAO.Delete(id);

                    GetAll_Click(sender, e);
                }
            }
        }
    }
}
