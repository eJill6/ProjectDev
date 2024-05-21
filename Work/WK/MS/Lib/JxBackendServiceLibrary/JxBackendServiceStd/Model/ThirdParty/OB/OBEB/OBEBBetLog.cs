using JxBackendService.Model.ThirdParty.Base;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.OB.OBEB
{
    public class OBEBBetLog
    {
        public int pageSize { get; set; }

        public int pageIndex { get; set; }

        public int totalRecord { get; set; }

        public int totalPage { get; set; }

        public List<BetRecordLog> record { get; set; }
    }

    public class BetRecordLog : BaseRemoteBetLog
    {
        /// <summary> 下注流水号 </summary>
        public long? id { get; set; }

        /// <summary> 玩家编号 </summary>
        public int playerId { get; set; }

        /// <summary> 玩家账号 </summary>
        public string playerName { get; set; }

        /// <summary> 商户编号 </summary>
        public int agentId { get; set; }

        /// <summary> 投注额 </summary>
        public decimal betAmount { get; set; }

        /// <summary> 有效投注额 (取消局或跳局情况下值为0) </summary>
        public decimal validBetAmount { get; set; }

        /// <summary> 输赢额 (取消局或跳局情况下值为0) </summary>
        public decimal netAmount { get; set; }

        /// <summary> 下注前余额 </summary>
        public decimal beforeAmount { get; set; }

        /// <summary> 投注时间 </summary>
        public long createdAt { get; set; }

        /// <summary> 结算时间 </summary>
        public long? netAt { get; set; }

        /// <summary> 注单重新结算时间 (默认为0，非0时表示有重算、取消) </summary>
        public long recalcuAt { get; set; }

        /// <summary> 更新时间 </summary>
        public long updatedAt { get; set; }

        /// <summary> 游戏编码 </summary>
        public int gameTypeId { get; set; }

        /// <summary> 厅id </summary>
        public int platformId { get; set; }

        /// <summary> 厅名称 </summary>
        public string platformName { get; set; }

        /// <summary> 注单状态 0：未结算、1：已结算、2：取消投注 </summary>
        public int betStatus { get; set; }

        /// <summary> 重算标志 </summary>
        /// 普通注单(0=正常结算，1=跳局，2=取消局，4=重算指定局 )
        /// 大赛注单(0=正常, 1=跳局, 2=比赛取消 ,3=取消局 ,4=重算局 ,5=已弃赛)

        public int betFlag { get; set; }

        /// <summary> 玩法，下注点 </summary>
        public int betPointId { get; set; }

        /// <summary> 赔率 </summary>
        public string odds { get; set; }

        /// <summary> 局结果 </summary>
        public string judgeResult { get; set; }

        /// <summary> 币种 </summary>
        public string currency { get; set; }

        /// <summary> 桌台号 </summary>
        public string tableCode { get; set; }

        /// <summary> 局号 </summary>
        public string roundNo { get; set; }

        /// <summary> 靴号 </summary>
        public string bootNo { get; set; }

        /// <summary> 游戏ip </summary>
        public string loginIp { get; set; }

        /// <summary> 设备类型 </summary>
        public int deviceType { get; set; }

        /// <summary> 设备id </summary>
        public string deviceId { get; set; }

        /// <summary> 注单类别 </summary>
        public int recordType { get; set; }

        /// <summary> 游戏模式 0：常规下注、1：入座、2：旁注、3：好路、4：多台 </summary>
        public int gameMode { get; set; }

        /// <summary> 签名 </summary>
        public string signature { get; set; }

        /// <summary> 荷官名称 </summary>
        public string dealerName { get; set; }

        /// <summary> 桌台名称 </summary>
        public string tableName { get; set; }

        /// <summary> 返奖额 (返奖额=投注额+输赢额) </summary>
        public decimal payAmount { get; set; }

        /// <summary>  </summary>
        public long startid { get; set; }

        /// <summary> 真实扣款金额 </summary>
        public decimal realDeductAmount { get; set; }

        /// <summary> 注单类型 (1：普通注单、2：大赛注单) </summary>
        public int bettingRecordType { get; set; }

        /// <summary> 扩展字段 </summary>
        public string addstr1 { get; set; }

        /// <summary> 扩展字段 </summary>
        public string addstr2 { get; set; }

        public string result { get; set; }

        /// <summary> 游戏名称 </summary>
        public string gameTypeName { get; set; }

        /// <summary> 玩法名称 </summary>
        public string betPointName { get; set; }

        public override string KeyId => $"{id}";

        public override string TPGameAccount => playerName;
    }
}
