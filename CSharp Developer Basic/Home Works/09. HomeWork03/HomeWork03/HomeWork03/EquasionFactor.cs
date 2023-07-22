using System.Numerics;
using System.Text;

namespace HomeWork03
{
    public class EquasionFactor
    {
        private readonly string _defaultValue;
        public string DefaultValue { get => _defaultValue; }
        private StringBuilder _factor = new StringBuilder();
        public string Factor
        {
            get => _factor.ToString();
            set
            {
                _factor.Clear();
                if (!string.IsNullOrEmpty(value))
                {
                    _factor.Append(value);
                }
                TryCastFactor();
            }
        }
        private int _number = 0;
        public int Number 
        {
            get => _number;
        }
        public InvalidCastException? CastException { get; private set; } = null;
        public OutOfIntegerRangeException? OutOfIntegerException { get; private set; } = null;
        public EquasionFactor(string value, string defaultValue)
        {
            Factor = value;
            _defaultValue = defaultValue;
        }
        public bool IsNumber() => (CastException == null && OutOfIntegerException == null);
        public bool TryCastFactor()
        {
            CastException = null;
            OutOfIntegerException = null;
            var factor = _factor.ToString();
            if (!BigInteger.TryParse(factor, out BigInteger bigInteger))
                CastException = new InvalidCastException(string.Format(Settings.IncorrectParameterText, _defaultValue));
            else
            {
                if (!(bigInteger <= int.MaxValue && bigInteger >= int.MinValue))
                {
                    var message = new StringBuilder(string.Format(Settings.IncorrectParameterText, _defaultValue))
                        .Append(Environment.NewLine)
                        .Append(string.Format(Settings.AvailableInputRangeText, int.MinValue, int.MaxValue));
                    OutOfIntegerException = new OutOfIntegerRangeException(message.ToString());
                }
                else
                {
                    if (!int.TryParse(factor, out _number))
                        CastException = new InvalidCastException(string.Format(Settings.IncorrectParameterText, _defaultValue));
                }
            }
            return IsNumber();
        }
        public override string ToString() => _factor.ToString();
        public void Add(char ch) => _factor.Append(ch);
        public void Backspace()
        {
            if (_factor.Length > 0)
                _factor.Length--;
        }
        public void ThrowNumberException()
        {
            if (CastException != null)
                throw CastException;
            if (OutOfIntegerException != null)
                throw OutOfIntegerException;
        }
    }
}
