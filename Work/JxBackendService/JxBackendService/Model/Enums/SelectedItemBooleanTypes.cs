using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class SelectedItemBooleanTypes : BaseStringValueModel<SelectedItemBooleanTypes>
    {
        private SelectedItemBooleanTypes() { }

        public string TrueItemName { get; set; }

        public string FalseItemName { get; set; }

        public static SelectedItemBooleanTypes FrontsideMenuActive = new SelectedItemBooleanTypes()
        {
            Value = "FrontsideMenuActive",
            TrueItemName = CommonElement.Open,
            FalseItemName = CommonElement.CloseDown
        };

        public string GetItemName(bool boolValue)
        {
            if (boolValue)
            {
                return TrueItemName;
            }
            else
            {
                return FalseItemName;
            }
        }
    }
}
