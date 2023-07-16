using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Enums
{
    public class BaseValueModelService<ValueType, T> : IBaseValueModelService<ValueType, T>
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, List<T>> TypeFieldLists = new ConcurrentDictionary<RuntimeTypeHandle, List<T>>();

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Dictionary<ValueType, T>> TypeFieldDictionaries
            = new ConcurrentDictionary<RuntimeTypeHandle, Dictionary<ValueType, T>>();

        protected virtual List<T> CreateAllList()
        {
            return BaseValueModel<ValueType, T>.GetAll();
        }

        public List<T> GetAll()
        {
            RuntimeTypeHandle typeHandle = GetType().TypeHandle;

            TypeFieldLists.TryGetValue(typeHandle, out List<T> list);

            if (list != null)
            {
                return list;
            }

            list = CreateAllList();
            TypeFieldLists[typeHandle] = list;
            return list;
        }

        public T GetSingle(ValueType value)
        {
            if (value == null)
            {
                return default(T);
            }

            Dictionary<ValueType, T> dictionary = GetKeyValues();

            if (value != null && dictionary.ContainsKey(value))
            {
                return dictionary[value];
            }

            return default(T);
        }

        public virtual string GetName(ValueType value)
        {
            T item = GetSingle(value);

            if (item != null)
            {
                return (item as BaseValueModel<ValueType, T>).Name;
            }

            return string.Empty;
        }

        public virtual int GetSort(T item)
        {
            if (item != null)
            {
                return (item as BaseValueModel<ValueType, T>).Sort;
            }

            return int.MaxValue;
        }

        public List<JxBackendSelectListItem> GetSelectListItems()
        {
            return GetSelectListItems(false);
        }

        public List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption)
        {
            return GetSelectListItems(GetAll(), hasBlankOption);
        }

        public List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption, string defaultValue, string defaultDisplayText)
        {
            return GetSelectListItems(GetAll(), hasBlankOption, defaultValue, defaultDisplayText);
        }

        public List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption)
        {
            return GetSelectListItems(list, hasBlankOption, string.Empty, SelectItemElement.PlzChoice);
        }

        public List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption, string defaultValue, string defaultDisplayText)
        {
            return BaseValueModel<ValueType, T>.GetSelectListItems(list, hasBlankOption, defaultValue, defaultDisplayText, (value) => GetName(value));
        }

        public List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption, string defaultValue, string defaultDisplayText,
            Func<ValueType, string> getNameJob, Func<ValueType, string> getValueJob)
        {
            return BaseValueModel<ValueType, T>.GetSelectListItems(list, hasBlankOption, defaultValue, defaultDisplayText, getNameJob, getValueJob);
        }

        private Dictionary<ValueType, T> GetKeyValues()
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
    }
}