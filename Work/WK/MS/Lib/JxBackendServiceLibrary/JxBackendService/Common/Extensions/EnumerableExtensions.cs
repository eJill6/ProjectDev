using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Common.Extensions
{
    public static class EnumerableExtensions
    {
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