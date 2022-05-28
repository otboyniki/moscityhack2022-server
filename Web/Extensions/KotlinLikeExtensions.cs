using System.Runtime.CompilerServices;

namespace Web.Extensions;

public static class KotlinLikeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TSource Also<TSource>(this TSource source, Action<TSource> action)
    {
        action(source);
        return source;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Let<TSource, TResult>(this TSource source, Func<TSource, TResult> func) =>
        func(source);
}