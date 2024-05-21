using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.Core.Extensions;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Infrastructures.Extensions;
using MS.Core.MM.Models.Booking.Enums;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace MMService.Controllers
{
    public class AdminIncomeController : ApiControllerBase
    {
        /// <summary>
        /// 狀態的檢查
        /// </summary>
        private static IncomeExpenseStatusEnum[] ExistIncomeExamineStatus => new IncomeExpenseStatusEnum[3]
        {
            IncomeExpenseStatusEnum.Approved,
            IncomeExpenseStatusEnum.Reject,
            IncomeExpenseStatusEnum.Unusual,
        };

        /// <summary>
        /// 解鎖相關服務
        /// </summary>
        private readonly IPostTransactionRepo _postTransactionRepo;

        /// <summary>
        /// 收益單相關Repo
        /// </summary>
        private readonly IIncomeExpenseRepo _incomeExpenseRepo;

        /// <summary>
        /// 會員卡相關Repo
        /// </summary>
        private readonly IVipTransactionRepo _vipTransactionRepo;

        /// <summary>
        /// 舉報相關Repo
        /// </summary>
        private readonly IPostRepo _postRepo;

        private readonly IBookingRepo _bookingRepo;

        /// <inheritdoc cref="IZeroOneApiService"/>
        private readonly IZeroOneApiService _zoService;

        private readonly IPostReportRepo _postReportRepo;

        /// <summary>
        /// 型別轉換
        /// </summary>
        private readonly IMapper _mapper;
        private readonly IVipService _vipService;
        public AdminIncomeController(IPostTransactionRepo postTransactionRepo,
            IIncomeExpenseRepo incomeExpenseRepo,
            IVipTransactionRepo vipTransactionRepo,
            IPostRepo postRepo,
            IBookingRepo bookingRepo,
            IPostReportRepo postReportRepo,
            IZeroOneApiService zoService,
            IMapper mapper,
            IVipService vipService,
            ILogger logger) : base(logger)
        {
            _postTransactionRepo = postTransactionRepo;
            _incomeExpenseRepo = incomeExpenseRepo;
            _vipTransactionRepo = vipTransactionRepo;
            _postRepo = postRepo;
            _bookingRepo = bookingRepo;
            _postReportRepo = postReportRepo;
            _zoService = zoService;
            _mapper = mapper;
            _vipService = vipService;
        }

        /// <summary>
        /// 收益单记录
        /// </summary>
        /// <param name="model">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<PageResultModel<AdminIncomeList>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(AdminIncomeListParam model)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<PageResultModel<AdminIncomeList>>();

                if (!string.IsNullOrWhiteSpace(param.PostId))
                {
                    var sourceIds = new string[] { };
                    var bookingFilter = new BookingFilter { PostId = param.PostId };
                    if (param.BookingPaymentType != 0) 
                    {
                        bookingFilter.PaymentType = (BookingPaymentType)param.BookingPaymentType;
                        MMBooking[] bookings = await _bookingRepo.GetUserBookingPost(bookingFilter);
                        sourceIds = bookings.Select(x => x.BookingId).ToArray();
                    }
                    else
                    {
                        MMPostTransactionModel[] unlocks = await _postTransactionRepo.List(new string[1] { param.PostId }, 1);
                        MMBooking[] bookings = await _bookingRepo.GetUserBookingPost(bookingFilter);
                        sourceIds = unlocks.Select(x => x.Id).Union(bookings.Select(x => x.BookingId)).ToArray();
                    }                        
                    
                    if (sourceIds.Length > 0)
                    {
                        //var reports = await _postReportRepo.QueryByFilter(new PostReportFilter
                        //{
                        //    PostIds = new List<string> { param.PostId }
                        //});
                        result.DataModel = _mapper.Map<PageResultModel<AdminIncomeList>>(await _incomeExpenseRepo.List(new AdminIncomeExpenseListByTypeParam()
                        {
                            Ids = sourceIds,
                            Type = IncomeExpenseTransactionTypeEnum.Income,
                            IdType = 0,
                        }));
                        foreach (var item in result.DataModel.Data)
                        {
                            item.PostId = param.PostId;
                            var unlock = await _postTransactionRepo.List(new string[1] { item.SourceId }, 0).FirstOrDefaultAsync();
                            if (!string.IsNullOrEmpty(unlock?.ReportId))
                            {
                                var report = (await _postRepo.GetReports(new string[1] { unlock.ReportId }));
                                item.ReportId = report.Any() ? report.FirstOrDefault().ReportId : "";
                                item.ReportStatus = report.Any() ? report.FirstOrDefault().Status : 0;
                            }
                            if ((item.Category == IncomeExpenseCategoryEnum.Square || item.Category == IncomeExpenseCategoryEnum.Agency) && item.ReportStatus == 1)
                                item.UnusualMemo = "被投诉(通过)";
                        }
                        result.SetCode(ReturnCode.Success);
                    }
                    else
                    {
                        result.DataModel = new PageResultModel<AdminIncomeList>()
                        {
                            Data = new AdminIncomeList[0]
                        };
                        result.SetCode(ReturnCode.Success);
                    }
                }
                else
                {
                    result.DataModel = _mapper.Map<PageResultModel<AdminIncomeList>>(await _incomeExpenseRepo.List(model));
                    if (result.DataModel.Data.Length > 0)
                    {
                        var sourceIds = result.DataModel.Data.Select(x => x.SourceId).ToArray();
                        //var unlocks = await _postTransactionRepo.List(sourceIds, 0);
                        //var bookingDic = await _bookingRepo.List(sourceIds);
                        //var unlockDic = unlocks.ToDictionary(x => x.Id, x => x.PostId);
                        //var postIdDic = unlockDic.Concat(bookingDic).ToDictionary(x => x.Key, x => x.Value);
                        foreach (var item in result.DataModel.Data)
                        {
                            var unlocks =  await _postTransactionRepo.List(new string[1] { item.SourceId }, 0);
                            //var bookingDic = await _bookingRepo.List(sourceIds);
                            //item.PostId = unlock.PostId;
                            if (unlocks.Any() && !string.IsNullOrEmpty(unlocks.FirstOrDefault().ReportId))
                            {
                                var report = (await _postRepo.GetReports(new string[1] { unlocks.FirstOrDefault().ReportId }));
                                item.ReportId = report.FirstOrDefault().ReportId;
                                item.ReportStatus = report.FirstOrDefault().Status;
                            }

                            if (unlocks.Any())
                                item.PostId = unlocks.FirstOrDefault().PostId;
                            else
                            {
                                var bookingDic = await _bookingRepo.List(sourceIds);
                                var unlockDic = unlocks.ToDictionary(x => x.Id, x => x.PostId);
                                var postIdDic = unlockDic.Concat(bookingDic).ToDictionary(x => x.Key, x => x.Value);
                                item.PostId = postIdDic[item.SourceId];
                            }

                            if ((item.Category == IncomeExpenseCategoryEnum.Square || item.Category == IncomeExpenseCategoryEnum.Agency) && item.ReportStatus == 1)
                                item.UnusualMemo = "被投诉(通过)";

                            //if (postIdDic.ContainsKey(item.SourceId))
                            //{
                            //    item.PostId = postIdDic[item.SourceId];


                            //    //var reports = await _postReportRepo.QueryByFilter(new PostReportFilter
                            //    //{
                            //    //    PostIds = new List<string> { item.PostId }
                            //    //});
                            //    //item.ReportId = reports.Any() ? reports.FirstOrDefault().ReportId : "";
                            //    //item.ReportStatus = reports.Any() ? reports.FirstOrDefault().Status : 0;
                            //    //item.ReportId = !string.IsNullOrEmpty(unlocks.FirstOrDefault().ReportId) ? unlocks.FirstOrDefault().ReportId : "";
                            //    //item.ReportStatus = unlocks.FirstOrDefault().s
                            //}
                            //else
                            //{
                            //    _logger.LogError($"{MethodInfo.GetCurrentMethod()?.Name ?? string.Empty} {nameof(MMIncomeExpenseModel)} or {nameof(MMBooking)} not have {nameof(MMPostTransactionModel)} item:{JsonConvert.SerializeObject(item)}");
                            //}
                        }
                        result.SetCode(ReturnCode.Success);
                    }
                }
                return result;
            }, model));
        }

        /// <summary>
        /// 收益單审核
        /// </summary>
        /// <param name="commentId">收益單Id</param>
        /// <param name="model">审核資訊</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{incomeId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(string incomeId, AdminIncomeData model)
        {
            model.Id = incomeId;
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var income = (await _incomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter()
                {
                    Ids = new string[1] { param.Id },
                    TransactionType = IncomeExpenseTransactionTypeEnum.Income
                })).FirstOrDefault();

                if (income != null)
                {
                    // 非正常的修改狀態
                    // 未成為異常單
                    // 不是未派發的狀態
                    if (!ExistIncomeExamineStatus.Contains(param.Status)
                     || income.Status != IncomeExpenseStatusEnum.Unusual)
                    {
                        return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                    }

                    var nickname = User?.Nickname();
                    if (string.IsNullOrEmpty(nickname))
                    {
                        return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                    }
                    param.ExamineMan = nickname;
                    if (param.Status == IncomeExpenseStatusEnum.Approved)
                    {
                        // 要說明備註
                        if (string.IsNullOrEmpty(param.Memo))
                        {
                            return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                        }
                        income.Status = IncomeExpenseStatusEnum.Approved;
                        var now = DateTimeExtension.GetCurrentTime();
                        income.UpdateTime = now;
						income.DistributeTime = now;
						var dbResult = await _incomeExpenseRepo.Dispatched(income);
                        if (dbResult.IsSuccess)
                        {
							var result = await _zoService.CashIncome(new ZOCashIncomeExpenseReq(
							ZOIncomeExpenseCategory.UnlockEarnings,
							income.UserId,
							income.Amount * income.Rebate,
							income.Id
						    ));
							return new BaseReturnModel(result);
						}
                        else
                        {
                            return new BaseReturnModel(dbResult.GetReturnCode());
                        }
                    }
                    else if (param.Status == IncomeExpenseStatusEnum.Reject)
                    {
                        // 要說明備註
                        if (string.IsNullOrEmpty(param.Memo))
                        {
                            return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
                        }
                        // 取出支出方，把鑽石還給支出的使用者
                        var expense = (await _incomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter()
                        {
                            TargetIds = new string[1] { param.Id },
                            TransactionType = IncomeExpenseTransactionTypeEnum.Expense
                        })).First();
                        if (expense != null)
                        {
							var dbResult = await _incomeExpenseRepo.Update(new AdminIncomeExpenseUpdateStatusParam()
							{
								Id = incomeId,
								Memo = model.Memo,
								Status = param.Status
							});
                            if (dbResult.IsSuccess)
                            {
								var refundResult = await _zoService.PointIncome(new ZOPointIncomeExpenseReq(
								ZOIncomeExpenseCategory.UnlockRefund,
								expense.UserId,
								expense.Amount * expense.Rebate,
								expense.Id));
								return new BaseReturnModel(refundResult);
                            }
                            else
                            {
                                return new BaseReturnModel(dbResult.GetReturnCode());
                            }
                        }
                    }
                    else
                    {
                        var dbResult = await _incomeExpenseRepo.Update(new AdminIncomeExpenseUpdateStatusParam()
                        {
                            Id = incomeId,
                            Memo = model.Memo,
                            Status = param.Status
                        });
                        return new BaseReturnModel(dbResult.GetReturnCode());
                    }
                }
                return new BaseReturnModel(ReturnCode.DataIsNotExist);
            }, model));
        }

        /// <summary>
        /// 入账审核详情
        /// </summary>
        /// <param name="incomeId">收益单Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{incomeId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminIncomeDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Detail(string incomeId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminIncomeDetail>();
                var income = (await _incomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter()
                {
                    Ids = new string[1] { param },
                    TransactionType = IncomeExpenseTransactionTypeEnum.Income
                })).FirstOrDefault();
                if (income != null)
                {
                    var expense = (await _incomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter()
                    {
                        TargetIds = new string[1] { param },
                        TransactionType = IncomeExpenseTransactionTypeEnum.Expense
                    })).First();

                    var unlock = (await _postTransactionRepo.List(new string[1] { income.SourceId }, 0)).First();
                    result.DataModel = _mapper.Map<AdminIncomeDetail>(income);
                    result.DataModel.UnlockUserId = expense.UserId;
                    result.DataModel.PostId = unlock.PostId;
                    result.DataModel.ReportId = unlock.ReportId;
                    if (string.IsNullOrEmpty(result.DataModel.UnusualMemo) &&
                        result.DataModel.Status != IncomeExpenseStatusEnum.Unusual)
                    {
                        var user = await _vipService.GetUserInfoData(result.DataModel.UserId);
                        if (!user.IsVip)
                        {
                            result.DataModel.UnusualMemo += "会员卡到期";
                        }
                        if (!string.IsNullOrEmpty(unlock.ReportId))
                        {
                            var report = (await _postRepo.GetReports(new string[1] { unlock.ReportId })).First();
                            if (!string.IsNullOrEmpty(result.DataModel.UnusualMemo))
                            {
                                result.DataModel.UnusualMemo += "，";
                            }
                            var status = report.Status;
                            result.DataModel.ReportStatus = status;
                            switch ((ReviewStatus)status)
                            {
                                case ReviewStatus.Approval:
                                    result.DataModel.UnusualMemo += "被投诉通过";
                                    break;
                                case ReviewStatus.UnderReview:
                                    result.DataModel.UnusualMemo += "被投诉审核中";
                                    break;
                                case ReviewStatus.NotApproved:
                                    result.DataModel.UnusualMemo += "被投诉未通过";
                                    break;
                            }
                        }
                    }
                    result.SetCode(ReturnCode.Success);
                }
                return result;
            }, incomeId));
        }

        /// <summary>
        /// 预约单入账审核详情
        /// </summary>
        /// <param name="incomeId">收益单Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{incomeId}")]
        [ProducesResponseType(typeof(ApiResponse<AdminIncomeBookingDetail>), StatusCodes.Status200OK)]
        public async Task<IActionResult> BookingDetail(string incomeId)
        {
            return ApiResult(await TryCatchProcedure(async (param) =>
            {
                var result = new BaseReturnDataModel<AdminIncomeBookingDetail>();
                var income = (await _incomeExpenseRepo.GetTransactionByFilter(new IncomeExpenseFilter()
                {
                    Ids = new string[1] { param },
                    TransactionType = IncomeExpenseTransactionTypeEnum.Income
                })).FirstOrDefault();
                if (income != null)
                {
                    var booking = await _bookingRepo.GetById(income.SourceId);
                    var report = (await _postRepo.GetUserReported(booking.UserId, booking.PostId, null)).FirstOrDefault();
                    result.DataModel = new AdminIncomeBookingDetail
                    {
                        Id = income.Id,
                        BookingId = booking.BookingId,
                        BookingStatus = (int)booking.Status,
                        Status = income.Status,
                        BookingTime = booking.BookingTime,
                        BookingUserId = booking.UserId,
                        Category = income.Category,
                        ComboPrice = booking.ComboPrice,
                        PaymentMoney = booking.PaymentMoney,
                        UserId = booking.PostUserId,
                        DistributeTime = income.DistributeTime,
                        PaymentType = (int)booking.PaymentType,
                        PostId = booking.PostId,
                        ReportId = report?.ReportId,
                        UnusualMemo = income.UnusualMemo
                    };
                    if (string.IsNullOrEmpty(result.DataModel.UnusualMemo) &&
                        result.DataModel.Status != IncomeExpenseStatusEnum.Unusual)
                    {
                        var user = await _vipService.GetUserInfoData(result.DataModel.UserId);
                        if (!user.IsVip)
                        {
                            result.DataModel.UnusualMemo += "会员卡到期";
                        }
                        if (report != null)
                        {
                            if (!string.IsNullOrEmpty(result.DataModel.UnusualMemo))
                            {
                                result.DataModel.UnusualMemo += "，";
                            }
                            var status = report.Status;
                            switch ((ReviewStatus)status)
                            {
                                case ReviewStatus.Approval:
                                    result.DataModel.UnusualMemo += "被投诉通过";
                                    break;
                                case ReviewStatus.UnderReview:
                                    result.DataModel.UnusualMemo += "被投诉审核中";
                                    break;
                                case ReviewStatus.NotApproved:
                                    result.DataModel.UnusualMemo += "被投诉未通过";
                                    break;
                            }
                        }
                    }
                    result.SetCode(ReturnCode.Success);
                }
                return result;
            }, incomeId));
        }
    }
}
