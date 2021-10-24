using JxBackendService.Interface.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Report
{
    public class ProfitLossHistoryViewModel
    {
        public IReportInnerSettingService BaseReportInnerSettingService { get; set; }

        public int ProfitLossReportTabTypes { get; set; }

    }
}
