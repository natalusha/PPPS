using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1.ViewModels
{
    public class WorkViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Genre Genre { get; set; }

        public string Authors { get; set; }

    }
}
