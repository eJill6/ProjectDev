using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Model.ThirdParty.Base;
using ProductTransferService.LCDataBase.DLL;

namespace ProductTransferService.LCDataBase.Model
{
    public class SingleBetInfo
    {
        /// <summary>
        /// 使用者id
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 游戏局号
        /// </summary>
        public string GameID { get; set; }

        /// <summary>
        /// 玩家帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 房间 ID
        /// </summary>
        public int ServerID { get; set; }

        /// <summary>
        /// 游戏 ID
        /// </summary>
        public int KindID { get; set; }

        /// <summary>
        /// 桌号
        /// </summary>
        public long TableID { get; set; }

        /// <summary>
        /// 椅子号
        /// </summary>
        public int ChairID { get; set; }

        /// <summary>
        /// 玩家数量
        /// </summary>
        public int UserCount { get; set; }

        /// <summary>
        /// 有效下注
        /// </summary>
        public string CellScore { get; set; }

        /// <summary>
        /// 总下注
        /// </summary>
        public string AllBet { get; set; }

        /// <summary>
        /// 盈利
        /// </summary>
        public string Profit { get; set; }

        /// <summary>
        /// 抽水
        /// </summary>
        public string Revenue { get; set; }

        /// <summary>
        /// 游戏开始时间
        /// </summary>
        public string GameStartTime { get; set; }

        /// <summary>
        /// 游戏结束时间
        /// </summary>
        public string GameEndTime { get; set; }

        /// <summary>
        /// 手牌公共牌
        /// </summary>
        public string CardValue { get; set; }

        /// <summary>
        /// 渠道 ID
        /// </summary>
        public int ChannelID { get; set; }

        /// <summary>
        /// 游戏结果对应玩家所属站点
        /// </summary>
        public string LineCode { get; set; }
    }

    public class SingleBetInfoViewModel : BaseRemoteBetLog
    {
        public SingleBetInfo SingleBetInfo { get; set; }

        public override string KeyId => SingleBetInfo.GameID;

        public override string TPGameAccount
        {
            get
            {
                string lcUserHeader = DependencyUtil.ResolveService<ILCConfigService>().Value.LCUserHeader;

                return SingleBetInfo.Account.ToNonNullString().Replace(lcUserHeader, string.Empty);
            }
        }
    }
}