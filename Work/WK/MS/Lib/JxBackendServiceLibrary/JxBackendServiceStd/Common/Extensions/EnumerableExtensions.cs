using System;
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

        public static IEnumerable<TSource> DistinctByFilter<TSource, TKey>(this List<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> keySet = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                TKey key = keySelector(element);

                if (keySet.Add(key))
                {
                    yield return element;
                }
            }
        }

        /// <summary>轉HashSet, standard沒有此方法,避免命名跟原本.net擴充跟core之後的版本相衝,所以加上了Convert前墜 </summary>
        public static HashSet<TSource> ConvertToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            return new HashSet<TSource>(source);
        }
    }
}