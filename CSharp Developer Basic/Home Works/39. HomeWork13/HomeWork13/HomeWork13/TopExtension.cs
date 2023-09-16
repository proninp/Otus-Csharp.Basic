namespace HomeWork13
{
    public static class TopExtension
    {
        public static IEnumerable<T> Top<T>(this IEnumerable<T> collection, int percent)
        {
            CheckPercentage(percent);
            var elementsCount = GetElementsCount(collection, percent);
            return collection.OrderByDescending(x => x).Take(elementsCount);
        }
        public static IEnumerable<T> Top<T>(this IEnumerable<T> collection, int percent, Func<T, int> predicate)
        {
            CheckPercentage(percent);
            var elementsCount = GetElementsCount(collection, percent);
            return collection.OrderByDescending(predicate).Take(elementsCount);
        }

        private static void CheckPercentage(int percent)
        {
            if (percent is < 1 or > 100)
                throw new ArgumentException("Параметр должен быть в диапазоне от 1 до 100.");
        }
        
        private static int GetElementsCount<T>(IEnumerable<T> collection, int percent) =>
            (int)Math.Round(collection.Count() / 100.0 * percent, MidpointRounding.AwayFromZero);

    }
}
