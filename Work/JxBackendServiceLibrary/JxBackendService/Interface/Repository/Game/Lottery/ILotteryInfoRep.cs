using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository.Game.Lottery
{
    public interface ILotteryInfoRep : IBaseDbRepository<LotteryInfo>
    {
        bool IsActive(int lotteryId);

        List<LotteryInfo> GetAll();

        List<LotteryInfo> GetActiveLotteryInfos();
    }
}