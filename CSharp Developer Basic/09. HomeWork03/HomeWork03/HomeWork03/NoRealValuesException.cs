namespace HomeWork03
{
    public class NoRealValuesException: Exception
    {
        public NoRealValuesException() : base() { }
        public NoRealValuesException(string? message) : base(message) { }
        public NoRealValuesException(string? message, Exception innerException) : base(message, innerException) { }
    }
}