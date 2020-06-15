namespace Core.Observers
{
    public interface IObservable
    {
        void AddObserver(IObserver observer);

        void RemoveObserver(IObserver observer);

        void Notify(object obj);
    }
}
