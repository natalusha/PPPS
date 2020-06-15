using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Observers
{
    public interface IObserver
    {
        void Handle(object obj);
    }
}
