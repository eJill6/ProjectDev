using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class SearchTPGameMoneyInfoParam : BasePagingRequestParam
    {
        public string UserName { get; set; }

        public int? OrderStatus { get; set; }

        public DateTime SearchOrderStartDate { get; set; }

        public DateTime SearchOrderEndDate { get; set; }
    }

    public class SearchTransferType : BaseIntValueModel<SearchTransferType>
    {
        private SearchTransferType() { }

        public static SearchTransferType In = new SearchTransferType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SearchTransferType_In)
        };

        public static SearchTransferType Out = new SearchTransferType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SearchTransferType_Out)
        };

        public string FullName => Name + SelectItemElement.GameAccount;

        public static List<JxBackendSelectListItem> GetFullNameSelectListItems()
        {
            List<JxBackendSelectListItem> selectListItems = GetAll()
                .Select(s => new JxBackendSelectListItem(s.Value.ToString(), s.FullName))
                .ToList();
            
            return selectListItems;
        }
    }
}
