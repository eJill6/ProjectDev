using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JxLottery.Models.Lottery;

namespace SLPolyGame.Web.Common
{
    public static class ManBase
    {
        /**/
        // /
        // / 转半角的函数(DBC case)
        // /
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }
        /// <summary>
        /// 下单数据格式判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isNumberic(string str)
        {
            str = str.Replace(",", string.Empty);
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);
            for (int i = 0; i <= 9; i++)
            {
                str = str.Replace(i.ToString(), string.Empty);
            }

            if (str.Length == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 下单数据格式判断
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isLH(string str)
        {
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);
            str = str.Replace("龙", string.Empty);
            str = str.Replace("虎", string.Empty);

            if (str.Length == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断大小数据格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isBS(string str)
        {
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);
            str = str.Replace("大", string.Empty);
            str = str.Replace("小", string.Empty);

            if (str.Length == 0)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 判断单双数据格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isSD(string str)
        {
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);
            str = str.Replace("单", string.Empty);
            str = str.Replace("双", string.Empty);

            if (str.Length == 0)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 判断大小单双数据格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isDXDS(string str)
        {
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);
            str = str.Replace("单", string.Empty);
            str = str.Replace("双", string.Empty);
            str = str.Replace("大", string.Empty);
            str = str.Replace("小", string.Empty);
            str = str.Replace(",", string.Empty);

            if (str.Length == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 追号时期号長度检测
        /// </summary>
        /// <param name="lotteryid"></param>
        /// <param name="IssueNo"></param>
        /// <param name="NowIssueNo"></param>
        /// <returns></returns>
        public static bool CheckIssueNo(int lotteryid, string IssueNo, string NowIssueNo)
        {
            try
            {
                long num = 0;
                switch (lotteryid)
                {
                    case 1:
                        if (IssueNo.Length != 9) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(59, NowIssueNo, IssueNo, "yyMMdd", 3, "1");
                    case 2:
                        if (IssueNo.Length != 11) return false;
                        return CheckNo2(lotteryid, IssueNo, NowIssueNo);
                    case 3:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.MOLHC:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.XGLHC:
                        if (IssueNo.Length != 7) return false;
                        return CheckNo2(lotteryid, IssueNo, NowIssueNo);
                    case 4:
                        if (IssueNo.Length != 5) return false;
                        return CheckNo2(lotteryid, IssueNo, NowIssueNo);
                    case 6:
                        if (IssueNo.Length != 8) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(42, NowIssueNo, IssueNo, "yyMMdd", 2, "1");
                    case 9:
                        if (IssueNo.Length != 8) return false;
                        return CheckNo2(lotteryid, IssueNo, NowIssueNo);
                    case 11:
                        if (IssueNo.Length != 8) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(43, NowIssueNo, IssueNo, "yyMMdd", 2, "1");
                    case 12:
                        if (IssueNo.Length != 6) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 150) return false;
                        return true;
                    case 13:
                        if (IssueNo.Length != 10) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 120) return false;
                        return true;
                    case 14:
                        if (IssueNo.Length != 10) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 84) return false;
                        return true;
                    case 15:
                        if (IssueNo.Length != 10) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(48, NowIssueNo, IssueNo, "yyyyMMdd", 2, "1");
                    case 16:
                        if (IssueNo.Length != 10) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 120) return false;
                        return true;
                    case 17:
                        if (IssueNo.Length != 10) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 150) return false;
                        return true;
                    case 18:
                        if (IssueNo.Length != 9) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(41, NowIssueNo, IssueNo, "yyMMdd", 3, "1");
                    case 21:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(42, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case 23:
                        if (IssueNo.Length != 9) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 203) return false;
                        return true;
                    case 24:
                        if (IssueNo.Length != 6) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 179) return false;
                        return true;
                    case 25:
                    case 30:
                        if (IssueNo.Length < 6) return false;
                        num = long.Parse(IssueNo) - long.Parse(NowIssueNo);
                        if (num < 0 || num > 288) return false;
                        return true;
                    case 26:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(960, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case 27:                    
                    case 35:                    
                    case 37:        
                    case 40:
                    case 46:
                        if (IssueNo.Length != 10) return false;                      
                        return VaildMaxPeriodsWithDateTimeFormat(1440, NowIssueNo, IssueNo, 1, "yyMMddHHmm");
                    case 28:
                    case 29:
                    case 43:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.GugeSSC:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.YTSSC:
                        if (IssueNo.Length != 12) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(1440, NowIssueNo, IssueNo, "yyyyMMdd", 4, "1"); 
                    case 31:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.Viet5SSC:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR5SSC:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR5PK10:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR5K3:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR511X5:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(288, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case 32:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(180, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case 33:
                    case 36:
                    case 38:
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.ChichuQQ5:
                        if (IssueNo.Length != 10) return false;
                        return VaildMaxPeriodsWithDateTimeFormat(288, NowIssueNo, IssueNo, 5, "yyMMddHHmm");
                    case 34:
                        if (IssueNo.Length != 9) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(960, NowIssueNo, IssueNo, "yyMMdd", 3, "1");
                    case 39:
                        if (IssueNo.Length != 10) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(1440, NowIssueNo, IssueNo, "yyMMdd", 4, "1");
                    case 41:
                        if (IssueNo.Length != 10) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(42, NowIssueNo, IssueNo, "yyyyMMdd", 2, "1");
                    case 42:
                        if (IssueNo.Length != 10) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(41, NowIssueNo, IssueNo, "yyyyMMdd", 2, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.JL11X5:
                        if (IssueNo.Length != 10) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(39, NowIssueNo, IssueNo, "yyyyMMdd", 2, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.AHK3:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(40, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.GSK3:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(36, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR3SSC:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(420, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR10SSC:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(126, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.XY28:
                        if (IssueNo.Length != 11) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(270, NowIssueNo, IssueNo, "yyyyMMdd", 3, "1");
                    case (int)JxLottery.Adapters.Models.Lottery.LotteryId.VR11X5:
                        if (IssueNo.Length != 12) return false;
                        return VaildMaxPeriodsWithDateTimeFormatAndSNO(1260, NowIssueNo, IssueNo, "yyyyMMdd", 4, "1");
                    default:
                        return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static bool CheckNo2(int lotteryid, string IssueNo, string NowIssueNo)
        {
            int num = 0;
            //当前期时间部分
            string No1 = string.Empty;
            //追号期时间部分
            string No2 = string.Empty;
            //当前期号码部分
            string zNo1 = string.Empty;
            //追号期号码部分
            string zNo2 = string.Empty;
            switch (lotteryid)
            {
                #region 重庆时时彩
                case 1:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 3);
                    zNo2 = IssueNo.Substring(6, 3);
                    //如果当天，则继续验证是否在120期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 120 || num < 0 || int.Parse(zNo2) > 120)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 120 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 120 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 江西时时彩
                case 2:
                    No1 = NowIssueNo.Substring(0, 8);
                    No2 = IssueNo.Substring(0, 8);
                    zNo1 = NowIssueNo.Substring(8, 3);
                    zNo2 = IssueNo.Substring(8, 3);
                    //如果当天，则继续验证是否在84期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 84 || num < 0 || int.Parse(zNo2) > 84)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 84 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 84 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 3D
                case 3:
                case (int)JxLottery.Adapters.Models.Lottery.LotteryId.MOLHC:
                case (int)JxLottery.Adapters.Models.Lottery.LotteryId.XGLHC:
                    No1 = NowIssueNo.Substring(0, 4);
                    No2 = IssueNo.Substring(0, 4);
                    zNo1 = NowIssueNo.Substring(4, 3);
                    zNo2 = IssueNo.Substring(4, 3);
                    //如果当天，则继续验证是否在10期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        //if (num > 365)//此处最多允许追10期，所以判断不大于10即可
                        if (num > 10 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一年
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 365 - int.Parse(zNo1) + int.Parse(zNo2);
                        //if (num > 365)//此处最多允许追10期，所以判断不大于10即可
                        if (num > 10 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region Tc
                case 4:
                    No1 = NowIssueNo.Substring(0, 2);
                    No2 = IssueNo.Substring(0, 2);
                    zNo1 = NowIssueNo.Substring(2, 3);
                    zNo2 = IssueNo.Substring(2, 3);
                    //如果当天，则继续验证是否在365期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        //if (num > 365)//此处最多允许追10期，所以判断不大于10即可
                        if (num > 10 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一年
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 365 - int.Parse(zNo1) + int.Parse(zNo2);
                        //if (num > 365)//此处最多允许追10期，所以判断不大于10即可
                        if (num > 10 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 广东11.5
                case 6:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 2);
                    zNo2 = IssueNo.Substring(6, 2);
                    //如果当天，则继续验证是否在120期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 84 || num < 0 || int.Parse(zNo2) > 84)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 84 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 84 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 重庆11.5
                case 9:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 2);
                    zNo2 = IssueNo.Substring(6, 2);
                    //如果当天，则继续验证是否在120期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 85 || num < 0 || int.Parse(zNo2) > 85)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 85 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 85 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 山东11.5
                case 11:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 2);
                    zNo2 = IssueNo.Substring(6, 2);
                    //如果当天，则继续验证是否在120期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 87 || num < 0 || int.Parse(zNo2) > 87)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 87 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 87 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 新疆时时彩
                case 15:
                    No1 = NowIssueNo.Substring(0, 8);
                    No2 = IssueNo.Substring(0, 8);
                    zNo1 = NowIssueNo.Substring(8, 2);
                    zNo2 = IssueNo.Substring(8, 2);
                    //如果当天，则继续验证是否在84期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 96 || num < 0 || int.Parse(zNo2) > 96)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 96 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 96 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 江苏快三
                case 18:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 3);
                    zNo2 = IssueNo.Substring(6, 3);
                    //如果当天，则继续验证是否在82期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 82 || num < 0 || int.Parse(zNo2) > 82120)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 82 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 82 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion

                #region 天津时时彩
                case 21:
                    No1 = NowIssueNo.Substring(0, 8);
                    No2 = IssueNo.Substring(0, 8);
                    zNo1 = NowIssueNo.Substring(8, 3);
                    zNo2 = IssueNo.Substring(8, 3);
                    //如果当天，则继续验证是否在84期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 84 || num < 0 || int.Parse(zNo2) > 84)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 84 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 84 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion
                #region 法國快三
                case 31:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 3);
                    zNo2 = IssueNo.Substring(6, 3);
                    //如果当天，则继续验证是否在960期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 960 || num < 0 || int.Parse(zNo2) > 82120)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 960 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 960 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion
                #region 威尼斯飛艇
                case 34:
                    No1 = NowIssueNo.Substring(0, 6);
                    No2 = IssueNo.Substring(0, 6);
                    zNo1 = NowIssueNo.Substring(6, 3);
                    zNo2 = IssueNo.Substring(6, 3);
                    //如果当天，则继续验证是否在960期内
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > 960 || num < 0 || int.Parse(zNo2) > 82120)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = 960 - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > 960 || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                #endregion
                #region 腾讯赛车分分彩
                case 35:
                    DateTime startIssueTime = new DateTime();

                    string startIssueStr = 
                        DateTime.Now.Year.ToString().Substring(0, 2) +
                        NowIssueNo.Substring(0, 2) + "-" +
                        NowIssueNo.Substring(2, 2) + "-" +
                        NowIssueNo.Substring(4, 2) + " " +
                        NowIssueNo.Substring(6, 2) + ":" +
                        NowIssueNo.Substring(8, 2) + 
                        ":00";

                    if (DateTime.TryParse(startIssueStr, out startIssueTime))
                    {
                        long maxIssueNo = Convert.ToInt64(startIssueTime.AddMinutes(1440).ToString("yyMMddHHmm"));
                        
                        return maxIssueNo > long.Parse(IssueNo);
                    }
                    else
                    {
                        return false;
                    }
                #endregion

                #region 江西，江蘇11选5
                case (int)Constant.LotteryType.Jx11x5:
                case (int)Constant.LotteryType.Js11x5:
                    No1 = NowIssueNo.Substring(0, 8);
                    No2 = IssueNo.Substring(0, 8);
                    zNo1 = NowIssueNo.Substring(8, 2);
                    zNo2 = IssueNo.Substring(8, 2);

                    int periodsCount = lotteryid == (int)Constant.LotteryType.Jx11x5 ? 42 : 41;
                    if (No1 == No2)
                    {
                        num = int.Parse(zNo2) - int.Parse(zNo1);
                        if (num > periodsCount || num < 0 || int.Parse(zNo2) > periodsCount)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //如果不是当天，则判断是否相差一天
                        num = int.Parse(No2) - int.Parse(No1);
                        if (num != 1)
                        {
                            return false;
                        }
                        num = periodsCount - int.Parse(zNo1) + int.Parse(zNo2);
                        if (num > periodsCount || num < 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    #endregion
            }
            return false;
        }

        /// <summary>
        /// 驗證追號期數是否合法
        /// </summary>
        /// <param name="maxPeriods">一天最大期數</param>
        /// <param name="currentIssueNo">當前期號</param>
        /// <param name="afterIssueNo">追号</param>
        /// <param name="diffMinute">差異分鐘數</param>
        /// <param name="issueFormat">期号格式</param>
        /// <returns>
        /// 是否合法
        /// </returns>
        static bool VaildMaxPeriodsWithDateTimeFormat(int maxPeriods, string currentIssueNo, string afterIssueNo, int diffMinute, string issueFormat)
        {
            bool isLegal = false;
            int yearLen = issueFormat.IndexOf("M");
            string currentYearStr = "";
            int shiftYearLen = 0;
            if (yearLen == 2)
            {
                string yearHead = DateTime.Now.Year.ToString().Substring(0, 2);
                currentYearStr = yearHead + currentIssueNo.Substring(0, 2); 
                shiftYearLen = 2;
            }
            else if (yearLen == 4)
            {
                currentYearStr = currentIssueNo.Substring(0, 4);                
            }

            string currentIssueStr =
                currentYearStr + "-" +
                currentIssueNo.Substring(4 - shiftYearLen, 2) + "-" +
                currentIssueNo.Substring(6 - shiftYearLen, 2) + " " +
                currentIssueNo.Substring(8 - shiftYearLen, 2) + ":" +
                currentIssueNo.Substring(10 - shiftYearLen, 2) +
                ":00";

            DateTime currentIssueTime, afterIssueTime;
            if (DateTime.TryParse(currentIssueStr, out currentIssueTime) && 
                DateTime.TryParse(currentIssueStr, out afterIssueTime))
            {
                long maxIssueNo = Convert.ToInt64(currentIssueTime.AddMinutes(diffMinute * maxPeriods).ToString(issueFormat));

                isLegal =  maxIssueNo > long.Parse(afterIssueNo);
            }
            
            return isLegal;
        }

        /// <summary>
        /// 驗證追號期數是否合法(连续号)
        /// </summary>
        /// <param name="maxPeriods">一天最大期數</param>
        /// <param name="currentIssueNo">最大期数</param>
        /// <param name="afterIssueNo">追号</param>
        /// <param name="issueFormat">期号格式</param>
        /// <param name="snoLen">流水号长度</param>
        /// <param name="startIssueNumber">第一期数字(取尾数一码)。例：001 = 1 or 000 = 0</param>
        /// <returns>
        /// 是否合法
        /// </returns>
        static bool VaildMaxPeriodsWithDateTimeFormatAndSNO(int maxPeriods,
            string currentIssueNo, string afterIssueNo, string issueFormat, int snoLen, string startIssueNumber)
        {
            bool isLegal = false;
            int yearLen = issueFormat.IndexOf("M");
            string currentYearStr = "";
            string afterYearStr = "";
            int shiftYearLen = 0;
            if (yearLen == 2)
            {
                string yearHead = DateTime.Now.Year.ToString().Substring(0, 2);
                currentYearStr = yearHead + currentIssueNo.Substring(0, 2);
                afterYearStr = yearHead + afterIssueNo.Substring(0, 2);
                shiftYearLen = 2;
            }
            else if (yearLen == 4)
            {
                currentYearStr = currentIssueNo.Substring(0, 4);
                afterYearStr = afterIssueNo.Substring(0, 4);
            }

            // 當前期號的日期
            int currentYear = 0;
            int currentMonth = 0;
            int currentDay = 0;
            int afterYear = 0;
            int afterMonth = 0;
            int afterDay = 0;
            int.TryParse(currentYearStr, out currentYear);
            int.TryParse(currentIssueNo.Substring(4 - shiftYearLen, 2), out currentMonth);
            int.TryParse(currentIssueNo.Substring(6 - shiftYearLen, 2), out currentDay);

            int.TryParse(afterYearStr, out afterYear);
            int.TryParse(afterIssueNo.Substring(4 - shiftYearLen, 2), out afterMonth);
            int.TryParse(afterIssueNo.Substring(6 - shiftYearLen, 2), out afterDay);

            DateTime currentIssueTime = new DateTime(currentYear, currentMonth, currentDay);


            DateTime afterIssueTime = new DateTime(afterYear, afterMonth, afterDay);

            long afterIssueNoByDay = Convert.ToInt64(afterIssueNo);

            if (DateTime.Compare(currentIssueTime, afterIssueTime) == 0)
            {
                long maxIssueNoByDay = Convert.ToInt64(currentIssueNo.Substring(0, currentIssueNo.Length - snoLen)  + maxPeriods.ToString().PadLeft(snoLen, '0'));
                
                isLegal = maxIssueNoByDay >= afterIssueNoByDay;
            }
            else if (new TimeSpan(afterIssueTime.Ticks - currentIssueTime.Ticks).Days == 1)
            {
                long modLength = (long)Math.Pow(10, snoLen);
                long currentPeriod = Convert.ToInt64(currentIssueNo) % modLength;

                string crossDayFormat = yearLen == 2 ? "yyMMdd" : "yyyyMMdd";

                long maxIssueNoCrossDay = Convert.ToInt64(afterIssueTime.ToString(crossDayFormat) + currentPeriod.ToString().PadLeft(snoLen, '0'));

                isLegal = maxIssueNoCrossDay > afterIssueNoByDay;
            }
           
            return isLegal;
        }
    }
}
