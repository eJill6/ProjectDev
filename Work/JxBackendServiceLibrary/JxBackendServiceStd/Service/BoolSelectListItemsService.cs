using JxBackendService.Common.Extensions;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service
{
    public class BoolSelectListItemsService : IBoolSelectListItemsService
    {
        public List<JxBackendSelectListItem> GetActionSelectListItems()
        {
            return GetSelectListItems((boolValue) => boolValue.GetActionText());
        }

        public List<JxBackendSelectListItem> GetRecommendSelectListItems()
        {
            return GetSelectListItems(DisplayElement.Recommend, DisplayElement.CancelRecommend);
        }

        private List<JxBackendSelectListItem> GetSelectListItems(string trueText, string falseText, bool hasBlankOption = false)
        {
            return GetSelectListItems((boolValue) => boolValue ? trueText : falseText, hasBlankOption);
        }

        private List<JxBackendSelectListItem> GetSelectListItems(Func<bool, string> getTextFunc, bool hasBlankOption = false)
        {
            List<JxBackendSelectListItem> selectListItems = new List<bool> { true, false }
                .Select(i => new JxBackendSelectListItem
                {
                    Value = i.ToString(),
                    Text = getTextFunc(i),
                })
                .ToList();

            selectListItems.AddBlankOption(hasBlankOption, string.Empty, SelectItemElement.PlzChoice);

            return selectListItems;
        }
    }
}