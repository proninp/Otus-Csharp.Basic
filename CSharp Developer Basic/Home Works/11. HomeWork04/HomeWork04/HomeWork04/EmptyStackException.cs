using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork4
{
    public class EmptyStackException: Exception
    {
        public EmptyStackException() : base() { }
        public EmptyStackException(string? message) : base(message) { }
        public EmptyStackException(string? message, Exception innerException) : base(message, innerException) { }
    }
}
