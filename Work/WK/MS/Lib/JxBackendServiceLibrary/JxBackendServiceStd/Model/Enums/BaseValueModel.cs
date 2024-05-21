using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JxBackendService.Model.Enums
{
    public class BaseShortValueModel<T> : BaseValueModel<short, T>
    {
        public static T GetSingleByNullableValue(short? value)
        {
            if (!value.HasValue)
            {
                return default(T);
            }

            return GetSingle(value.Value);
        }
    }

    public class BaseIntValueModel<T> : BaseValueModel<int, T>
    {
        public static T GetSingleByNullableValue(int? value)
        {
            if (!value.HasValue)
            {
                return default(T);
            }

            return GetSingle(value.Value);
        }
    }

    public class BaseStringValueModel<T> : BaseValueModel<string, T>
    {
        public static T GetSingle(string value, StringComparison stringComparison)
        {
            T item = GetSingle(value);

            if (item != null)
            {
                return item;
            }

            return GetAll()
                .Where(w => w.GetType().GetProperty("Value").GetValue(w, null).ToString().Equals(value, stringComparison))
                .SingleOrDefault();
        }
    }

    public class BaseDecimalValueModel<T> : BaseValueModel<decimal, T>
    { }

    public class BaseValueModel<ValueType, T>
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, List<T>> TypeFieldLists = new ConcurrentDictionary<RuntimeTypeHandle, List<T>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Dictionary<ValueType, T>> TypeFieldDictionaries = new ConcurrentDictionary<RuntimeTypeHandle, Dictionary<ValueType, T>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, FieldInfo[]> FieldInfoMaps = new ConcurrentDictionary<RuntimeTypeHandle, FieldInfo[]>();

        public ValueType Value { get; protected set; }

        public Type ResourceType { get; protected set; }

        public string ResourcePropertyName { get; protected set; }

        public int Sort { get; protected set; }

        public string Name => GetNameByResourceInfo(ResourceType, ResourcePropertyName);

        protected string GetNameByResourceInfo(Type resourceType, string resourcePropertyName)
        {
            if (resourceType == null || resourcePropertyName.IsNullOrEmpty())
            {
                return string.Empty;
            }

            PropertyInfo propertyInfo = resourceType.GetProperty(resourcePropertyName, BindingFlags.Public | BindingFlags.Static);

            if (propertyInfo == null)
            {
                return string.Empty;
            }

            return propertyInfo.GetValue(null, null).ToNonNullString();
        }

        public static List<T> GetAll()
        {
            Type type = typeof(T);

            if (TypeFieldLists.TryGetValue(type.TypeHandle, out List<T> list))
            {
                return list;
            }

            FieldInfo[] fieldInfos = ReflectUtil.GetAllFieldInfos<T>();
            FieldInfoMaps[type.TypeHandle] = fieldInfos;

            list = ReflectUtil.GetAllFields<T>().OrderBy(o => o.GetType().GetProperty(nameof(Sort)).GetValue(o)).ToList();
            TypeFieldLists[type.TypeHandle] = list;
            return list;
        }

        public static List<T> GetAllWithFilter(List<ValueType> exclusionFilter)
        {
            Type type = typeof(T);

            if (TypeFieldLists.TryGetValue(type.TypeHandle, out List<T> list))
            {
                return Filter(list, exclusionFilter);
            }

            list = ReflectUtil.GetAllFields<T>().ToList();
            TypeFieldLists[type.TypeHandle] = list;

            return Filter(list, exclusionFilter);
        }

        private static List<T> Filter(List<T> list, List<ValueType> exclusionFilter)
        {
            if (exclusionFilter != null)
            {
                return list.Where(x => !exclusionFilter.Contains((x as BaseValueModel<ValueType, T>).Value)).ToList();
            }

            return list;
        }

        public static Dictionary<ValueType, T> GetKeyValues()
        {
            Type type = typeof(T);

            if (TypeFieldDictionaries.TryGetValue(type.TypeHandle, out Dictionary<ValueType, T> dic))
            {
                return dic;
            }

            dic = new Dictionary<ValueType, T>();
            GetAll().ForEach(f =>
            {
                var baseValueModel = f as BaseValueModel<ValueType, T>;
                dic.Add(baseValueModel.Value, f);
            });

            TypeFieldDictionaries[type.TypeHandle] = dic;
            return dic;
        }

        public static T GetSingle(ValueType value)
        {
            return GetSingle(GetKeyValues(), value);
        }

        public static T GetSingle(List<T> list, ValueType value)
        {
            return list.Where(w => (w as BaseValueModel<ValueType, T>).Value.Equals(value)).SingleOrDefault();
        }

        public static T GetSingle(Dictionary<ValueType, T> dic, ValueType value)
        {
            if (value != null && dic.ContainsKey(value))
            {
                return dic[value];
            }

            return default(T);
        }

        public static string GetName(ValueType value)
        {
            T item = GetSingle(value);

            if (item != null)
            {
                return (item as BaseValueModel<ValueType, T>).Name;
            }

            return string.Empty;
        }

        public static List<JxBackendSelectListItem> GetSelectListItems()
        {
            return GetSelectListItems(false);
        }

        public static List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption)
        {
            return GetSelectListItems(GetAll(), hasBlankOption);
        }

        public static List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption, string defaultValue, string defaultDisplayText)
        {
            return GetSelectListItems(GetAll(), hasBlankOption, defaultValue, defaultDisplayText);
        }

        public static List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption,
            Func<ValueType, string> getNameJob = null, Func<ValueType, string> getValueJob = null)
        {
            return GetSelectListItems(list, hasBlankOption, string.Empty, SelectItemElement.PlzChoice, getNameJob, getValueJob);
        }

        public static List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption,
            string defaultValue, string defaultDisplayText,
            Func<ValueType, string> getNameJob = null, Func<ValueType, string> getValueJob = null)
        {
            var selectListItems = new List<JxBackendSelectListItem>();

            list.ForEach(f =>
            {
                BaseValueModel<ValueType, T> item = (f as BaseValueModel<ValueType, T>);
                string value = null;

                if (getValueJob != null)
                {
                    value = getValueJob.Invoke(item.Value);
                }
                else
                {
                    value = item.Value.ToString();
                }

                string text = null;

                if (getNameJob != null)
                {
                    text = getNameJob.Invoke(item.Value);
                }
                else
                {
                    text = item.Name;
                }

                selectListItems.Add(new JxBackendSelectListItem(value.ToString(), text));
            });

            selectListItems.AddBlankOption(hasBlankOption, defaultValue, defaultDisplayText);

            return selectListItems;
        }

        #region 輔助運算用途

        public static bool operator ==(ValueType value, BaseValueModel<ValueType, T> valueModel)
        {
            return valueModel.Value.Equals(value);
        }

        public static bool operator !=(ValueType value, BaseValueModel<ValueType, T> valueModel)
        {
            return !valueModel.Value.Equals(value);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion 輔助運算用途
    }

    public static class BaseValueModelExtensions
    {
        public static JxBackendSelectListItem ToSelectListItem<T>(this BaseIntValueModel<T> model)
        {
            return new JxBackendSelectListItem()
            {
                Value = model.Value.ToString(),
                Text = model.Name
            };
        }

        public static JxBackendSelectListItem ToSelectListItem<T>(this BaseStringValueModel<T> model)
        {
            return new JxBackendSelectListItem()
            {
                Value = model.Value,
                Text = model.Name
            };
        }

        public static JxBackendSelectListItem ToSelectListItem<ValueType, T>(this BaseValueModel<ValueType, T> model)
        {
            string value = string.Empty;

            if (model.Value != null)
            {
                if (model.Value is System.Enum)
                {
                    value = Convert.ToInt32(model.Value).ToString();
                }
                else
                {
                    value = model.Value.ToString();
                }
            }

            return new JxBackendSelectListItem()
            {
                Value = value,
                Text = model.Name
            };
        }

        public static ValueType TryGetValue<KeyType, ValueType>(this Dictionary<KeyType, ValueType> keyValuePairs, KeyType key)
        {
            keyValuePairs.TryGetValue(key, out ValueType value);

            return value;
        }

        /// <summary>
        /// add if not exists
        /// </summary>
        public static bool AddNX<KeyType, ValueType>(this Dictionary<KeyType, ValueType> keyValuePairs, KeyType key, ValueType value)
        {
            if (!keyValuePairs.ContainsKey(key))
            {
                keyValuePairs.Add(key, value);
            }

            return true;
        }
    }
}