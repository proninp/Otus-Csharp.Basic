namespace Task1
{
    public interface ISubject
    {
        public void RegisterObserver(IObserve observer);
        
        public void RemoveObserver(IObserve observer);
    }
}
