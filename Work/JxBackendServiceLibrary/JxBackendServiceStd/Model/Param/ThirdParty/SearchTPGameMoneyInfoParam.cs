using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Model.Param.ThirdParty
{
    public class SearchTPGameMoneyInfoParam : BasePagingRequestParam
    {
        public int? UserID { get; set; }

        public int? OrderStatus { get; set; }

        public DateTime SearchOrderStartDate { get; set; }

        public DateTime SearchOrderEndDate { get; set; }
    }

    public class SearchTransferType : BaseIntValueModel<SearchTransferType>
    {
        public string GameTransferTypeName { get; private set; }

        public string DisplayTextCssClass { get; private set; }

        private SearchTransferType()
        { }

        public static SearchTransferType In = new SearchTransferType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SearchTransferType_In),
            GameTransferTypeName = SelectItemElement.GameTransferType_In,
            DisplayTextCssClass = "transfer_in",
        };

        public static SearchTransferType Out = new SearchTransferType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.SearchTransferType_Out),
            GameTransferTypeName = SelectItemElement.GameTransferType_Out,
            DisplayTextCssClass = "transfer_out",
        };
    }
}