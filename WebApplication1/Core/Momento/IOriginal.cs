using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Momento
{
    public interface IOriginal
    {
        void SetMomento(Momento momento);

        Momento CreateMomento();
    }
}
