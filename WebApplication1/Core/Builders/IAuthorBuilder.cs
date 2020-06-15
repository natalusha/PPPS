using Core.Models;

namespace Core.Builders
{
    public interface IAuthorBuilder : IBuilder<Author>
    {
        IAuthorBuilder AddName(string name);
    }
}
