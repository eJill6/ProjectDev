using JxBackendService.Common.Util;
using JxBackendService.Model.Enums.Date;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JxBackendService.Model.ViewModel.Date
{
    public class CommonDateRangeViewModel
    {
        public CommonDateRangeViewModel(CommonDateRange rangeType)
        {
            Value = rangeType.Value;
            Name = rangeType.Name;
            IsCalendarVisible = rangeType.IsCalendarVisible;

            if (IsCalendarVisible)
            {
                MinCalendarDate = DateTime.Today.AddDays(rangeType.StartDateAddDaysFromToday);
                MaxCalendarDate = DateTime.Today.ToQuerySmallEqualThanTime(DatePeriods.Day);
            }
        }

        public string Value { get; set; }

        public string Name { get; set; }

        public bool IsCalendarVisible { get; set; }

        public DateTime? MinCalendarDate { get; set; }

        public DateTime? MaxCalendarDate { get; set; }
    }
}