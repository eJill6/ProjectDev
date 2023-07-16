using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class IMVRTransferSqlLiteRep : BaseTransferSqlLiteRepository<IMLotteryBetLog>
    {
        public IMVRTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.IMVR;

        public override string ProfitlossTableName => "IMVRProfitLossInfo";

        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                 KeyId NVARCHAR(100) PRIMARY KEY,           --抽象欄位, 主索引資料, 不可重複
                 TPGameAccount NVARCHAR(50),                --抽象欄位, 第三方帳號名稱
                 BetId NVARCHAR(50) NULL,                   --产品供应商提供的注单号                 
                 GameNoId NVARCHAR(50),                     --子單號
                 Provider NVARCHAR(50) NULL,                --产品名，VR or SG WIN
                 GameId NVARCHAR(50) NULL,                  --彩种ID
                 GameName NVARCHAR(50) NULL,                --彩种英文名
                 ChineseGameName NVARCHAR(50) NULL,         --彩种中文名
                 GameNo NVARCHAR(50) NULL,                  --期号
                 PlayerId NVARCHAR(50) NULL,                --用户名
                 ProviderPlayerId NVARCHAR(50) NULL,        --在第三方的用户名
                 Currency NVARCHAR(50) NULL,                --弊别
                 Tray NVARCHAR(50) NULL,                    --盘口(奖金组)                 
                 BetOn NVARCHAR(50) NULL,                   --下注玩法
                 BetType NVARCHAR(50) NULL,                 --下注内容
                 BetDetails NVARCHAR(500) NULL,             --下注内容
                 Odds NVARCHAR(50) NULL,                    --奖金赔率
                 BetAmount REAL NULL,                       --投注金额
                 ValidBet REAL NULL,                        --有效金额
                 WinLoss REAL NULL,                         --盈亏金额
                 PlayerWinLoss REAL NULL,                   --盈亏金额(仅适用于 VR 彩票)
                 LossPrize REAL NULL,                       --重新颁奖损失
                 Tips REAL NULL,                            --打赏(贡献)金额
                 CommissionRate REAL NULL,                  --返水百分比，仅适用于SG双赢彩票
                 Commission REAL NULL,                      --返水金额（产品供应商提供）仅适用于SHICAI彩票
                 Status NVARCHAR(50) NULL,                  --注单状态: Open, Settled, Cancelled, Adjusted.
                 Platform NVARCHAR(50) NULL,                --装置(Desktop, mobile)
                 BetDate NVARCHAR(50) NULL,                 --下注时间
                 ResultDate NVARCHAR(50) NULL,              --结算时间
                 SettlementDate NVARCHAR(50) NULL,          --结算时间，此属性将会取代 ResultDate 属性。
                 ReportingDate NVARCHAR(50) NULL,           --报表时间。用于计算报表的时间。
                 DateCreated NVARCHAR(50) NULL,             --IMOne 系统在收到下注单是的创建时间戳
                 LastUpdatedDate NVARCHAR(50) NULL,         --IMOne 系统对注单的最后跟新时间
                 Memo NVARCHAR(500) NULL,
                 LocalSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                 RemoteSaved INTEGER default 0 NOT NULL,
                 RemoteSavedTime TIMESTAMP NULL,
                 RemoteSaveTryCount INTEGER default 0 NOT NULL,
                 RemoteSaveLastTryTime TIMESTAMP NULL)",
            $"CREATE UNIQUE INDEX {ProfitlossTableName}_idx_Id ON {ProfitlossTableName}(BetId ASC, GameNoId ASC);"
        };
    }
}
