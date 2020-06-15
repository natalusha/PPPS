using System.Collections.Generic;

namespace Core.Models
{
    public class Work
    {
        public int Id { get; set; }
      public string Name { get; set; }

        public IList<Author> Authors { get; set; }

        public Genre Genre { get; set; }
    }
}
