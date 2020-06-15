using Core.Models;
using Core.Momento;
using Core.Observers;
using Data.Connections;
using Data.DAO;
using Data.Factories;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Configuration;

namespace WpfApp1.Pages
{
    /// <summary>
  
    /// </summary>
    public partial class AuthorPage : Page
    {
        private readonly AuthorDAO creatorDAO;
        private readonly Observer observer;

        public AuthorPage()
        {
            InitializeComponent();

            var connection = new ConnectionFactory(SqlServerConfiguration.Settings).GetConnection(typeof(SqlServerConnection));
            creatorDAO = new DAOFactory(connection).GetConnection(typeof(AuthorDAO)) as AuthorDAO;

            observer = new Observer(GetAll_Click);
            creatorDAO.AddObserver(observer);

            Get_.Click += Get_Click;
            GetAll.Click += GetAll_Click;
            Add.Click += Add_Click;
            Update.Click += Update_Click;
            Delete.Click += Delete_Click;
            Restore.Click += Restore_Click;
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            int id;

            if (int.TryParse(Id.Text, out id))
            {
                creatorDAO.Restore(id);
            }
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

            var authors = creatorDAO.Get(id, text);

            foreach (var author in authors)
            {
                Authors.Items.Add(author);
            }
        }

        private void GetAll_Click(object sender, EventArgs e)
        {
            var authors = creatorDAO.GetAll();
            Authors.Items.Clear();

            foreach (var author in authors)
            {
                Authors.Items.Add(author);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Name.Text))
            {
                var author = new Author()
                {
                    Name = Name.Text
                };

                creatorDAO.Add(author);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Name.Text) && !string.IsNullOrWhiteSpace(Id.Text))
            {
                int id;

                if (int.TryParse(Id.Text, out id))
                {
                    var author = new Author()
                    {
                        Name = Name.Text
                    };

                    creatorDAO.Update(id, author);
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
                    creatorDAO.Delete(id);
                }
            }
        }
    }
}
