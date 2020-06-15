using Core.Models;
using Core.Observers;
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

namespace WpfApp1.Pages
{
    /// <summary>
   
    /// </summary>
    public partial class GenrePage : Page
    {
        private readonly GenreDAO genreDAO;
        private readonly Observer observer;

        public GenrePage()
        {
            InitializeComponent();

            var connection = new ConnectionFactory(SqlServerConfiguration.Settings).GetConnection(typeof(SqlServerConnection));
            genreDAO = new DAOFactory(connection).GetConnection(typeof(GenreDAO)) as GenreDAO;

            observer = new Observer(GetAll_Click);
            genreDAO.AddObserver(observer);

            Get_.Click += Get_Click;
            GetAll.Click += GetAll_Click;
            Add.Click += Add_Click;
            Update.Click += Update_Click;
            Delete.Click += Delete_Click;
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {
            int? id = null;
            Authors.Items.Clear();

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

            var text = string.IsNullOrWhiteSpace(Name.Text) ? null : Name.Text;

            var creators = genreDAO.Get(id, text);

            foreach (var creator in creators)
            {
                Authors.Items.Add(creator);
            }
        }

        private void GetAll_Click(object sender, EventArgs e)
        {
            var creators = genreDAO.GetAll();
           Authors.Items.Clear();

            foreach (var creator in creators)
            {
                Authors.Items.Add(creator);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Name.Text))
            {
                              

                var genre = new Genre()
                {
                   
                    Name = Name.Text
                };

                genreDAO.Add(genre);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Name.Text) && !string.IsNullOrWhiteSpace(Id.Text))
            {
                int id;

                if (int.TryParse(Id.Text, out id))
                {
                    

                    var genre = new Genre()
                    {
                        
                        Name = Name.Text
                    };

                    genreDAO.Update(id, genre);
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
                    genreDAO.Delete(id);
                }
            }
        }

        private void Authors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
