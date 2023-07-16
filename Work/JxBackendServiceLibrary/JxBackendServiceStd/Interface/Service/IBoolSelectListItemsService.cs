using JxBackendService.Model.Common;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IBoolSelectListItemsService
    {
        List<JxBackendSelectListItem> GetActionSelectListItems();

        List<JxBackendSelectListItem> GetRecommendSelectListItems();
    }
}