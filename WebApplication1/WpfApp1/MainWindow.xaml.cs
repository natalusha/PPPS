using System;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
   
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += Window_Loaded;
            Works.Click += Works_Click;
            Genres.Click += Genres_Click;
            Authors.Click += Authors_Click;
        }

        private void Window_Loaded(object sender, EventArgs args)
        {
            Works_Click(sender, args);
        }

        private void Genres_Click(object sedner, EventArgs args)
        {
            MainFrame.Navigate(new Uri("Pages/GenrePage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Works_Click(object sedner, EventArgs args)
        {
            MainFrame.Navigate(new Uri("Pages/WorkPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Authors_Click(object sedner, EventArgs args)
        {
            MainFrame.Navigate(new Uri("Pages/AuthorPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Works_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
