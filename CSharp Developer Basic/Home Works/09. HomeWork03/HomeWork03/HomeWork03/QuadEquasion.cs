using System.Text;

namespace HomeWork03
{
    public class QuadEquasion
    {
        private EquasionFactor[] _factors;
        public EquasionFactor[] Factors
        {
            get => _factors;
            set => _factors = value;
        }
        private double _x1 = 0.0;
        private double _x2 = 0.0;
        public QuadEquasion(): this(string.Empty, string.Empty, string.Empty) { }
        public QuadEquasion(string a, string b, string c)
        {
            _factors = new EquasionFactor[]
            {
                new EquasionFactor(a, Settings.ASign),
                new EquasionFactor(b, Settings.BSign),
                new EquasionFactor(c, Settings.CSign)
            };
        }
        private string GetEquasionText()
        {
            // a * x^2 + b * x + c = 0
            var equasion = new StringBuilder();

            equasion.Append(GetSignedCoefEquasionPart(_factors[0]));
            equasion.Append(GetSignedCoefEquasionPart(_factors[1]));
            equasion.Append(GetSignedCoefEquasionPart(_factors[2]));
            equasion.Append("= 0");
            return equasion.ToString();
        }
        private string GetSignedCoefEquasionPart(EquasionFactor equasionFactor)
        {
            StringBuilder part = new StringBuilder();
            string factor = equasionFactor.Factor;
            string sign = equasionFactor.DefaultValue.Equals(Settings.ASign) ? string.Empty : Settings.PlusSign;
            if (factor.Length > 0)
            {
                var c = factor[0].ToString();
                if (c.Equals(Settings.MinusSign))
                {
                    sign = Settings.MinusSign;
                    factor = factor[1..];
                }
                else
                {
                    if (c.Equals(Settings.PlusSign))
                        factor = factor[1..];
                }
            }
            part.Append(sign);
            if (!string.IsNullOrEmpty(sign))
                part.Append(Settings.Separator);

            if (string.IsNullOrEmpty(factor))
                factor = equasionFactor.DefaultValue;
            part.Append(factor);
            if (equasionFactor.DefaultValue.Equals(Settings.ASign))
                part.Append(" * x^2 ");
            if (equasionFactor.DefaultValue.Equals(Settings.BSign))
                part.Append(" * x");
            part.Append(Settings.Separator);
            return part.ToString();
        }
        private string SolveQuadEquasion()
        {
            foreach (var factor in _factors)
            {
                if (!factor.TryCastFactor())
                    factor.ThrowNumberException();
            }

            var a = _factors[0].Number;
            var b = _factors[1].Number;
            var c = _factors[2].Number;

            if (a == 0)
                throw new EquasionTypeException(Settings.EquasionTypeText);

            var result = string.Empty;

            var d = b * b - 4 * a * c; // D = b^2 - 4 * a * c
            if (d > 0)
            {
                _x1 = (-b + Math.Sqrt(d)) / 2.0 / a;
                _x2 = (-b - Math.Sqrt(d)) / 2.0 / a;
                result = $"x1 = {_x1}; x2 = {_x2}";
            }
            else if (d == 0)
            {
                _x1 = (-b) / 2.0 / a;
                result = $"x = {_x1}";
            }
            else
            {
                throw new NoRealValuesException(Settings.NoRealValuesText);
            }
            return result;
        }
        public override string ToString() => GetEquasionText();
        public bool IsFactorsAsNumbers()
        {
            foreach (var factor in _factors)
                if (!factor.IsNumber())
                    return false;
            return true;
        }
        public string TrySolve()
        {
            var result = string.Empty;
            try
            {
                result = SolveQuadEquasion();
            }
            catch(NoRealValuesException ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
    }
}
