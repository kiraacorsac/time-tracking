public static class IEnumerableExtensions
{
    public static string ToReadable<T>(this IEnumerable<T> enumerable) where T : notnull
    {
        return string.Join(Environment.NewLine, enumerable);
    }
}