using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class WhiteIpType : BaseIntValueModel<WhiteIpType>
    {
        private WhiteIpType() { }

        public static readonly WhiteIpType FrontSideLogin = new WhiteIpType()
        {
            Value = 1,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WhiteIpType_FrontSide)
        };

        public static readonly WhiteIpType BackSideWebLogin = new WhiteIpType()
        {
            Value = 2,
            ResourceType = typeof(SelectItemElement),
            ResourcePropertyName = nameof(SelectItemElement.WhiteIpType_BackSide)
        };
    }
}
