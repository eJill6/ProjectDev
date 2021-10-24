using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.Base;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Repository.Transfer
{
    public class IMBGTransferSqlLiteRep : BaseTransferSqlLiteRepository<BaseRemoteBetLog>
    {
        public IMBGTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.IMBG;

        public override string ProfitlossTableName => "IMBGProfitLossInfo";

        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                Id INTEGER PRIMARY KEY,         --注单号（玩家下注记录标识，全平台唯一值，不会重复）
                AgentId NVARCHAR(50) NULL,      --代理商编号
                UserCode NVARCHAR(50) NULL,     --代理商玩家标示
                GameId INTEGER NULL,            --游戏 ID
                RoomId INTEGER NULL,            --房间 ID
                DealId NVARCHAR(50) NULL,       --牌局号
                DeskId INTEGER NULL,            --桌子号
                SeatId INTEGER NULL,            --座位号
                InitMoney NVARCHAR(50) NULL,    --初始分数
                Money NVARCHAR(50) NULL,        --结算后分数
                TotalBet NVARCHAR(50) NULL,     --总下注
                EffectBet NVARCHAR(50) NULL,    --有效下注—保留参数
                WinLost NVARCHAR(50) NULL,      --输赢分数   
                WinLostAbs NVARCHAR(50) NULL,   --输赢分数的绝对值
                Fee NVARCHAR(50) NULL,          --抽水额
                PayAmount NVARCHAR(50) NULL,    --派彩额
                AllBills NVARCHAR(50) NULL,     --有效下注=总输+总赢，棋牌可以根据这个值进行返水
                AllLost NVARCHAR(50) NULL,      --总输=所有亏损的下注位亏损额度总和的绝对值
                AllWin NVARCHAR(50) NULL,       --总赢=所有赢的下注位盈利额度总和
                OpenTime NVARCHAR(50) NULL,     --开局时间
                EndTime NVARCHAR(50) NULL,      --结束时间
                Memo NVARCHAR(1024) NULL,       --備註
                localSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                remoteSaved INTEGER default 0 NOT NULL,
                remoteSavedTime TIMESTAMP NULL,
                remoteSaveTryCount INTEGER default 0 NOT NULL,
                remoteSaveLastTryTime TIMESTAMP NULL
                )",
            $"CREATE UNIQUE INDEX {ProfitlossTableName}_idx_Id ON {ProfitlossTableName}(Id ASC);"
        };
    }
}
