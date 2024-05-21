using JxBackendService.Interface.Repository.Game.Lottery;
using JxBackendService.Model.Entity.Game.Lottery;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System.Collections.Generic;

namespace JxBackendService.Repository.Game.Lottery
{
    public class LotteryInfoRep : BaseDbRepository<LotteryInfo>, ILotteryInfoRep
    {
        public LotteryInfoRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public List<LotteryInfo> GetAll()
        {
            return GetAll(InlodbType.Inlodb);
        }

        public List<LotteryInfo> GetActiveLotteryInfos()
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb) + @"
				WHERE [Status] = 1
				ORDER BY GameTypeID, Priority";

            return DbHelper.QueryList<LotteryInfo>(sql, null);
        }

        public bool IsActive(int lotteryId)
        {
            LotteryInfo lotteryInfo = GetSingleByKey(InlodbType.Inlodb, new LotteryInfo() { LotteryID = lotteryId });

            if (lotteryInfo.Status == 1)
            {
                return true;
            }

            //沒資料當作沒開啟
            return false;
        }
    }
}