using System.Runtime.CompilerServices;

namespace Web.Extensions;

public static class CommonExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T As<T>(this object obj) =>
        (T)obj;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? AsOrDefault<T>(this object obj)
    {
        try
        {
            return (T) obj;
        }
        catch (Exception)
        {
            return default;
        }
    }
}