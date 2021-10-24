using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            var hashSet = new HashSet<TSource>();

            if (source != null)
            {
                foreach (TSource item in source)
                {
                    hashSet.Add(item);
                }
            }

            return hashSet;
        }
    }
}
