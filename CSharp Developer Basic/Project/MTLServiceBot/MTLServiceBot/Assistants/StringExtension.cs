namespace MTLServiceBot.Assistants
{
    public static class StringExtension
    {
        public static string PrepareMarkDown(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            if (!text.Contains("_"))
                return text;
            return text.Replace("_", "\\_");
        }
    }
}
