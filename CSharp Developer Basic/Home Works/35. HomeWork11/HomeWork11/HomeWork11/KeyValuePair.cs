namespace HomeWork11
{
    public class KeyValuePair
    {
        public int Key { get; set; }
        public string? Value { get; set; }
        public KeyValuePair(int key = 0, string? val = null)
        {
            Key = key;
            Value = val;
        }
    }
}
