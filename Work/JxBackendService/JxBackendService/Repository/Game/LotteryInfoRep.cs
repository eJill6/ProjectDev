using JxBackendService.Interface.Repository.Game;
using JxBackendService.Model.Entity.Game;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Game
{
    public class LotteryInfoRep : BaseDbRepository<LotteryInfo>, ILotteryInfoRep
    {
        public LotteryInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {

        }

        public List<LotteryInfo> GetAll()
        {
            return DbHelper.QueryList<LotteryInfo>(GetAllQuerySQL(InlodbType.Inlodb), null);
        }

        public bool IsActive(int lotteryId)
        {
            LotteryInfo lotteryInfo = GetSingleByKey(InlodbType.Inlodb, new LotteryInfo() { LotteryID = lotteryId });
            
            if(lotteryInfo.Status == 1)
            {
                return true;
            }

            //沒資料當作沒開啟
            return false;
        }
    }
}
