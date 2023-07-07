namespace HomeWork03
{
    public class OutOfIntegerRangeException : Exception
    {
        public OutOfIntegerRangeException() : base() { }
        public OutOfIntegerRangeException(string? message) : base(message) { }
        public OutOfIntegerRangeException(string? message, Exception innerException) : base(message, innerException) { }
    }
}