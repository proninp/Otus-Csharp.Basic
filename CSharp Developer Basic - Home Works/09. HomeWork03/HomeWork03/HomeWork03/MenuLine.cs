using System.Text;

namespace HomeWork03
{
    public class MenuLine: MenuPlainLine
    {
        private string _linePrefix = ""; // "a", "b", "c"
        public string LinePrefix
        { 
            get => _linePrefix;
            private set => _linePrefix = value;
        }
        private bool _isSelected = false;
        public bool IsSelected 
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    _color = _isSelected ? MenuLineColor.Green : MenuLineColor.Default;
                    _isReprintRequire = true;
                }
            }
        }
        public MenuLine(string linePrefix, bool isSelected, int position): base("", position)
        {
            _linePrefix = linePrefix;
            IsSelected = isSelected;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (_isSelected)
                sb.Append(Settings.SelectedText);
            if (!string.IsNullOrEmpty(_linePrefix))
            {
                if (!_isSelected)
                    sb.Append(new string(' ', Settings.SelectedText.Length));
                sb.Append(_linePrefix).Append(": ");
            }
            sb.Append(_text);

            AddColor(sb);
            return sb.ToString();
        }
    }
}
