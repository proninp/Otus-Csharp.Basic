namespace HomeWork11
{
    public class KeyValuePair
    {
        public int Key { get => _key; }
        private int _key;
        public string? Value { get; set; }
        public KeyValuePair(int key = 0, string? val = null)
        {
            _key = key;
            Value = val;
        }
        public override string ToString()
        {
            return $"[{_key}:\t{Value}]";
        }
    }
}
