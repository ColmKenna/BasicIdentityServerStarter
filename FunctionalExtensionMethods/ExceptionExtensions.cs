namespace FunctionalExtensionMethods;

public static class ExceptionExtensions
{
    public static void ThrowIfNull<T>(this T obj, string paramName) where T : class
    {
        if (obj == null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
    
    public static void ThrowIfAllAreNullOrEmpty(this string[] array, string exceptionMessage)
    {
        if (array.All(string.IsNullOrEmpty))
        {
            throw new ArgumentException(exceptionMessage);
        }
    }
    
    
    
}