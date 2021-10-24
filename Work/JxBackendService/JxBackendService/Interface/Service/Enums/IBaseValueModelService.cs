using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IBaseValueModelService<ValueType, T>
    {
        List<T> GetAll();
        string GetName(ValueType value);
        List<JxBackendSelectListItem> GetSelectListItems();
        List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption);
        List<JxBackendSelectListItem> GetSelectListItems(bool hasBlankOption, string defaultValue, string defaultDisplayText);
        List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption);
        List<JxBackendSelectListItem> GetSelectListItems(List<T> list, bool hasBlankOption, string defaultValue, string defaultDisplayText);
        T GetSingle(ValueType value);
    }
}
