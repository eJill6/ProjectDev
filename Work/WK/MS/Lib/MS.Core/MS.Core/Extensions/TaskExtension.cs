using Amazon.Auth.AccessControlPolicy;
using MS.Core.Models;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MS.Core.Extensions
{
    public static class TaskExtension
    {
        public static async Task<IEnumerable<TSource>> AsEnumerableAsync<TSource>(this Task<TSource[]> source)
        {
            return (await source).AsEnumerable();
        }

        public static async Task<IEnumerable<TSource>> OrderByDescendingAsync<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        {
            return (await source).OrderByDescending(keySelector);
        }

        public static async Task<IOrderedEnumerable<TSource>> OrderByAsync<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        {
            return (await source).OrderBy(keySelector);
        }
        public static async Task<bool> IsSuccessAsync<T>(this Task<T> task) where T : BaseReturnModel
        {
            var result = await task;

            return result.IsSuccess;
        }

        public static async Task<T> GetReturnDataAsync<T>(this Task<BaseReturnDataModel<T>> task)
        {
            var result = await task;

            return result.DataModel;
        }

        
        public static async Task<T?> FirstOrDefaultAsync<T>(this Task<IEnumerable<T>> task)
        {
            var result = await task;
            return result.FirstOrDefault();
        }

        public static async Task<T> FirstAsync<T>(this Task<IEnumerable<T>> task)
        {
            var result = await task;
            return result.First();
        }

        public static async Task<T?> FirstOrDefaultAsync<T>(this Task<T[]> task)
        {
            var result = await task;
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Creates an array from a System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> task)
        {
            var result = await task;
            return result.ToArray();
        }
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TSource>> WhereAsync<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            var result = await source;
            return result.Where(predicate);
        }
        /// <summary>
        /// Projects each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TResult> selector)
        {
            var result = await source;
            return result.Select(selector);
        }

        /// <summary>
        /// Determines whether a sequence contains a specified element by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource"></typeparam
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> ContainsAsync<TSource>(this Task<HashSet<TSource>> source, TSource value)
        {
            var result = await source;
            return result.Contains(value);
        }

        /// <summary>
        /// Creates a System.Collections.Generic.HashSet`1 from an System.Collections.Generic.IEnumerable`1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<HashSet<T>> ToHashSetAsync<T>(this Task<IEnumerable<T>> task)
        {
            var result = await task;
            return result.ToHashSet();
        }

        private static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            HashSet<T> result = new HashSet<T>(items);
            return result;
        }

        /// <summary>
        /// Error 再嘗試一次
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task ErrorReTry(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                await task;
            }
        }

        /// <summary>
        /// Error 再嘗試一次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T> ErrorReTry<T>(this Task<T> task)
        {
            try
            {
                return await task;
            }
            catch
            {
                return await task;
            }
        }
    }
}
