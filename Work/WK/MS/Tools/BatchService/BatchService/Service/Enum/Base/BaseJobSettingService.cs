using BatchService.Interface;
using BatchService.Model.Enum;
using JxBackendService.Service.Enums;
using System.Collections.Generic;

namespace BatchService.Service.Enum.Base
{
    public class BaseJobSettingService : BaseValueModelService<string, JobSetting>, IJobSettingService
    {
        ///// <summary>
        ///// 取得所有商戶共用job
        ///// </summary>
        //protected override List<JobSetting> CreateAllList()
        //{
        //    return new List<JobSetting>()
        //    {
        //        JobSetting.StoredProcedureErrorNotice,
        //        JobSetting.RecheckTranferOrdersFromMiseLive
        //    };
        //}
    }
}