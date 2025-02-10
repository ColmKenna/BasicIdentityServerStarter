namespace FunctionalExtensionMethods;

// extension methods for the IDictionary<TKey, TValue> interface
public static class DictionaryExtensions
{
    // add if not null
    public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (value != null)
        {
            dictionary.Add(key, value);
        }
    }
}

