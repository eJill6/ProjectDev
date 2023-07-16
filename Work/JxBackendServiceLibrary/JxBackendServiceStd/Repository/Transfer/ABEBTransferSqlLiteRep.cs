using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.AllBet;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class ABEBTransferSqlLiteRep : BaseTransferSqlLiteRepository<ABEBBetLog>
    {
        public ABEBTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.ABEB;

        public override string ProfitlossTableName => "ABEBProfitLossInfo";

        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                 KeyId NVARCHAR(100) PRIMARY KEY,           --抽象欄位, 主索引資料, 不可重複
                 TPGameAccount NVARCHAR(50),                --抽象欄位, 第三方帳號名稱
                 appType INTEGER NULL,                      --客户端类型(3: H5 手机版,6: H5 电脑版,7: 旗舰版,8: 休闲版)
                 betAmount REAL NULL,                       --投注金额
                 betMethod INTEGER NULL,                    --下注方法
                 betNum INTEGER,                            --注单编号
                 betTime NVARCHAR(50) NULL,                 --投注时间
                 betType INTEGER NULL,                      --投注类型
                 client NVARCHAR(50) NULL,                  --玩家用户名
                 commission INTEGER NULL,                   --桌台类型(100 非免佣,101 免佣)
                 currency NVARCHAR(50) NULL,                --币种
                 deposit REAL NULL,                         --预扣金额
                 exchangeRate REAL NULL,                    --汇率
                 gameResult NVARCHAR(50) NULL,              --开牌结果
                 gameRoundEndTime NVARCHAR(50) NULL,        --游戏局结束时间
                 gameRoundId INTEGER NULL,                  --游戏局编号
                 gameRoundStartTime NVARCHAR(50) NULL,      --游戏局开始时间
                 gameType INTEGER NULL,                     --游戏类型
                 ip NVARCHAR(50) NULL,                      --玩家IP地址
                 state INTEGER NULL,                        --注单状态
                 status INTEGER NULL,                       --注单状态 (新)
                 tableName NVARCHAR(50) NULL,               --桌台名称
                 validAmount REAL NULL,                     --有效投注金额
                 winOrLoss REAL NULL,                       --输赢金额
                 Memo NVARCHAR(500) NULL,
                 LocalSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                 RemoteSaved INTEGER default 0 NOT NULL,
                 RemoteSavedTime TIMESTAMP NULL,
                 RemoteSaveTryCount INTEGER default 0 NOT NULL,
                 RemoteSaveLastTryTime TIMESTAMP NULL)",
            $"CREATE UNIQUE INDEX {ProfitlossTableName}_idx_Id ON {ProfitlossTableName}(betNum ASC);"
        };
    }
}
