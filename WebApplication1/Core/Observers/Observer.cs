using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Observers
{
    public class Observer : IObserver
    {
        public delegate void EventHandler(object sedner, EventArgs e);

        private EventHandler handeler;

        private Observer()
        {

        }

        public Observer(EventHandler handeler)
        {
            this.handeler = handeler;
        }

        public void Handle(object obj)
        {
            handeler?.Invoke(obj, new EventArgs());
        }
    }
}
