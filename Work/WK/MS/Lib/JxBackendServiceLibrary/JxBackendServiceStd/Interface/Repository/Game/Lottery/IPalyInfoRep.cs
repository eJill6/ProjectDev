using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.BackSideWeb;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game.Lottery
{
    public interface IPalyInfoRep : IBaseDbRepository<PalyInfo>
    {
        List<PalyInfo> GetPalyInfos(List<int> palyIDs);
        PagedResultModel<JxBackendService.Model.BackSideWeb.PalyInfo> GetPalyInfoReport(PalyInfoParam param);
        JxBackendService.Model.BackSideWeb.PalyInfo GetPalyInfoDetail(string PalyID);
    }
}