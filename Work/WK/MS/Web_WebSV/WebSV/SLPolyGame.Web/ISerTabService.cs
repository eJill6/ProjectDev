using System;
using System.Collections.Generic;
using System.ServiceModel;
using SLPolyGame.Web.Model;
using SLPolyGame.Web.Behavior;

namespace SLPolyGame.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISerTabService”。
    [ServiceContract]
    public interface ISerTabService
    {
        /// <summary>
        /// 获取各彩种购买单选方式
        /// </summary>
        [OperationContract]
        [MOperationBehavior]
        List<PlayTypeRadio> GetPlayTypeRadio();

        [OperationContract]
        [MOperationBehavior]
        List<PlayTypeInfo> GetPlayTypeInfo();

        [OperationContract]
        [MOperationBehavior]
        List<LotteryInfo> GetLotteryType();

        [OperationContract]
        [MOperationBehavior]
        CurrentLotteryInfo GetLotteryInfos(int lotteryid);

        [OperationContract]
        [MOperationBehavior]
        List<LotteryInfo> Get_AllWebLotteryType();

        [OperationContract]
        [MOperationBehavior]
        List<CurrentLotteryInfo> QueryCurrentLotteryInfo(CurrentLotteryQueryInfo query);

        [OperationContract]
        [MOperationBehavior]
        TodaySummaryInfo GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID, int count);

        [OperationContract]
        [MOperationBehavior]
        TodaySummaryInfo GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy);
    }
}