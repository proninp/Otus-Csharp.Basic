using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Task1
{
    public class Shop : ISubject
    {
        private ObservableCollection<Item> _items;
        private List<IObserve> _observers;
        public Shop()
        {
            _observers = new List<IObserve>();
            _items = new ObservableCollection<Item>();
            _items.CollectionChanged += Items_CollectionChanged;
        }

        public void RegisterObserver(IObserve observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserve observer)
        {
            _observers.Remove(observer);
        }

        public void Add(string name)
        {
            Add(new Item(_items.Count + 1, name));
        }

        public void Add(Item item)
        {
            _items.Add(item);
        }

        public void Remove(int id)
        {
            var item = _items.Where(x => x.Id == id).FirstOrDefault();
            if (item != null)
                Remove(item);
        }

        public void Remove(Item item)
        {
            _items.Remove(item);
        }

        private void Items_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _observers.ForEach(o => o.OnItemChanged($"Добавлен товар. {e?.NewItems?[0]}"));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    _observers.ForEach(o => o.OnItemChanged($"Удален товар. {e?.OldItems?[0]}"));
                    break;
            }
        }

    }
}
