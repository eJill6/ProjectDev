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

        /// <summary>轉HashSet, standard沒有此方法,避免命名跟原本.net擴充跟core之後的版本相衝,所以加上了Convert前墜 </summary>
        public static HashSet<TSource> ConvertToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            return new HashSet<TSource>(source);
        }
    }
}