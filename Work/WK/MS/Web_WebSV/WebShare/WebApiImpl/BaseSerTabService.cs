using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using SLPolyGame.Web.BLL;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model = SLPolyGame.Web.Model;
using System.Linq;

namespace WebApiImpl
{
    public abstract class BaseSerTabService : BaseWebApiService, ISerTabWebSVService
    {
        private readonly Lazy<PalyInfo> _palyInfo;

        public BaseSerTabService()
        {
            _palyInfo = DependencyUtil.ResolveService<PalyInfo>();
        }

        /// <summary> 获取彩种列表 </summary>
        public async Task<List<Model.LotteryInfo>> GetLotteryType()
        {
            try
            {
                LotteryInfo lo = new LotteryInfo();

                return await Task.FromResult(lo.GetLotteryType());
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetLotteryType异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        /// <summary> 获取各彩种选号方式列表 </summary>
        public async Task<List<Model.PlayTypeInfo>> GetPlayTypeInfo()
        {
            try
            {
                PlayTypeInfo pt = new PlayTypeInfo();

                return await Task.FromResult(pt.GetPlayTypeInfo());
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetPlayTypeInfo异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        /// <summary> 获取各彩种购买单选方式 </summary>
        public async Task<List<Model.PlayTypeRadio>> GetPlayTypeRadio()
        {
            try
            {
                PlayTypeRadio pr = new PlayTypeRadio();

                return await Task.FromResult(pr.GetPlayTypeRadio());
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetPlayTypeRadio异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        /// <summary> 获取彩票信息 </summary>
        public async Task<Model.CurrentLotteryInfo> GetLotteryInfos(int lotteryid)
        {
            try
            {
                CurrentLotteryInfo curr = new CurrentLotteryInfo();

                return await Task.FromResult(curr.GetLotteryInfos(lotteryid));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetLotteryInfos异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        public async Task<List<Model.LotteryInfo>> Get_AllWebLotteryType()
        {
            try
            {
                LotteryInfo lo = new LotteryInfo();

                return await Task.FromResult(lo.Get_AllWebLotteryType());
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"Get_AllWebLotteryType异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        public async Task<List<Model.CurrentLotteryInfo>> QueryCurrentLotteryInfo(Model.CurrentLotteryQueryInfo query)
        {
            try
            {
                CurrentLotteryInfo cl = new CurrentLotteryInfo();

                return await Task.FromResult(cl.QueryCurrentLotteryInfo(query));
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"获取走势图开奖结果时异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        /// <summary> 取得使用者今日所有投注單號 </summary>
        public async Task<Model.TodaySummaryInfo> GetTodaySummaryInfo(DateTime start, DateTime end, int lotteryID,
            int count)
        {
            try
            {
                Model.TodaySummaryInfo result = new Model.TodaySummaryInfo();
                int userId = EnvLoginUser.LoginUser.UserId;
                var orders = _palyInfo.Value.GetAllOrderList(userId, start, end);
                List<Model.CurrentLotteryInfo> lotteryResults = null;

                if ((Constant.LotteryType)lotteryID == Constant.LotteryType.HSSEC_PK10 ||
                    (Constant.LotteryType)lotteryID == Constant.LotteryType.HSSEC_MMC)
                {
                    lotteryResults = new CurrentLotteryInfo().GetCurrentLotteryInfo_Sec(lotteryID, count, userId);
                }
                else
                {
                    lotteryResults = new CurrentLotteryInfo().GetCurrentLotteryInfo(lotteryID, count);
                }

                result.Orders = orders;
                result.LotteryResults = lotteryResults;
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"GetTodaySummaryInfo异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        /// <summary> 网站获取投注概要信息 </summary>
        public async Task<Model.TodaySummaryInfo> GetPlayInfoSummaryInfo(int lotteryID, int count, bool isansy)
        {
            DateTime start = DateTime.Today;
            DateTime end = DateTime.Now;

            Model.TodaySummaryInfo result = new Model.TodaySummaryInfo();
            result.PlaySummaryInfoes = new List<Model.PlaySummaryModel>();
            int userId = EnvLoginUser.LoginUser.UserId;
            var palyInfo = _palyInfo.Value;

            List<Model.PalyInfo> orders = new List<Model.PalyInfo>();

            if (!isansy)
            {
                orders = palyInfo.GetAllOrderList(userId, start, end);
            }
            else
            {
                orders = palyInfo.GetAllOrderList1(userId, start, end);
            }

            List<Model.CurrentLotteryInfo> lotteryResults = new List<Model.CurrentLotteryInfo>();

            if ((Constant.LotteryType)lotteryID == Constant.LotteryType.HSSEC_PK10
                || (Constant.LotteryType)lotteryID == Constant.LotteryType.HSSEC_MMC)
            {
                if (!isansy)
                {
                    lotteryResults = new CurrentLotteryInfo().GetCurrentLotteryInfo_Sec(lotteryID, count, userId);
                }
                else
                {
                    lotteryResults = new CurrentLotteryInfo().GetCurrentLotteryInfo_Sec1(lotteryID, count, userId);
                }
            }
            else
            {
                if (!isansy)
                {
                    lotteryResults = new CurrentLotteryInfo().GetCurrentLotteryInfo(lotteryID, count);
                }
                else
                {
                    lotteryResults = new CurrentLotteryInfo().GetCurrentLotteryInfo1(lotteryID, count);
                }
            }

            var s1 = palyInfo.GetXDaysSummary(userId, DateTime.Now.Date, DateTime.Now, isansy);

            s1.Scope = "一天内输赢";

            result.PlaySummaryInfoes.Add(s1);

            var s2 = palyInfo.GetXDaysSummary(userId, DateTime.Now.AddDays(-7).Date, DateTime.Now, isansy);

            s2.Scope = "七天内输赢";

            result.PlaySummaryInfoes.Add(s2);

            result.Orders = orders;
            result.LotteryResults = lotteryResults;

            return await Task.FromResult(result);
        }

        /// <summary> 半屏游戏选单资讯 </summary>
        public async Task<IEnumerable<LiveGameManage>> GetLiveGameManageInfos()
        {
            try
            {
                var liveGameManageReadService =
                    DependencyUtil.ResolveJxBackendService<ILiveGameManageReadService>(EnvLoginUser,
                        DbConnectionTypes.Slave).Value;

                IEnumerable<LiveGameManage> liveGameManages = liveGameManageReadService.GetAll();
                
                return await Task.FromResult(liveGameManages);
            }
            catch (Exception ex)
            {
                LogUtilService.Error($"{nameof(GetLiveGameManageInfos)}异常:{ex.Message},ex:{ex}");

                return null;
            }
        }

        public async Task<Model.CurrentLotteryInfo[]> GetNextIssueNos(string lotteryIds)
        {
            var curr = new CurrentLotteryInfo();
            var nextIssueNos = curr.GetNextIssueNos(lotteryIds);

            return await Task.FromResult(nextIssueNos.ToArray());
        }
    }
}