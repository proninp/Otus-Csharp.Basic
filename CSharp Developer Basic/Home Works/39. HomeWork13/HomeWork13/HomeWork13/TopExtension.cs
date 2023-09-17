namespace HomeWork13
{
    public static class TopExtension
    {
        public static IEnumerable<T> Top<T>(this IEnumerable<T> collection, int percent) => collection.Top(percent, x => x);

        public static IEnumerable<T> Top<T, K>(this IEnumerable<T> collection, int percent, Func<T, K> predicate)
        {
            if (collection is null)
                throw new ArgumentNullException(nameof(collection));
            if (percent is < 1 or > 100)
                throw new ArgumentOutOfRangeException(nameof(percent), percent, "Параметр должен быть в диапазоне от 1 до 100.");
            
            var elementsCount = (int)Math.Ceiling(collection.Count() / 100.0 * percent);
            return collection.OrderByDescending(predicate).Take(elementsCount);
        }
    }
}
