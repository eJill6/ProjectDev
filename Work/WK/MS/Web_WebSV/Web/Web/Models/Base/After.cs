using System;
using System.Collections.Generic;
using JxLottery.Adapters.Models.Lottery;

namespace Web.Models.Base
{
    public enum AfterType
    {
        /// <summary>
        /// 利润率追号
        /// </summary>
        Profit = 0,
        /// <summary>
        /// 同倍追号
        /// </summary>
        SameMultilple = 1,
        /// <summary>
        /// 翻倍追号
        /// </summary>
        DoubleMultilple = 2
    }

    public class After
    {
        public static KIND GetKind(int lotid)
        {
            switch (lotid)
            {
                case 1:
                    return KIND.CQ_SSC;
                case 2:
                    return KIND.JX_SSC;
                case 3:
                    return KIND.THREE_D;
                case 4:
                    return KIND.P_THREE;
                case 6:
                    return KIND.GD_ELE;
                case 9:
                    return KIND.CQ_ELE;
                case 11:
                    return KIND.SD_ELE;
                case 12:
                    return KIND.BJ_PK10;
                case 13:
                    return KIND.HJ_SSC;
                case 14:
                    return KIND.HJ_ELE;
                case 15:
                    return KIND.XJ_SSC;
                case 16:
                    return KIND.HJ_CQ;
                case 17:
                    return KIND.HJ_PK10;
                case 18:
                    return KIND.JS_KS;
                case 21:
                    return KIND.TJ_SSC;
                case 23:
                    return KIND.BingGo;
                case 24:
                    return KIND.Keno;
                case 25:
                    return KIND.Korea5;
                case 26:
                    return KIND.De_PK10;
                case 27:
                    return KIND.QQ_SSC;
                case 28:
                    return KIND.Italy_PK10;
                case 29:
                    return KIND.Italy_SSC;
                case 30:
                    return KIND.HLJSSC;
                case 31:
                    return KIND.FRKS;
                case 32:
                    return KIND.LKA_PK10;
                case 33:
                    return KIND.QQ5;
                case 34:
                    return KIND.VNS_PK10;
                case 35:
                    return KIND.QQRC_PK10;
                case 36:
                    return KIND.QQRC5_PK10;
                case 37:
                    return KIND.MQQ;
                case 38:
                    return KIND.MQQ5;
                case 39:
                    return KIND.FHQQ;
                case 40:
                    return KIND.WeixinQQ;
                case 41:
                    return KIND.Jx11x5;
                case 42:
                    return KIND.Js11x5;
                case 43:
                    return KIND.VietSSC;
                case 46:
                    return KIND.ChichuQQ;
                default:
                    return 0;
            }
        }

        public enum KIND
        {
            CQ_SSC,//重庆时时彩140411001 最大120
            JX_SSC, //江西时时彩84
            XJ_SSC, //新疆时时彩96
            THREE_D, //3d
            P_THREE, //p3
            GD_ELE,//11.5广东84
            BJ_PK10,//最大150期
            De_PK10,//最大150期
            Italy_PK10,//義大利PK10
            QQ_SSC,//腾讯分分彩
            Italy_SSC,//義大利SSC
            /// <summary>
            /// 山东十一选五87期
            /// </summary>
            SD_ELE, // 山东11选5 87期
            CQ_ELE,//重庆十一选五85期
            HJ_ELE,//和记十一选五
            HJ_SSC,//和记时时彩
            HJ_CQ,
            HJ_PK10,
            JS_KS,
            TJ_SSC,
            BingGo,
            Keno,
            Korea5,
            HLJSSC,// 黑龍江时时彩
            FRKS,//法國快三
            LKA_PK10,//法國快三
            QQ5, // 腾讯5分彩
            VNS_PK10,    //威尼斯飞艇
            QQRC_PK10,    //腾讯赛车分分彩
            QQRC5_PK10,    //腾讯赛车5分彩
            MQQ, // QQ分分彩
            MQQ5, // QQ5分彩            
            FHQQ, // 鳳凰騰訊分分彩
            WeixinQQ, // 微信分分彩
            Jx11x5, // 江西11選5
            Js11x5,  // 江蘇11選5
            VietSSC, // 河內分分彩
            ChichuQQ, // 奇趣腾讯分分彩
        };

