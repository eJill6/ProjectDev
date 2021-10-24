using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Model.ThirdParty.OB;
using JxBackendService.Model.ThirdParty.OB.OBFI;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class OBFITransferSqlLiteRep : BaseTransferSqlLiteRepository<OBFIBetLog>
    {
        public OBFITransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.OBFI;

        public override string ProfitlossTableName => "OBFIProfitLossInfo";

        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                 KeyId NVARCHAR(100) PRIMARY KEY,           --抽象欄位, 主索引資料, 不可重複
                 TPGameAccount NVARCHAR(50),                --抽象欄位, 第三方帳號名稱
                 bi INTEGER NULL,                           --注单ID
                 mi INTEGER NULL,                           --商户 id 
                 mmi NVARCHAR(50) NULL,                     --玩家账号(用户名)
                 st INTEGER NULL,                           --投注时间 
                 et INTEGER NULL,                           --结算时间
                 gd INTEGER NULL,                           --游戏桌号 
                 gi INTEGER NULL,                           --游戏 id 
                 gn NVARCHAR(50) NULL,                      --游戏名称
                 gt INTEGER NULL,                           --房间类型(1：初级，2：中级...) 
                 gr NVARCHAR(50) NULL,                      --游戏房间
                 mw REAL NULL,                              --输赢金额 
                 mp REAL NULL,                              --抽水金额 
                 bc REAL NULL,                              --有效投注 
                 dt INTEGER NULL,                           --终端设备类型（0:web,1:h5,2：ios,3:android）
                 tb REAL NULL,                              --总投注金额 
                 cn NVARCHAR(50) NULL,                      --局号：用于游戏内、后台展示和查询 
                 gf INTEGER NULL,                           --游戏分类标记（0:捕鱼类，1:电子类）
                 Memo NVARCHAR(500) NULL,
                 LocalSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                 RemoteSaved INTEGER default 0 NOT NULL,
                 RemoteSavedTime TIMESTAMP NULL,
                 RemoteSaveTryCount INTEGER default 0 NOT NULL,
                 RemoteSaveLastTryTime TIMESTAMP NULL)",
            $"CREATE UNIQUE INDEX {ProfitlossTableName}_idx_Id ON {ProfitlossTableName}(bi ASC);"
        };
    }
}
