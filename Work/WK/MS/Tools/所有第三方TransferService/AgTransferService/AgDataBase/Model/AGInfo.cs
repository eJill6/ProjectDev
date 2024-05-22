using System;

namespace AgDataBase.Model
{
    public class AGInfo : BaseAGInfoModel, IComparable
    {
        public int Id { get; set; }

        public string billNo { get; set; }          //下注唯一编号

        public string dataType { get; set; }        //单据类型，BR：真人，EBR：电子游戏

        public string mainbillno { get; set; }      //电子游戏特有，主单号，订单编号与主单号不一致，则是免费游戏

        public string playerName { get; set; }      //用户账号

        public decimal beforeCredit { get; set; }  //下注前用户余额

        public decimal betAmount { get; set; }     //下注总金额

        /// <summary>
        /// 有效下注总金额，
        /// 若为真人，算法为：
        ///    如果該注贏錢，有效投注額=該注贏的金額
        ///    如果該注輸錢，有效投注額=該注輸的金額
        ///    若是和局，有效投注額=0
        ///
        /// 若为电子游戏，算法为：
        ///   若是付费电子游戏，有效投注额=下注金额
        ///   若是免费电子游戏，有效投注額=0
        /// </summary>
        public decimal validBetAmount { get; set; }

        public decimal netAmount { get; set; }     //亏赢，负数代表用户亏，正数代表用户赢

        public DateTime betTime { get; set; }       //下注时间

        public DateTime? recalcuTime { get; set; }  //重新派奖时间

        public string @round { get; set; }          //平台内大厅类型

        public string gameType { get; set; }        //游戏类型

        public string playType { get; set; }        //游戏玩法

        public string tableCode { get; set; }       //桌子编号，电子游戏为空

        public string gameCode { get; set; }        //游戏局号

        /// <summary>
        /// 1
        /// 已结算
        /// 0
        /// 未结算
        /// -1
        /// 重置试玩额度
        /// -2
        /// 注单被篡改
        /// -8
        /// 取消指定局注单
        /// -9
        /// 取消注单
        /// </summary>
        public string flag { get; set; }

        public int CompareTo(object obj)
        {
            int res = 0;
            AGInfo sObj = (AGInfo)obj;
            if (this.betTime > sObj.betTime)
            {
                res = 1;
            }
            else if (this.betTime < sObj.betTime)
            {
                res = -1;
            }

            return res;
        }

        public string RoundName
        {
            get
            {
                if (AGConstParams.Rounds.ContainsKey(round))
                {
                    return AGConstParams.Rounds[round];
                }

                return null;
            }
        }

        public string GameTypeName
        {
            get
            {
                if (AGConstParams.GameTypes.ContainsKey(gameType))
                {
                    return AGConstParams.GameTypes[gameType];
                }

                return null;
            }
        }

        public string PlayTypeName
        {
            get
            {
                if (AGConstParams.PlayTypes.ContainsKey(playType))
                {
                    return AGConstParams.PlayTypes[playType];
                }

                return null;
            }
        }

        public override bool IsIgnoreAddProfitLoss => dataType == "TR";

        public string MiseOrderGameId { get; set; }
    }
}