        /// <summary>
        /// 產生與時間關聯的追號的期號
        /// </summary>
        /// <param name="totalIssueCount">要追的期數</param>
        /// <param name="currentIssueNo">當前期號</param>
        /// <param name="diffMinute">间隔分钟</param>
        /// <param name="issueFormat">期号格式</param>
        /// <returns>
        /// 追号清单
        /// </returns>
        static List<string> CreatePeriodsWithDateTimeFormat(int totalIssueCount, string currentIssueNo, int diffMinute, string issueFormat)
        {
            List<string> afterResults = new List<string>();

            DateTime startIssueTime = new DateTime();

            int yearLen = issueFormat.IndexOf("M");
            string yearStr = DateTime.Now.Year.ToString();
            int shiftYearLen = 0;
            if (yearLen == 2)
            {
                yearStr = DateTime.Now.Year.ToString().Substring(0, 2) + currentIssueNo.Substring(0, 2);
                shiftYearLen = 2;
            }
            else if (yearLen == 4)
            {
                yearStr = currentIssueNo.Substring(0, 4);
            }

            string startIssueStr =
                yearStr + "-" +
                currentIssueNo.Substring(4 - shiftYearLen, 2) + "-" +
                currentIssueNo.Substring(6 - shiftYearLen, 2) + " " +
                currentIssueNo.Substring(8 - shiftYearLen, 2) + ":" +
                currentIssueNo.Substring(10 - shiftYearLen, 2) +
                ":00";

            if (DateTime.TryParse(startIssueStr, out startIssueTime))
            {
                for (int i = 0; i < totalIssueCount; i++)
                {
                    string issueNo = startIssueTime.ToString(issueFormat);

                    afterResults.Add(issueNo);
                    startIssueTime = startIssueTime.AddMinutes(diffMinute);
                }
            }

            return afterResults;
        }

        /// <summary>
        /// 產生與時間關聯的追號 + 连续号
        /// </summary>
        /// <param name="totalIssueCount">要追的期數</param>
        /// <param name="maxPeriods">最大期数</param>
        /// <param name="currentIssueNo">當前期號</param>
        /// <param name="issueFormat">期号格式</param>
        /// <param name="snoLen">流水号长度</param>
        /// <param name="startIssueNumber">第一期数字(取尾数一码)。例：001 = 1 or 000 = 0</param>
        /// <param name="currentTime">目前追号的时间</param>
        /// <returns>
        /// 追号清单
        /// </returns>
        static List<string> CreatePeriodsWithDateTimeFormatAndSNO(int totalIssueCount,
            int maxPeriods, long currentIssueNo, string issueFormat, int snoLen, string startIssueNumber, DateTime currentTime)
        {
            List<string> afterResults = new List<string>();

            long modLength = (long)Math.Pow(10, snoLen);
            long h = currentIssueNo % modLength;

            if (totalIssueCount + h <= maxPeriods)
            {
                for (int i = 0; i < totalIssueCount; i++)
                {
                    afterResults.Add((currentIssueNo++).ToString());
                }
            }
            else
            {
                string nextDayStr = currentTime.AddDays(1).Date.ToString(issueFormat) + startIssueNumber.PadLeft(snoLen, '0');
                long nextDayIssue = Convert.ToInt64(nextDayStr);
                for (int i = 0; i < totalIssueCount; i++)
                {
                    long x = h++;
                    if (x <= maxPeriods)
                    {
                        afterResults.Add((currentIssueNo++).ToString());
                    }
                    else
                    {
                        afterResults.Add((nextDayIssue++).ToString());
                    }
                }
            }
            return afterResults;
        }

