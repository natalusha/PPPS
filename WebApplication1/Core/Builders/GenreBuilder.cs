using Core.Models;
using System;

namespace Core.Builders
{
    public class GenreBuilder : IGenreBuilder
    {
        private readonly Genre genre;

        public GenreBuilder() : this(new Genre())
        {
        }

        public GenreBuilder(Genre genre)
        {
            this.genre = genre;
        }

        
        public IGenreBuilder AddName(string name)
        {
            genre.Name = name;

            return this;
        }

        public Genre Build()
        {
            return genre;
        }
    }
}
