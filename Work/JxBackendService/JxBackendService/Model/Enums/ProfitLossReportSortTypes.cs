using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class ProfitLossReportSortTypes : BaseIntValueModel<ProfitLossReportSortTypes>
    {
        private ProfitLossReportSortTypes() { }

        public string SqlScript { get; private set; }

        //用户名称倒序
        public static readonly ProfitLossReportSortTypes UserNameDesc = new ProfitLossReportSortTypes() { Value = 0 , SqlScript = " Username DESC " };
        //用户名称顺序
        public static readonly ProfitLossReportSortTypes UserNameAsc = new ProfitLossReportSortTypes() { Value = 1, SqlScript = " Username ASC " };
        //默認:盈虧倒序
        public static readonly ProfitLossReportSortTypes ProfitLossDesc = new ProfitLossReportSortTypes() { Value = 2, SqlScript = " ZKYProfitLossMoney DESC , Username ASC " };
        //盈虧順序
        public static readonly ProfitLossReportSortTypes ProfitLossAsc = new ProfitLossReportSortTypes() { Value = 3, SqlScript = " ZKYProfitLossMoney ASC , Username ASC " };
        //投注倒序
        public static readonly ProfitLossReportSortTypes BetDesc = new ProfitLossReportSortTypes() { Value = 4, SqlScript = " TZProfitLossMoney DESC , Username ASC " };
        //投注顺序
        public static readonly ProfitLossReportSortTypes BetAsc = new ProfitLossReportSortTypes() { Value = 5, SqlScript = " TZProfitLossMoney ASC , Username ASC " };
        //充值倒序
        public static readonly ProfitLossReportSortTypes MoneyInDesc = new ProfitLossReportSortTypes() { Value = 6, SqlScript = " CZProfitLossMoney DESC , Username ASC " };
        //充值顺序
        public static readonly ProfitLossReportSortTypes MoneyInAsc = new ProfitLossReportSortTypes() { Value = 7, SqlScript = " CZProfitLossMoney ASC, Username ASC " };
    }
}
