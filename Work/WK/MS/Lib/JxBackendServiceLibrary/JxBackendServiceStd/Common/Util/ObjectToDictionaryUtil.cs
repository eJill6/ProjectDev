using JxBackendService.Model.Paging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace JxBackendService.Common.Util
{
    /// <summary>
    /// 参数对象转字典
    /// </summary>
    public static class ObjectToDictionaryUtil
    {
        /// <summary>
        /// 参数对象转字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjectToDictionary<T>(this T t) where T : BasePagingRequestParam
        {
            Dictionary<string, string> dResult = new Dictionary<string, string>();

            Type type = t.GetType();


            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string key = propertyInfo.Name;
                string value = propertyInfo.GetValue(t, null)?.ToString();

                dResult.Add(key, value);
            }

            return dResult;

        }
    }
}
