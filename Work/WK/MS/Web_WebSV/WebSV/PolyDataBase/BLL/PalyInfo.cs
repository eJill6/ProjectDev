using Microsoft.Extensions.Logging;
using SLPolyGame.Web.Common;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SLPolyGame.Web.BLL
{
    /// <summary>
    /// PalyInfo
    /// </summary>
    public class PalyInfo
    {
        private readonly DAL.PalyInfo _dal = null;
        private readonly ILogger<PalyInfo> _logger = null;

        public PalyInfo(DAL.PalyInfo dal, ILogger<PalyInfo> logger)
        {
            _dal = dal;
            _logger = logger;
        }

        #region Method

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Model.PalyInfo> DataTableToList(DataTable dt)
        {
            List<Model.PalyInfo> modelList = new List<Model.PalyInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Model.PalyInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Model.PalyInfo();
                    if (dt.Rows[n]["PalyID"].ToString() != "")
                    {
                        model.PalyID = dt.Rows[n]["PalyID"].ToString();
                    }
                    model.PalyCurrentNum = dt.Rows[n]["PalyCurrentNum"].ToString();
                    model.PalyNum = dt.Rows[n]["PalyNum"].ToString();
                    if (dt.Rows[n]["PlayTypeID"].ToString() != "")
                    {
                        model.PlayTypeID = int.Parse(dt.Rows[n]["PlayTypeID"].ToString());
                    }
                    if (dt.Rows[n]["LotteryID"].ToString() != "")
                    {
                        model.LotteryID = int.Parse(dt.Rows[n]["LotteryID"].ToString());
                    }
                    model.UserName = dt.Rows[n]["UserName"].ToString();
                    if (dt.Rows[n]["NoteNum"].ToString() != "")
                    {
                        model.NoteNum = int.Parse(dt.Rows[n]["NoteNum"].ToString());
                    }
                    if (dt.Rows[n]["SingleMoney"].ToString() != "")
                    {
                        model.SingleMoney = decimal.Parse(dt.Rows[n]["SingleMoney"].ToString());
                    }
                    if (dt.Rows[n]["NoteMoney"].ToString() != "")
                    {
                        model.NoteMoney = decimal.Parse(dt.Rows[n]["NoteMoney"].ToString());
                    }
                    if (dt.Rows[n]["NoteTime"].ToString() != "")
                    {
                        model.NoteTime = DateTime.Parse(dt.Rows[n]["NoteTime"].ToString());
                    }
                    if (dt.Rows[n]["IsWin"].ToString() != "")
                    {
                        if ((dt.Rows[n]["IsWin"].ToString() == "1") || (dt.Rows[n]["IsWin"].ToString().ToLower() == "true"))
                        {
                            model.IsWin = true;
                        }
                        else
                        {
                            model.IsWin = false;
                        }
                    }
                    if (dt.Rows[n]["WinMoney"].ToString() != "")
                    {
                        model.WinMoney = decimal.Parse(dt.Rows[n]["WinMoney"].ToString());
                    }
                    if (dt.Rows[n]["IsFactionAward"].ToString() != "")
                    {
                        model.IsFactionAward = int.Parse(dt.Rows[n]["IsFactionAward"].ToString());
                    }
                    if (dt.Rows[n]["PlayTypeRadioID"].ToString() != "")
                    {
                        model.PlayTypeRadioID = int.Parse(dt.Rows[n]["PlayTypeRadioID"].ToString());
                    }
                    if (dt.Rows[n]["RebatePro"].ToString() != "")
                    {
                        model.RebatePro = decimal.Parse(dt.Rows[n]["RebatePro"].ToString());
                    }
                    model.RebateProMoney = dt.Rows[n]["RebateProMoney"].ToString();
                    if (dt.Rows[n]["WinNum"].ToString() != "")
                    {
                        model.WinNum = int.Parse(dt.Rows[n]["WinNum"].ToString());
                    }
                    if (dt.Rows[n]["UserID"].ToString() != "")
                    {
                        model.UserID = int.Parse(dt.Rows[n]["UserID"].ToString());
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #endregion  Method

        public Model.PalyInfo InsertPlayInfo(Model.PalyInfo model)
        {
            Model.PalyInfo paly = new Model.PalyInfo();
            var singleMoney = model.SingleMoney;
            var userId = model.UserID;
            int userType = _dal.GetUserType(Convert.ToInt32(model.PlayTypeID));
            if (userType == 1)
            {
                if (string.IsNullOrEmpty(model.SourceType))
                {
                    model.SourceType = "oldweb";
                }
                paly = _dal.InsertPlayInfo_v1(model);
            }
            else if (userType == 2)
            {
                model.SourceType = "web";
                paly = _dal.InsertPlayInfo(model);
            }
            else
            {
                paly.UserName = "参数错误，0M8";
            }

            if (paly.UserName.Contains("参数错误") || paly.UserName == "帐户被冻结" || paly.UserName == "非法")
            {
                _logger.LogError("下单时,用户名：{0},用户ID：{1},错误提示：{2},序列化数据：{3}"
                    , model.UserName, model.UserID, paly.UserName, ModelToJson.ObjectToJson(model, typeof(Model.PalyInfo)));
            }
            return paly;
        }

        /// <summary>
        /// 撤单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string CancelOrder(Model.PalyInfo model)
        {
            return _dal.CancelOrder(model);
        }

        /// <summary>
        /// 获取当天下单数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Model.PalyInfo> GetBetRecord(int userId)
        {
            return _dal.GetBetRecord(userId);
        }

        /// <summary>
        /// 游戏记录分页总数
        /// </summary>
        public Model.PalyInfo GetUserPalyBetCount(int lotteryId, string PalyCurrentNum, string iswin, DateTime starttime, DateTime endtime, int userId)
        {
            return _dal.GetUserPalyBetCount(lotteryId, PalyCurrentNum, iswin, starttime, endtime, userId);
        }

        /// <summary>
        /// 指定用户投注记录
        /// </summary>
        public List<Model.PalyInfo> GetUserPalyBet(int pageSize, int pageNum, int lotteryId, string PalyCurrentNum, string iswin, DateTime starttime, DateTime endtime, int userId)
        {
            return _dal.GetUserPalyBet(pageSize, pageNum, lotteryId, PalyCurrentNum, iswin, starttime, endtime, userId);
        }

        /// <summary>
        /// 获取单个订单的信息
        /// </summary>
        /// <param name="playId"></param>
        /// <returns></returns>
        public Model.PalyInfo GetPalyIDPalyBet(int playId, int userId)
        {
            return _dal.GetPalyIDPalyBet(playId, userId);
        }

        /// <summary>
        /// web端获取当天下单数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Model.PalyInfo> GetAllOrderList(int userId)
        {
            return _dal.GetAllOrderList(userId);
        }

        public PlaySummaryModel GetXDaysSummary(int userId, DateTime start, DateTime end, bool isAnsy)
        {
            return _dal.GetXDaysSummary(userId, start, end, isAnsy);
        }

        public List<Model.PalyInfo> GetAllOrderList(int userId, DateTime start, DateTime end)
        {
            return _dal.GetAllOrderList_FromMainDb(userId, start, end);
        }

        public List<Model.PalyInfo> GetAllOrderList1(int userId, DateTime start, DateTime end)
        {
            return _dal.GetAllOrderList_FromBakDb(userId, start, end);
        }

        public List<string> GetLatestWinningList(string period)
        {
            var items = _dal.GetLatestWinningList(period);

            return items.Select(x => string.Format("【{0}】在{1}中喜中<span class=\"red\">{2}元</span>", x.UserName, x.LotteryName, x.AmountText)).ToList();
        }

        public List<WinningListItem> GetLatestWinningListItems(string period)
        {
            return _dal.GetLatestWinningList(period);
        }

        public async Task<CursorPaginationTotalData<Model.PalyInfo>> GetSpecifyOrderList(int userId, int? lotteryId, string status, DateTime searchDate, string cursor, int pageSize, string roomId)
        {
            return await _dal.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize, roomId);
        }

        /// <summary>
        ///秘色跟单资讯
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CursorPagination<Model.PalyInfo> GetFollowBet(string palyId, int lottertId)
        {
            return _dal.GetFollowBet(palyId, lottertId);
        }

        public Model.PalyInfo[] GetPlayBetsByAnonymous(string startTime, string endTime, string gameId)
        {
            return _dal.GetPlayBetsByAnonymous(startTime, endTime, gameId);
        }

        public Model.PalyInfo GetPlayBetByAnonymous(string playId)
        {
            return _dal.GetPlayBetByAnonymous(playId);
        }
    }
}

