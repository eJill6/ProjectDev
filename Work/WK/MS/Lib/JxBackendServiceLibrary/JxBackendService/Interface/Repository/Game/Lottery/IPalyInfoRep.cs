using JxBackendService.Model.Entity.Game.Lottery;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game.Lottery
{
    public interface IPalyInfoRep : IBaseDbRepository<PalyInfo>
    {
        List<PalyInfo> GetPalyInfos(List<int> palyIDs);
    }
}