using JxBackendService.Model.Common;
using System.Collections.Generic;

namespace JxBackendService.Model.ThirdParty.AllBet
{
    public class ABRegisterRequestModel : ABUserBaseModel
    {
        /// <summary> 直属上线代理帐号 </summary>
        public string Agent => ABEBSharedAppSetting.AgentUserName;
    }

    public class ABUserScoreRequestModel
    {
        /// <summary> 直属上线代理帐号 </summary>
        public string Agent => ABEBSharedAppSetting.AgentUserName;

        public int PageSize => 1;

        public int PageIndex => 1;

        /// <summary>
        /// 0: 指定代理下的直属玩家 1：所有下线玩家
        /// </summary>
        public int Recursion => 0;

        public List<string> Players { get; set; }
    }

    public class ABLunchGameRequestModel : ABUserBaseModel
    {
    }

    public class ABCheckTransferRequestModel
    {
        public string Sn { get; set; }
    }

    public class ABTransferRequestModel : ABUserBaseModel
    {
        /// <summary> 交易流水号 (唯一的)接入商编号 + 13個数字 </summary>
        public string Sn { get; set; }

        /// <summary> 直属上线代理帐号 </summary>
        public string Agent => ABEBSharedAppSetting.AgentUserName;

        /// <summary> 转账类型 1: 存入 0: 提取</summary>
        public int Type { get; set; }

        /// <summary> 转账金额</summary>
        public string Amount { get; set; }
    }

    public class ABBetLogRequestModel
    {
        /// <summary> 直属上线代理帐号 </summary>
        public string Agent => ABEBSharedAppSetting.AgentUserName;

        public string StartDateTime { get; set; }

        public string EndDateTime { get; set; }

        public int PageNum { get; set; }

        public int PageSize => 1000;
    }
}