        /// <summary>
        /// 生产期号
        /// </summary>
        /// <param name="num">要生产的期数</param>
        /// <param name="k">彩种种类</param>
        /// <param name="currentIssueNo">当前期</param>
        /// <returns></returns>
        static public List<string> CreatePeriods(int num, KIND k, string currentIssueNo, DateTime time, int lotteryType)
        {
            var result = new List<string>();
            long n;
            if (!long.TryParse(currentIssueNo, out n))
            {
                return result;
            }

            if (k == KIND.CQ_SSC && lotteryType == (int)LotteryId.CQSSC)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 59, n, "yyMMdd", 3, "1", time));
            }
            else if (k == KIND.TJ_SSC)
            {
                // 20190209 春節後改為20分鐘一期
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 42, n, "yyyyMMdd", 3, "1", time));
            }
            else if (k == KIND.JX_SSC)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 84, n, "yyyyMMdd", 3, "1", time));
            }
            else if (k == KIND.XJ_SSC)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 48, n, "yyyyMMdd", 2, "1", time));
            }
            else if (k == KIND.GD_ELE)
            {
                // 20190209 春節後改為20分鐘一期
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 42, n, "yyMMdd", 2, "1", time));
            }
            else if (k == KIND.CQ_ELE)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 85, n, "yyMMdd", 2, "1", time));
            }
            else if (k == KIND.SD_ELE)
            {
                // 20190209 春節後改為20分鐘一期 
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 43, n, "yyMMdd", 2, "1", time));
            }
            else if (k == KIND.De_PK10)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 960, n, "yyyyMMdd", 3, "1", time));
            }
            else if (k == KIND.VNS_PK10)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 960, n, "yyMMdd", 3, "1", time));
            }
            else if (k == KIND.FRKS ||
                lotteryType == (int)LotteryId.Viet5SSC ||
                lotteryType == (int)LotteryId.VR5SSC ||
                lotteryType == (int)LotteryId.VR5PK10 ||
                lotteryType == (int)LotteryId.VR5K3 ||
                lotteryType == (int)LotteryId.VR511X5
                )
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 288, n, "yyyyMMdd", 3, "1", time));
            }
            else if (k == KIND.JS_KS)
            {
                // 20190209 春節後改為20分鐘一期
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 41, n, "yyMMdd", 3, "1", time));
            }
            else if (k == KIND.LKA_PK10)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 180, n, "yyyyMMdd", 3, "1", time));
            }
            else if (k == KIND.Italy_SSC || k == KIND.Italy_PK10 || k == KIND.VietSSC || lotteryType == (int)LotteryId.GugeSSC || lotteryType == (int)LotteryId.YTSSC)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 1440, n, "yyyyMMdd", 4, "1", time));
            }
            else if (k == KIND.FHQQ)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 1440, n, "yyMMdd", 4, "1", time));
            }
            else if (k == KIND.QQ_SSC || k == KIND.QQRC_PK10 || k == KIND.MQQ || k == KIND.WeixinQQ || k == KIND.ChichuQQ)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormat(num, currentIssueNo, 1, "yyMMddHHmm"));
            }
            else if (k == KIND.QQ5 || k == KIND.QQRC5_PK10 || k == KIND.MQQ5 || lotteryType == (int)LotteryId.ChichuQQ5)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormat(num, currentIssueNo, 5, "yyMMddHHmm"));
            }
            else if (k == KIND.THREE_D || k == KIND.P_THREE || k == KIND.BJ_PK10 || k == KIND.HJ_SSC || //福彩3d、体彩排列3、北京pk10、{ProductName} ssc
                     k == KIND.HJ_ELE || k == KIND.HJ_CQ || k == KIND.HJ_PK10 || k == KIND.BingGo ||    //{ProductName} 11x5、{ProductName}秒秒、pk10秒秒、台湾5分彩
                     k == KIND.Keno || k == KIND.Korea5 || k == KIND.HLJSSC || //北京快乐8、韩国5分彩、意大利pk10、黑龙江时时
                     lotteryType == (int)LotteryId.MOLHC || // 澳門六合彩
                     lotteryType == (int)LotteryId.XGLHC    // 香港六合彩
                     )
            {
                for (int i = 0; i < num; i++)
                {
                    result.Add((n++).ToString());
                }
            }
            else if (k == KIND.Jx11x5)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 42, n, "yyyyMMdd", 2, "1", time));
            }
            else if (k == KIND.Js11x5)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 41, n, "yyyyMMdd", 2, "1", time));
            }
            else if (lotteryType == (int)LotteryId.JL11X5)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 39, n, "yyyyMMdd", 2, "1", time));
            }
            else if (lotteryType == (int)LotteryId.AHK3)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 40, n, "yyyyMMdd", 3, "1", time));
            }
            else if (lotteryType == (int)LotteryId.GSK3)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 36, n, "yyyyMMdd", 3, "1", time));
            }
            else if (lotteryType == (int)LotteryId.VR3SSC)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 420, n, "yyyyMMdd", 3, "1", time));
            }
            else if (lotteryType == (int)LotteryId.VR10SSC)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 126, n, "yyyyMMdd", 3, "1", time));
            }
            else if (lotteryType == (int)LotteryId.XY28)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 270, n, "yyyyMMdd", 3, "1", time));
            }
            else if (lotteryType == (int)LotteryId.VR11X5)
            {
                result.AddRange(CreatePeriodsWithDateTimeFormatAndSNO(num, 1260, n, "yyyyMMdd", 4, "1", time));
            }

            return result;
        }

        /// <summary>
        /// 盈利率追号
        /// </summary>
        /// <param name="num">要追的期数</param>
        /// <param name="Pro">盈利率</param>
        /// <param name="betmoney">单次投注金额，比如是1元或者0.1元</param>
        /// <param name="singBetMoney">单笔投注金额，是这单的总金额</param>
        /// <param name="prize">单笔中奖奖金</param>
        /// <param name="m">初始倍数</param>
        /// <returns>返回符合盈利率的投注金额和倍数集合</returns>
        static public List<ZhStrct> ProfitRate(int num, double Pro, double betmoney, double singBetMoney, double prize, int m)
        {
            // 20180714 Yark GD提說不要擋住追號，所以將中獎金額<投注本金時，提示用戶賠本訊息，但仍給予追號投注
            // 拉到上一層進行判斷
            //if ((betmoney * prize - singBetMoney) < 0)
            //{
            //	return null;
            //}
            List<ZhStrct> zh = new List<ZhStrct>();

            for (int i = 0; i < num; i++)
            {
                double AllMoney = 0;
                foreach (ZhStrct t in zh)
                {
                    AllMoney = AllMoney + t.Money;
                }
                ZhStrct z = new ZhStrct();
                z.Num = i;

                // 20180722 Yark 加入有利潤才進行計算
                if ((betmoney * prize - singBetMoney) > 0)
                {
                    while ((betmoney * m * prize - (AllMoney + singBetMoney * m)) / (AllMoney + singBetMoney * m) < Pro)
                    {
                        m++;
                        if (betmoney * m * prize > 300000)
                        {
                            m = 0;
                            break;
                        }
                    }
                }

                z.Multiple = m == 0 ? 0 : m;
                z.Money = singBetMoney * m;
                zh.Add(z);
            }
            return zh;
        }

        /// <summary>
        /// 同倍追号
        /// </summary>
        /// <param name="num">要追的期数</param>
        /// <param name="singBetMoney">本单金额</param>
        /// <param name="m">起始倍数</param>
        /// <returns></returns>
        static public List<ZhStrct> ProfitRate(int num, double singBetMoney, int m)
        {
            List<ZhStrct> zh = new List<ZhStrct>();
            for (int i = 0; i < num; i++)
            {
                ZhStrct z = new ZhStrct();
                z.Money = singBetMoney * m;
                z.Multiple = m;
                zh.Add(z);
                if (singBetMoney * m > 100000)
                {
                    m = 0;
                    break;
                }
            }
            return zh;
        }

        /// <summary>
        /// 翻倍追号
        /// </summary>
        /// <param name="num">要追的期数</param>
        /// <param name="singBetMoney">本单金额</param>
        /// <param name="m">倍数</param>
        /// <param name="Pre">间隔期数</param>
        /// <returns></returns>
        static public List<ZhStrct> ProfitRate(int num, int Pre, double singBetMoney, int m)
        {
            int checkNum = 0;
            List<ZhStrct> zh = new List<ZhStrct>();
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < Pre; j++)
                {
                    checkNum++;
                    if (checkNum > num) break;

                    ZhStrct z = new ZhStrct();

                    if (singBetMoney * Math.Pow(m, i) > 100000)
                    {
                        z.Money = 0;
                        z.Multiple = 0;
                        zh.Add(z);
                        continue;
                    }
                    z.Money = singBetMoney * Math.Pow(m, i);
                    z.Multiple = long.Parse(Math.Pow(m, i).ToString());
                    zh.Add(z);
                }
            }
            return zh;
        }

        /// <summary>
        /// 取得可追號期數
        /// </summary>
        /// <param name="lotteryId">彩種代碼</param>
        /// <param name="num">追號數量</param>
        /// <returns>
        /// 回傳期數
        /// </returns>
        public static int GetChaseNumberCount(int lotteryId, int num)
        {
            int result = num;
            switch (lotteryId)
            {
                case (int)LotteryId.CQSSC:        //重庆时时彩
                    if (num > 59)
                    {
                        result = 59;
                    }
                    break;
                case (int)LotteryId.JXSSC:        //江西时时彩
                    if (num > 84)
                    {
                        result = 84;
                    }
                    break;
                case (int)LotteryId.FC3D:         //福彩3D
                case (int)LotteryId.MOLHC:
                case (int)LotteryId.TCPL3:        //体彩排列三
                    if (num > 10)
                    {
                        result = 10;
                    }
                    break;
                case (int)LotteryId.GD115:        //广东十一选五
                case (int)LotteryId.TAIJINSSC:    // 天津时时彩
                case (int)LotteryId.HLJSSC:       //黑龙江时时彩
                case (int)LotteryId.Jx11x5:       //江西11选5 
                    if (num > 42)
                    {
                        result = 42;
                    }
                    break;
                case (int)LotteryId.CQ115:        //重庆十一选五
                    if (num > 85)
                    {
                        result = 85;
                    }
                    break;
                case (int)LotteryId.SD115:        //山东十一选五
                    if (num > 43)
                    {
                        result = 43;
                    }
                    break;
                case (int)LotteryId.JL11X5:
                    if (num > 39)
                    {
                        result = 39;
                    }
                    break;
                case (int)LotteryId.BJPK:         //北京PK10
                    if (num > 44)
                    {
                        result = 44;
                    }
                    break;
                case (int)LotteryId.DEPK:         //德国PK拾
                case (int)LotteryId.VNSPK:      //威尼斯飞艇
                    if (num > 960)
                    {
                        result = 960;
                    }
                    break;
                case (int)LotteryId.XJSSC:        //新疆时时彩
                    if (num > 48)
                    {
                        result = 48;
                    }
                    break;
                case (int)LotteryId.HSSFC:        //{ProductName} 三分彩
                case (int)LotteryId.HSPK:         //{ProductName} PK拾
                    if (num > 480)
                    {
                        result = 480;
                    }
                    break;
                case (int)LotteryId.JSKS:         //江苏快三
                case (int)LotteryId.Js11x5:       //江苏11选5
                    if (num > 41)
                    {
                        result = 41;
                    }
                    break;
                case (int)LotteryId.FRKS:         //法国快三
                case (int)LotteryId.QQ5:          //腾讯5分彩
                case (int)LotteryId.QQRC5PK10:    //腾讯賽車5分彩
                case (int)LotteryId.MQQ5SSC:         //QQ5分彩
                case (int)LotteryId.Viet5SSC:     //河內5分彩
                case (int)LotteryId.ChichuQQ5:
                case (int)LotteryId.VR5SSC:
                case (int)LotteryId.VR5PK10:
                case (int)LotteryId.VR5K3:
                case (int)LotteryId.VR511X5:
                    if (num > 288)
                    {
                        result = 288;
                    }
                    break;
                case (int)LotteryId.HSSSC:        //{ProductName} 时时彩
                case (int)LotteryId.HS115:        //{ProductName} 十一选五
                case (int)LotteryId.QQSSC:        //腾讯分分彩
                case (int)LotteryId.ITALYPK:      //意大利PK拾
                case (int)LotteryId.ITALYSSC:     //意大利分分彩
                case (int)LotteryId.QQRCPK10:     //腾讯賽車分分彩
                case (int)LotteryId.MQQSSC:          //QQ分分彩
                case (int)LotteryId.FHQQSSC:         //鳳凰騰訊分分彩
                case (int)LotteryId.WEIXINQQ:     //微信分分彩
                case (int)LotteryId.VietSSC:      //河內分分彩
                case (int)LotteryId.ChichuQQ:      //奇趣腾讯分分彩
                case (int)LotteryId.GugeSSC: //谷歌分分彩
                case (int)LotteryId.YTSSC: //水管分分彩
                    if (num > 1440)
                    {
                        result = 1440;
                    }
                    break;
                case (int)LotteryId.LKA:      //幸运飞艇
                    if (num > 180)
                    {
                        result = 180;
                    }
                    break;
                case (int)LotteryId.AHK3:
                    if (num > 40)
                    {
                        result = 40;
                    }
                    break;
                case (int)LotteryId.GSK3:
                    if (num > 36)
                    {
                        result = 36;
                    }
                    break;
                case (int)LotteryId.VR3SSC:
                    if (num > 420)
                    {
                        result = 420;
                    }
                    break;
                case (int)LotteryId.VR10SSC:
                    if (num > 126)
                    {
                        result = 126;
                    }
                    break;
                case (int)LotteryId.XY28:
                    if (num > 270)
                    {
                        result = 270;
                    }
                    break;
                case (int)LotteryId.VR11X5: //VR5分十一选五
                    if (num > 1260)
                    {
                        result = 1260;
                    }
                    break;
                default:
                    result = 1;
                    break;
            }

            return result;
        }

    }
}