using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.Date
{
    public class CommonDateRange : BaseStringValueModel<CommonDateRange>
    {
        public bool IsCalendarVisible { get; private set; }

        public int StartDateAddDaysFromToday { get; private set; }

        public int EndDateAddDaysFromToday { get; private set; }

        private CommonDateRange()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary> 今日 </summary>
        public static readonly CommonDateRange Today = new CommonDateRange()
        {
            Value = "Today",
            ResourcePropertyName = nameof(SelectItemElement.CommonDateRange_Today)
        };

        /// <summary> 昨日 </summary>
        public static readonly CommonDateRange Yesterday = new CommonDateRange()
        {
            Value = "Yesterday",
            StartDateAddDaysFromToday = -1,
            EndDateAddDaysFromToday = -1,
            ResourcePropertyName = nameof(SelectItemElement.CommonDateRange_Yesterday)
        };

        /// <summary> 近7日 </summary>
        public static readonly CommonDateRange Last7Days = new CommonDateRange()
        {
            Value = "Last7Days",
            StartDateAddDaysFromToday = -6,
            ResourcePropertyName = nameof(SelectItemElement.CommonDateRange_Last7Days)
        };

        /// <summary> 近30日 </summary>
        public static readonly CommonDateRange Last30Days = new CommonDateRange()
        {
            Value = "Last30Days",
            StartDateAddDaysFromToday = -29,
            ResourcePropertyName = nameof(SelectItemElement.CommonDateRange_Last30Days)
        };

        /// <summary> 自定義 </summary>
        public static readonly CommonDateRange Custom = new CommonDateRange()
        {
            Value = "Custom",
            IsCalendarVisible = true,
            StartDateAddDaysFromToday = -34,
            ResourcePropertyName = nameof(SelectItemElement.CommonDateRange_Custom)
        };
    }
}