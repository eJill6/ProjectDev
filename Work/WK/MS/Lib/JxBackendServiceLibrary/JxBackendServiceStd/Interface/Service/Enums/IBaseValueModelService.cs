using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IBaseValueModelService<ValueType, T>
    {
        List<T> GetAll();

        string GetName(ValueType value);

        int GetSort(T item);

        List<JxBackendSelectListItem> GetSelectListItems();

        List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption);

        List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption, string defaultValue, string defaultDisplayText);

        List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption);

        List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption, string defaultValue, string defaultDisplayText);

        List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption, string defaultValue, string defaultDisplayText, Func<ValueType, string> getNameJob, Func<ValueType, string> getValueJob);

        T GetSingle(ValueType value);
    }
}