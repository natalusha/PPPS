using Core.Models;
using System.Collections.Generic;

namespace Core.Builders
{
    public class WorkBuilder : IWorkBuilder
    {
        private readonly Work work;

        public WorkBuilder() : this(new Work())
        {
        }

        public WorkBuilder(Work work)
        {
            this.work = work;
        }

        public IWorkBuilder AddAuthor(Author author)
        {
            work.Authors.Add(author);

            return this;
        }

        public IWorkBuilder AddAuthors(IList<Author> authors)
        {
            foreach (var author in authors)
            {
                work.Authors.Add(author);
            }

            return this;
        }

        public IWorkBuilder AddGenre(Genre genre)
        {
            work.Genre = genre;

            return this;
        }

        public Work Build()
        {
            return work;
        }
    }
}
