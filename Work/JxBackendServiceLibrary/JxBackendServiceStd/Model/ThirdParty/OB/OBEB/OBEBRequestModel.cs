using System;
using JxBackendService.Common.Util;

namespace JxBackendService.Model.ThirdParty.OB.OBEB
{
    public class OBEBBaseRequestModel
    {
        /// <summary> 游戏账号 </summary>
        public string loginName { get; set; }

        /// <summary> 时间戳 </summary>
        public long timestamp => DateTime.Now.ToUnixOfTime();
    }

    /// <summary> 創建帳號 </summary>
    public class OBEBUserRequestModel : OBEBBaseRequestModel
    {
        /// <summary> 登陆密码 </summary>
        public string loginPassword { get; set; }

        /// <summary> 语言 (固定设为1，此字段废弃，后续版本会删除。) </summary>
        public int lang => 1;
    }

    /// <summary> 进入游戏 </summary>
    public class OBEBForwardGameRequestModel : OBEBUserRequestModel
    {
        /// <summary> 设备类型 1：網頁版，2：手機版網頁版，3：android，4：ios </summary>
        public int deviceType { get; set; }

        /// <summary> 登陆失败时，平台跳回商户的首页地址 </summary>
        public string backurl { get; set; }

        /// <summary> 是否显示退出按钮 0：显示，1：不显示 </summary>
        public int showExit => 1;

        /// <summary> 是否进入大赛 0：进入大厅，1：进入大赛 </summary>
        public int isCompetition { get; set; }

        /// <summary> 玩家真实ip </summary>
        public string ip { get; set; }

        /// <summary> 主播ID </summary>
        public string anchorId { get; set; }
    }

    /// <summary> 快速進入遊戲(建帳號、充值、取URL) </summary>
    public class OBEBFastGameRequestModel : OBEBForwardGameRequestModel
    {
        /// <summary> 转账金额(上分) </summary>
        public decimal amount { get; set; }

        /// <summary> 转账单号(上分) </summary>
        public string transferNo { get; set; }
    }

    /// <summary> 充值/提現 </summary>
    public class OBEBTransferRequestModel : OBEBBaseRequestModel
    {
        /// <summary> 转账金额(精确到2位小数点) </summary>
        public decimal amount { get; set; }

        /// <summary> 交易单号 </summary>
        public string transferNo { get; set; }

        /// <summary> 是否需要返回最新余额 0：不需要，1：需要 </summary>
        public int showBalance => 1;
    }

    /// <summary> 转账结果查询 </summary>
    public class OBEBCheckTransferRequestModel : OBEBBaseRequestModel
    {
        /// <summary> 交易单号 </summary>
        public string transferNo { get; set; }
    }

    /// <summary> 擷取資料盈虧 </summary>
    public class OBEBDataBaseRequestModel
    {
        /// <summary> 开始日期 </summary>
        public string startTime { get; set; }

        /// <summary> 结束日期 </summary>
        public string endTime { get; set; }

        /// <summary> 页码 </summary>
        public int pageIndex { get; set; }

        /// <summary> 时间戳 </summary>
        public long timestamp => DateTime.Now.ToUnixOfTime();
    }

    public class OBEBAnchorRequestModel
    {
        public int ClientType => 1;

        public int PageIndex => 1;

        public string Ip => "127.0.0.1";

        public long Timestamp { get; set; }        
    }
}