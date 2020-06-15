using Core.Models;

namespace Core.Builders
{
    public class AuthorBuilder : IAuthorBuilder
    {
        private readonly Author author;

        public AuthorBuilder() : this(new Author())
        {
        }

        public AuthorBuilder(Author author)
        {
            this.author = author;
        }

        public IAuthorBuilder AddName(string name)
        {
            author.Name = name;

            return this;
        }

        public Author Build()
        {
            return author;
        }
    }
}
