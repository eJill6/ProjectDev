using Amazon.Auth.AccessControlPolicy;
using System.Diagnostics.CodeAnalysis;

namespace MS.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? source)
        {
            if (source == null)
                return false;

            return source.Any();
        }

        public static bool IsEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source)
        {
            if (source == null)
                return true;

            return !source.Any();
        }

        public static IEnumerable<T> ToEnumerable<T>(this T source)
        {
            yield return source;
        }

        public static decimal SumOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            return source.Select(selector).DefaultIfEmpty().Sum();
        }

        public static int SumOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            return source.Select(selector).DefaultIfEmpty().Sum();
        }

        public static TValue MinOrDefault<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> selector)
            where TValue : struct
        {
            return source.Select(selector).DefaultIfEmpty().Min();
        }

        public static TValue MaxOrDefault<TSource, TValue>(this IEnumerable<TSource> source, Func<TSource, TValue> selector)
           where TValue : struct
        {
            return source.Select(selector).DefaultIfEmpty().Max();
        }

        public static bool AnyAndNotNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source != null)
            {
                return source.Any();
            }

            return false;
        }

        public static void RemoveRangeByFit<TSource>(this List<TSource> source, int index, int count)
        {
            int totalCount = source.Count;
            int removeCount = count;

            if (index + count > totalCount)
            {
                removeCount = totalCount - index;
            }

            source.RemoveRange(index, removeCount);
        }
    }
}