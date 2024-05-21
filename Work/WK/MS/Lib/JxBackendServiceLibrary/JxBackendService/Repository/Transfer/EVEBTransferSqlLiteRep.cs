using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.EVO;
using JxBackendService.Repository.Base;

namespace JxBackendService.Repository.Transfer
{
    public class EVEBTransferSqlLiteRep : BaseTransferSqlLiteRepository<EVEBBetLog>
    {
        public EVEBTransferSqlLiteRep() { }

        public override PlatformProduct Product => PlatformProduct.EVEB;

        public override string ProfitlossTableName => "EVEBProfitLossInfo";

        public override string[] CreateProfitlossTableSqls => new string[]
        {
           $@"CREATE TABLE {ProfitlossTableName}(
                 KeyId NVARCHAR(100) PRIMARY KEY,                               --抽象欄位, 主索引資料, 不可重複
                 TPGameAccount NVARCHAR(50),                                    --抽象欄位, 第三方帳號名稱
                 BetId NVARCHAR(50) NULL,                                       --注單單號
                 MemberAccount NVARCHAR(50) NULL,                               --會員帳號
                 WagersTime NVARCHAR(50) NULL,                                  --投注時間
                 BetAmount REAL NULL,                                           --投注金額
                 Payoff REAL NULL,                                              --派彩金額
                 Commissionable REAL NULL,                                      --有效投注
                 UpdateTime NVARCHAR(50) NULL,                                  --更新時間
                 GameId NVARCHAR(50) NULL,                                      --局号
                 GameStartedAt NVARCHAR(50) NULL,                               --局开始时间
                 GameSettledAt NVARCHAR(50) NULL,                               --局结束时间
                 GameStatus NVARCHAR(50) NULL,                                  --游戏状态
                 GameType NVARCHAR(50) NULL,                                    --游戏类型
                 TableId NVARCHAR(50) NULL,                                     --桌号
                 TableName NVARCHAR(50) NULL,                                   --桌名称
                 DealerUid NVARCHAR(50) NULL,                                   --荷官ID
                 DealerName NVARCHAR(50) NULL,                                  --荷官名称
                 GameCurrency NVARCHAR(50) NULL,                                --游戏货币代号
                 GameWager REAL NULL,                                           --局总投注额
                 GamePayout REAL NULL,                                          --局总返回额
                 GameResult NVARCHAR(5000) NULL,                                --游戏结果
                 BetCode NVARCHAR(50) NULL,                                     --下注代号
                 BetStake NVARCHAR(50) NULL,                                    --投注额
                 BetPayout NVARCHAR(50) NULL,                                   --返回额
                 BetPlacedOn NVARCHAR(50) NULL,                                 --下注时间
                 BetDescription NVARCHAR(50) NULL,                              --下注描述
                 BetTransactionId NVARCHAR(50) NULL,                            --下注唯一值
                 --ParticipantScreenName NVARCHAR(50) NULL,                       --玩家昵称
                 --ParticipantCasinoId NVARCHAR(50) NULL,                         --娛樂城約定值
                 --ParticipantSeats NVARCHAR(50) NULL,                            --座位(json SeatsInfo)
                 --ParticipantConfigOverlays NVARCHAR(50) NULL,                   --Array of configuration overlay id’s (internal)(json List<string>)
                 --ParticipantDecisions NVARCHAR(50) NULL,                        --下注说明(json List<Decision>)
                 --ParticipantSessionId NVARCHAR(50) NULL,                        --会话号
                 --ParticipantPlayerId NVARCHAR(50) NULL,                         --游戏帐号
                 --ParticipantCurrency NVARCHAR(50) NULL,                         --货币代号
                 --ParticipantSideBetPlayerPair NVARCHAR(50) NULL,                --PlayerPair下注结果
                 --ParticipantSideBetBankerPair NVARCHAR(50) NULL,                --PlayerPair下注结果
                 --ParticipantSideBetPerfectPair NVARCHAR(50) NULL,               --PerfectPair下注结果
                 --ParticipantSideBetEitherPair NVARCHAR(50) NULL,                --EitherPair下注结果
                 --ParticipantSideBetPlayerBonus NVARCHAR(50) NULL,               --ParticipantSideBetPlayerBonus
                 --ParticipantSideBetBankerBonus NVARCHAR(50) NULL,               --BankerBonus下注结果
                 --ParticipantSideBetBonusBet NVARCHAR(50) NULL,                  --BonusBet下注结果(json Sidebet)
                 --ParticipantSideBet5P1 NVARCHAR(50) NULL,                       --5P1下注结果(json Sidebet)
                 --ParticipantSideBetPairPlus NVARCHAR(50) NULL,                  --PairPlus下注结果(json Sidebet)
                 --ParticipantSideBet6CardBonus NVARCHAR(50) NULL,                --6CardBonus下注结果(json Sidebet)
                 --ParticipantSideBetPairOrBetter NVARCHAR(50) NULL,              --PairOrBetter下注结果(json Sidebet)
                 --ParticipantSideBetTrips NVARCHAR(50) NULL,                     --BetTrips下注结果(json Sidebet)
                 --ParticipantSideBetBestFive NVARCHAR(50) NULL,                  --BestFive下注结果(json Sidebet)
                 --ParticipantSideBetAaBonus NVARCHAR(50) NULL,                   --AaBonus下注结果(json Sidebet)
                 WagersId NVARCHAR(50) NULL,                                    --注單唯一值($'BetTransactionId_BetCode')
                 --ParticipantAamsParticipationId NVARCHAR(50) NULL,              --AAMS参与ID。适用于AAMS监管的游戏
                 --ParticipantAamsSessionId NVARCHAR(50) NULL,                    --AAMS会话ID。适用于AAMS监管的游戏
                 --ParticipantFreebet NVARCHAR(50) NULL,                          --玩家决定免费投注。适用于FreeBet二十一点
                 --ParticipantCasinoSessionId NVARCHAR(50) NULL,                  --会话号(Licensee)
                 --ParticipantChannel NVARCHAR(50) NULL,                          --desktop、mobile、other
                 --ParticipantDevice NVARCHAR(50) NULL,                           --裝置
                 --ParticipantOs NVARCHAR(50) NULL,                               --作業系統
                 --ParticipantBetCoverage NVARCHAR(50) NULL,                      --代表特定玩家获胜的概率。 [0，1]范围内的值
                 --ParticipantRewardBets NVARCHAR(50) NULL,                       --奖励游戏的玩家下注详细信息数组
                 --ParticipantPlayMode NVARCHAR(50) NULL,                         --播放器的播放模式(如果等于RewardGames或PlayForFun，则下注将在rewardBets数组中)
                 --ParticipantSideBetSuperSix NVARCHAR(50) NULL,                  --百家乐速成6额外边注的结果
                 --ParticipantQualificationSpin NVARCHAR(50) NULL,                --Deal Or No Deal 旋轉資格結果
                 --ParticipantQualifiedAt NVARCHAR(50) NULL,                      --Deal Or No Deal 玩家通過的日期及資格時間
                 --ParticipantBoxes NVARCHAR(50) NULL,                            --Deal Or No Deal 內容
                 --ParticipantTopUpSpins NVARCHAR(50) NULL,                       --Deal Or No Deal 旋轉結果
                 --ParticipantOffers NVARCHAR(50) NULL,                           --Deal Or No Deal 優惠結果
                 --ParticipantUseNewBetCodes NVARCHAR(50) NULL,                   --超级球的下注代码类型
                 --ParticipantBetStakePerCard NVARCHAR(50) NULL,                  --每张Mega Ball卡的赌注
                 --ParticipantCards NVARCHAR(50) NULL,                            --超级球抽奖卡数
                 --ParticipantTotalMultiplier NVARCHAR(50) NULL,                  --Crazy Time内的奖金金额乘数
                 --ParticipantBonus NVARCHAR(50) NULL,                            --Crazy Time参与者获得的奖金
                 --ParticipantLeftOnRoll NVARCHAR(50) NULL,                       --Craps：如果玩家提早离开，则掷出该玩家离开的ID	
                 --ParticipantSubType NVARCHAR(50) NULL,                          --适用于特定参与者的游戏规则
                 Memo NVARCHAR(500) NULL,                                       
                 LocalSavedTime TIMESTAMP default CURRENT_TIMESTAMP NOT NULL,
                 RemoteSaved INTEGER default 0 NOT NULL,
                 RemoteSavedTime TIMESTAMP NULL,
                 RemoteSaveTryCount INTEGER default 0 NOT NULL,
                 RemoteSaveLastTryTime TIMESTAMP NULL)",
            $"CREATE UNIQUE INDEX {ProfitlossTableName}_idx_Id ON {ProfitlossTableName}(BetId ASC);"
        };
    }
}
