using System.Collections.Generic;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Game;

namespace JxBackendService.Interface.Repository.Game
{
    public interface ILotteryInfoRep :IBaseDbRepository<LotteryInfo>
    {
        bool IsActive(int lotteryId);
        
        List<LotteryInfo> GetAll();
    }
}