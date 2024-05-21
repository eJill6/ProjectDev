using System;
using System.Collections.Concurrent;

namespace JxBackendService.Common.Util
{
    public static class AssignValueOnceUtil
    {
        public static T GetAssignValueOnce<T>(this T propertyValue, Func<T> getPropertyValueJob)
        {
            if (propertyValue == null)
            {
                return getPropertyValueJob.Invoke();
            }

            return propertyValue;
        }

        public static TValue GetAssignValueOnce<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> map, TKey key,
            Func<TValue> getValueJob)
        {
            if (map.TryGetValue(key, out TValue savedValue))
            {
                return savedValue;
            }

            TValue value = getValueJob.Invoke();
            map[key] = value;

            return value;
        }
    }
}
