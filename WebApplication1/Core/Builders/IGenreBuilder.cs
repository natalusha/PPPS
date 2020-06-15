using Core.Models;
using System;

namespace Core.Builders
{
    public interface IGenreBuilder : IBuilder<Genre>
    {
        

        IGenreBuilder AddName(string name);
    }
}
