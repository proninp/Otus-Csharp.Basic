using System.Collections;

namespace HomeWork11
{
    public class OtusDictionary: IEnumerable<KeyValuePair>
    {
        private KeyValuePair[] _bucket = new KeyValuePair[32];

        public string? this[int i]
        {
            get => Get(i);
            set => AddOrSet(i, value, true);
        }

        public void Add(int key, string? val) => AddOrSet(key, val, false);

        public string Get(int key)
        {
            if (TryGetNode(key, out KeyValuePair? node))
                return node.Value;
            throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary");
        }

        public bool TryGetValue(int key, out string? value)
        {
            value = TryGetNode(key, out KeyValuePair? node) ? node.Value : null;
            return value is not null;
        }

        public bool ContainsKey(int key) => TryGetNode(key, out KeyValuePair? node);

        private void AddOrSet(int key, string? val, bool isSet)
        {
            if (val == null)
                throw new ArgumentNullException(nameof(val));
            if (!IsFreeBin())
                Resize();
            int ind = FindInd(key, _bucket);
            if (_bucket[ind] == null)
            {
                _bucket[ind] = new KeyValuePair(key, val);
                return;
            }
            if (!isSet)
                throw new ArgumentException($"An item with the same key has already been added. Key: '{key}'");
            _bucket[ind].Value = val;
        }

        private bool TryGetNode(int key, out KeyValuePair? node)
        {
            node = null;
            int ind = FindInd(key, _bucket);
            if (_bucket[ind] is not null && _bucket[ind].Key == key)
            {
                node = _bucket[ind];
                return true;
            }
            return false;
        }

        private int FindInd(int key, KeyValuePair[] array)
        {
            int ind = key % array.Length;
            while (array[ind] is not null && array[ind].Key != key)
                ind = (ind + 1) % array.Length;
            return ind;
        }

        private bool IsFreeBin() => _bucket.Any(n => n is null);

        private void Resize()
        {
            var newBucket = new KeyValuePair[_bucket.Length * 2];
            for (int i = 0; i < _bucket.Length; i++)
            {
                if (_bucket[i] is not null)
                {
                    int ind = FindInd(_bucket[i].Key, newBucket);
                    newBucket[ind] = _bucket[i];
                }
            }
            _bucket = newBucket;
        }

        public IEnumerator<KeyValuePair> GetEnumerator()
        {
            foreach (var kvp in _bucket)
                if (kvp is not null)
                    yield return kvp;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}