using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.PG;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class PGSLTransferSqlLiteRep : BaseTransferSqlLiteRepository<PGSLBetLog>
    {
        public PGSLTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.PGSL;

        public override string ProfitlossTableName => "PGSLProfitLossInfo";

        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                 KeyId NVARCHAR(100) PRIMARY KEY,           --抽象欄位, 主索引資料, 不可重複
                 TPGameAccount NVARCHAR(50),                --抽象欄位, 第三方帳號名稱
                 parentBetId INTEGER NULL,                  --母注单的唯一标识符 
                 betId INTEGER,                             --子投注的唯一标识符 （唯一键值） 
                 playerName NVARCHAR(50) NULL,              --玩家的唯一标识符 
                 gameId INTEGER NULL,                       --游戏的唯一标识符 
                 betType INTEGER NULL,                      --投注记录类别： 1: 真实游戏 
                 transactionType INTEGER NULL,              --交易类别： 1: 现金 2: 红利 3: 免费游戏 
                 platform INTEGER NULL,                     --投注记录平台 
                 currency NVARCHAR(50) NULL,                --记录货币 
                 betAmount REAL NULL,                       --玩家的投注额 
                 winAmount REAL NULL,                       --玩家的所赢金额
                 jackpotRtpContributionAmount REAL NULL,    --玩家的rtp奖池贡献额 
                 jackpotContributionAmount REAL NULL,       --玩家的奖池贡献额 
                 jackpotWinAmount REAL NULL,                --玩家的奖池金额 
                 balanceBefore REAL NULL,                   --玩家交易前的余额 
                 balanceAfter REAL NULL,                    --玩家交易后的余额 
                 handsStatus INTEGER NULL,                  --投注状态： 1: 非最后一手投注 2：最后一手投注 3：已调整 
                 rowVersion INTEGER NULL,                   --数据更新时间 （以毫秒为单位的 Unix 时间戳） 
                 betTime INTEGER NULL,                      --当前投注的开始时间 （以毫秒为单位的 Unix 时间戳） 
                 betEndTime INTEGER NULL,                   --当前投注的结束时间 （以毫秒为单位的 Unix 时间戳） 
                 Memo NVARCHAR(500) NULL,
                 LocalSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                 RemoteSaved INTEGER default 0 NOT NULL,
                 RemoteSavedTime TIMESTAMP NULL,
                 RemoteSaveTryCount INTEGER default 0 NOT NULL,
                 RemoteSaveLastTryTime TIMESTAMP NULL)",
            $"CREATE UNIQUE INDEX {ProfitlossTableName}_idx_Id ON {ProfitlossTableName}(betId ASC);"
        };
    }
}
