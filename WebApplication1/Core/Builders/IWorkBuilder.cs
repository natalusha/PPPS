using Core.Models;
using System.Collections.Generic;

namespace Core.Builders
{
    public interface IWorkBuilder : IBuilder<Work>
    {
        IWorkBuilder AddAuthor(Author author);

        IWorkBuilder AddAuthors(IList<Author> authors);

        IWorkBuilder AddGenre(Genre genre);
    }
}
