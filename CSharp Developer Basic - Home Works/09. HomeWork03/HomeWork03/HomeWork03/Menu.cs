namespace HomeWork03
{
    public class Menu
    {
        private static int _bottomPositionGapLines = 2;
        private int _lastOtputCursorPosition = 0; // Запоминаем нижнюю позицию, чтобы до неё очищать консоль
        private MenuPlainLine _menuHeader;
        private MenuLine[] _menuLines = new MenuLine[3];
        private QuadEquasion _headerEquasion;
        private int _option = 0;
        public int Option
        {
            get => _option;
            set
            {
                _menuLines[_option].IsSelected = false;
                if (value < 0)
                    _option = _menuLines.Length - 1;
                else if (value >= _menuLines.Length)
                    _option = 0;
                else
                    _option = value;
                _menuLines[_option].IsSelected = true;
            }
        }
        public Menu(QuadEquasion equasion)
        {
            Console.CursorVisible = false;
            _headerEquasion = equasion;
            var i = 0;
            var positions = GetLinesPositions(_menuLines.Length + 1);
            
            _menuHeader = new MenuPlainLine(_headerEquasion.ToString(), positions[i], MenuLineColor.Cyan);
            _menuLines[i] = new MenuLine(Settings.ASign, true, positions[++i]);
            _menuLines[i] = new MenuLine(Settings.BSign, false, positions[++i]);
            _menuLines[i] = new MenuLine(Settings.CSign, false, positions[++i]);
            _lastOtputCursorPosition = positions[i];
        }
        /// <summary>
        /// Получение позиций строк меню в консоли перед первым запуском
        /// </summary>
        /// <param name="linesCount">Количество строк меню (включая заголовок)</param>
        /// <returns>Массив позиций строк меню</returns>
        private int[] GetLinesPositions(int linesCount)
        {
            var currentConsoleTopPosition = Console.CursorTop;
            var positions = new int[linesCount];
            for (int i = 0; i < linesCount; i++)
            {
                positions[i] = Console.CursorTop;
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, currentConsoleTopPosition);
            return positions;
        }
        /// <summary>
        /// Нижняя позиция курсора, котрая была в текущей итерации вывода меню/данных/ошибки
        /// </summary>
        /// <returns>Позиция курсора</returns>
        private int GetCursorBottomPosition()
        {
            var cursorBottomPosition = _menuLines[_menuLines.Length - 1].PositionTop + _bottomPositionGapLines;
            if (cursorBottomPosition < _lastOtputCursorPosition)
                cursorBottomPosition = _lastOtputCursorPosition;
            return cursorBottomPosition;
        }
        
        /// <summary>
        /// Установка позиции курсора ниже вывода меню
        /// </summary>
        private void SetMenuBottomPosition()
        {
            Console.CursorTop = _menuLines[_menuLines.Length - 1].PositionTop + _bottomPositionGapLines;
            Console.CursorLeft = 0;
        }
        public void AddSelected(char ch)
        {
            _menuLines[_option].Add(ch);
            _headerEquasion.Factors[_option].Add(ch);
        }
        public void BackspaceSelected()
        {
            _menuLines[_option].Backspace();
            _headerEquasion.Factors[_option].Backspace();
        }
        public void Show()
        {
            if (_menuHeader.IsReprintRequire || _menuLines.Any(x => x.IsReprintRequire))
            {
                _menuHeader.Text = _headerEquasion.ToString();
                _menuHeader.PrintLine();
            }
            for (int i = 0; i < _menuLines.Length; i++)
            {
                if (_menuLines[i].IsReprintRequire)
                    _menuLines[i].PrintLine();
            }
        }
        /// <summary>
        /// Установка курсора в позицию ввыбранной строки меню
        /// </summary>
        public void SetSelectedPosition()
        {
            _lastOtputCursorPosition = Console.CursorTop;
            var pos = 0;
            for (int i = 0; (i < _menuLines.Length) && (pos == 0); i++)
            {
                if (_menuLines[i].IsSelected)
                {
                    pos = _menuLines[i].PositionTop;
                    Console.CursorLeft = 0;
                    Console.CursorTop = pos;
                    _menuLines[i].PrintLine();
                }
            }
        }
        /// <summary>
        /// Функция, чтобы очистить область под меню и перевести курсор ниже уровня меню
        /// </summary>
        public void PrepareWindowOutput()
        {
            SetMenuBottomPosition();
            var line = new string(' ', Console.WindowWidth);
            var cursorBottomPosition = GetCursorBottomPosition();
            for (int i = Console.CursorTop; i < cursorBottomPosition; i++)
            {
                Console.CursorTop = i;
                Console.CursorLeft = 0;
                Console.Write(line);
            }
            SetMenuBottomPosition();
        }
        /// <summary>
        /// Функция используется для установки курсора в нижнюю позицию перед выходом из приложения
        /// </summary>
        public void Quit()
        {
            Console.CursorTop = GetCursorBottomPosition();
            Console.CursorLeft = 0;
        }
        /// <summary>
        /// Функция для репрезентации данных menu  - используем при форматировании в FormatData()
        /// </summary>
        /// <returns>Позиция строки, [Название коэфициента] = [Введенное значение]</returns>
        public IDictionary<int, string> GetMenuParams()
        {
            var map = new Dictionary<int, string>(_menuLines.Length);
            for (int i = 0; i < _menuLines.Length; i++)
            {
                var line = _menuLines[i];
                map.Add(i, $"{line.LinePrefix} = {line.Text}");
            }
            return map;
        }
    }
}
