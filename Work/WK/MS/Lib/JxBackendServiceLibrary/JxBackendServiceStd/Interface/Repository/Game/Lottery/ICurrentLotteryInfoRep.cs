using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Interface.Repository.Game.Lottery
{
    public interface ICurrentLotteryInfoRep : IBaseDbRepository<CurrentLotteryInfo>    
    {
       PagedResultModel<CurrentLotteryInfo> GetCurrentLotteryInfoReport(CurrentLotteryParam param);
    }
}
