namespace HomeWork03
{
    public static class Settings
    {
        public static string PlusSign => "+";
        public static string MinusSign => "-";
        public static string Separator => " ";
        public static string ASign => "a";
        public static string BSign => "b";
        public static string CSign => "c";
        public static string SelectedText => "> ";
        public static string ColorTemplate => "\u001b[{0}m";
        public static string IncorrectParameterText => "Неверный формат параметра {0}:";
        public static string AvailableInputRangeText => "Допустимый диапазон значений от {0} до {1}";
        public static string NoRealValuesText => "Вещественных значений не найдено";
        public static string EquasionTypeText => "Уравнение не является квадратным";
        public static string InstructionsText => "# Для перемещения по меню используйте стрелки вверх и вниз;" +
            "\nЗнак \"-\" меняет знак коэффициента;" +
            "\nДля удаления используйте Backspace;" +
            "\nДля расчета уравнения введите все три коэффициента и нажмите Enter;" +
            "\nДля выхода нажмите Esc\n";
    }
}
