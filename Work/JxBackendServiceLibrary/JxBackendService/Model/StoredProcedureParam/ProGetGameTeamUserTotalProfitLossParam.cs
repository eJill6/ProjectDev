using System;
using JxBackendService.Model.Paging;

namespace JxBackendService.Model.StoredProcedureParam
{
    public class ProGetGameTeamUserTotalProfitLossParam : BasePagedParamsModel
    {
        public DateTime QueryStartDate { get; set; }
        public DateTime QueryEndDate { get; set; }
        public string SearchUserName { get; set; }
        public DateTime? ExclusiveAfterSaveTime { get; set; }
        public SortModel SortModel { get; set; } = new SortModel();
    }

    public class SearchProductProfitlossParam : BasePagedParamsModel
    {
        public DateTime QueryStartDate { get; set; }
        public DateTime QueryEndDate { get; set; }
        public string SearchUserName { get; set; }
        public string ProductCode { get; set; }
        public string SortText { get; set; }

        public bool IsOnlySearchLastMonthBonus { get; set; }
        public DateTime? ExclusiveAfterSaveTime
        {
            get
            {
                if (IsOnlySearchLastMonthBonus)
                {
                    return DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1).AddHours(1).AddMinutes(5); //當月1號1:05
                }

                return null;
            }
        }
    }
}
