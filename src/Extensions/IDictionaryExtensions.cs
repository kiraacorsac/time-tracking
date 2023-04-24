public static class IDictionaryExtensions
{
    public static string ToReadable<T, V>(this IDictionary<T, V> d) where T : notnull
    {
        return string.Join(Environment.NewLine, d.Select(a => $"{a.Key}: {a.Value}"));
    }
}