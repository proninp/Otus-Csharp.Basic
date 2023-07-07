using System.Text;

namespace HomeWork03
{
    public class MenuPlainLine
    {
        protected int _positionTop = 0;
        public int PositionTop
        {
            get => _positionTop;
        }
        protected StringBuilder _text = new StringBuilder();
        public string Text
        {
            get => _text.ToString();
            set
            {
                _text.Clear();
                _text.Append(value);
                _isReprintRequire = true;
            }
        }
        protected MenuLineColor _color = MenuLineColor.Default;
        public virtual MenuLineColor Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    _isReprintRequire = true;
                }   
            }
        }
        protected bool _isReprintRequire;
        public bool IsReprintRequire { get => _isReprintRequire; }
        public MenuPlainLine(string lineText, int position): this(lineText, position, MenuLineColor.Default) { }
        public MenuPlainLine(string lineText, int position, MenuLineColor color)
        {
            _text.Append(lineText);
            _positionTop = position;
            _color = color;
            _isReprintRequire = true;
        }
        public void Add(char ch)
        {
            _text.Append(ch);
            _isReprintRequire = true;
        }
        public void Backspace()
        {
            if (_text.Length > 0)
            {
                _text.Length--;
                _isReprintRequire = true;
            }
        }
        public void ClearMenuLine()
        {
            if (Console.CursorLeft != 0 || Console.CursorTop != _positionTop)
                Console.SetCursorPosition(0, _positionTop);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, _positionTop);
            _isReprintRequire = true;
        }
        public void PrintLine()
        {
            ClearMenuLine();
            Console.Write(ToString());
            _isReprintRequire = false;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_text);
            AddColor(sb);
            return sb.ToString();
        }
        protected void AddColor(StringBuilder sb)
        {
            if (Color == MenuLineColor.Default)
                return;
            
            string color = Color == MenuLineColor.Default ? string.Empty : string.Format(Settings.ColorTemplate, (int)Color);
            sb.Insert(0, color);
            sb.Append(string.Format(Settings.ColorTemplate, (int)MenuLineColor.Default));
        }
    }
}
