using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Models.Entities.Post;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using Newtonsoft.Json;

namespace MS.Core.MM.Service
{
    public class IncomeExpenseService : MMBaseService, IIncomeExpenseService
    {
        public IncomeExpenseService(Microsoft.Extensions.Logging.ILogger logger,
            IIncomeExpenseRepo incomeExpenseRepo,
            IVipTransactionRepo vipTransactionRepo,
            IUserInfoRepo userInfoRepo,
            IUserVipRepo userVipRepo,
            IPostReportRepo postReportRepo,
            IZeroOneApiService zeroOneApiService,
            IVipService vipService
            ) : base(logger)
        {
            IncomeExpenseRepo = incomeExpenseRepo;
            VipTransactionRepo = vipTransactionRepo;
            UserInfoRepo = userInfoRepo;
            UserVipRepo = userVipRepo;
            PostReportRepo = postReportRepo;
            ZeroOneApiService = zeroOneApiService;
            VipService = vipService;
        }

        private IIncomeExpenseRepo IncomeExpenseRepo { get; }
        private IVipTransactionRepo VipTransactionRepo { get; }
        private IUserInfoRepo UserInfoRepo { get; }
        private IUserVipRepo UserVipRepo { get; }
        private IPostReportRepo PostReportRepo { get; }
        private IZeroOneApiService ZeroOneApiService { get; }
        private IVipService VipService { get; }

        /// <summary>
        /// 派發
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnModel> DistributeAmount(DateTime time)
        {
            return await base.TryCatchProcedure(async () =>
            {
                //更新 Amount 為 0 的狀態
                await IncomeExpenseRepo.UpdateZeroAmountStatus();

                PageResultModel<MMIncomeExpenseModel> pageResult =
                    await IncomeExpenseRepo.GetPageTransactionByFilter(new PageIncomeExpenseFilter
                    {
                        EndTime = time.AddHours(-120),
                        Status = IncomeExpenseStatusEnum.UnDispatched,
                        TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Income },
                        Categories = new IncomeExpenseCategoryEnum[2] {
                            IncomeExpenseCategoryEnum.Square,
                            IncomeExpenseCategoryEnum.Agency
                        },
                        PayType = IncomeExpensePayType.Amount,
                        IsZeroAmount = false,
                        Pagination = new PaginationModel
                        {
                            PageNo = 1,
                            PageSize = 50,
                        }
                    });

                MMIncomeExpenseModel[] incomes = pageResult.Data;
                if (incomes.IsEmpty())
                {
                    return new BaseReturnModel(ReturnCode.Success) { };
                }

                //舉報單
                var reports = (await PostReportRepo.QueryByFilter(new PostReportFilter
                {
                    PostTranIds = pageResult.Data.Select(e => e.SourceId).ToArray()
                })).GroupBy(x => x.PostTranId).ToDictionary(e => e.Key, e => e.ToArray());

                var incomeUserIds = incomes.Select(e => e.UserId).Distinct();
                HashSet<int> userIds = new HashSet<int>();
                foreach (var incomeUserId in incomeUserIds)
                {
                    if (await VipService.IsVip(incomeUserId))
                        userIds.Add(incomeUserId);
                }
                //有身份的会员ID
                HashSet<int> identityUserIds = await UserInfoRepo.GetUserInfos(incomeUserIds).WhereAsync(a => a.UserIdentity > 0).SelectAsync(a => a.UserId).ToHashSetAsync();

                foreach (MMIncomeExpenseModel income in incomes)
                {
                    MMPostReport[] report = reports.GetValueOrDefault(income.SourceId) ?? new MMPostReport[0];
                    var r1 = report.FirstOrDefault();
                    var r2 = report.LastOrDefault();
                    if (report.Length < 2)
                    {
                        r2 = null;
                    }
                    new CheckIncomeStatus(income)
                        .CheckIsIdentityAndVip(userIds, identityUserIds)
                        .CheckIsReported((ReviewStatus?)r1?.Status)
                        .CheckIsReported((ReviewStatus?)r2?.Status)
                        .UpdateResult();
                }

                //更新狀態(異常)
                await IncomeExpenseRepo.UpdateStatus(incomes.Where(e => e.Status == IncomeExpenseStatusEnum.Unusual));

                //未派發
                var unusualIncomes = incomes.Where(e => e.Status == IncomeExpenseStatusEnum.UnDispatched).ToArray();

                foreach (var income in unusualIncomes)
                {
                    decimal amount = income.Amount * income.Rebate;
                    ZOCashIncomeExpenseReq zoIncome = new(ZOIncomeExpenseCategory.UnlockEarnings, income.UserId, amount, income.Id);

                    await ZeroOneApiService.CashIncome(zoIncome);

                    //已派發
                    income.Status = IncomeExpenseStatusEnum.Dispatched;
                    income.DistributeTime = DateTimeExtension.GetCurrentTime();
                    await IncomeExpenseRepo.Dispatched(income);
                }
                return new BaseReturnModel(ReturnCode.Success);
            });
        }

        /// <summary>
        /// 检查无身份无会员卡异常收益单
        /// </summary>
        /// <returns></returns>
        public async Task<BaseReturnModel> AuditAbnormalOrder()
        {
            return await base.TryCatchProcedure(async () =>
            {
                PageResultModel<MMIncomeExpenseModel> pageResult = default;
                int pageNo = 1;
                do
                {
                    pageResult = await IncomeExpenseRepo.GetPageTransactionByFilter(new PageIncomeExpenseFilter
                    {
                        Status = IncomeExpenseStatusEnum.Unusual,
                        TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Income },
                        Pagination = new PaginationModel
                        {
                            PageNo = pageNo,
                            PageSize = 500
                        }
                    });
                    if (pageResult.Data.IsEmpty())
                        break;

                    //舉報單
                    var reports = (await PostReportRepo.QueryByFilter(new PostReportFilter
                    {
                        PostTranIds = pageResult.Data.Select(e => e.SourceId).ToArray()
                    })).GroupBy(x => x.PostTranId).ToDictionary(e => e.Key, e => e.ToArray());

                    var incomeUserIds = pageResult.Data.Select(e => e.UserId).Distinct();
                    HashSet<int> userIds = new HashSet<int>();
                    foreach (var incomeUserId in incomeUserIds)
                    {
                        if (await VipService.IsVip(incomeUserId))
                            userIds.Add(incomeUserId);
                    }

                    //有身份的会员ID
                    HashSet<int> identityUserIds = await UserInfoRepo.GetUserInfos(incomeUserIds).WhereAsync(a => a.UserIdentity > 0).SelectAsync(a => a.UserId).ToHashSetAsync();

                    foreach (MMIncomeExpenseModel income in pageResult.Data)
                    {
                        MMPostReport[] report = reports.GetValueOrDefault(income.SourceId) ?? new MMPostReport[0];
                        var r1 = report.FirstOrDefault();
                        var r2 = report.LastOrDefault();
                        if (report.Length < 2)
                        {
                            r2 = null;
                        }
                        new CheckIncomeStatus(income)
                            .CheckIsIdentityAndVip(userIds, identityUserIds)
                            .CheckIsReported((ReviewStatus?)r1?.Status)
                            .CheckIsReported((ReviewStatus?)r2?.Status)
                            .UpdateResult(CheckIncomeStatus.UpateResultType.AuditAbnormalOrder);
                    }

                    //更新狀態(派发)
                    var result = await IncomeExpenseRepo.Dispatched(pageResult.Data);
                    if (result.IsSuccess)
                    {
                        foreach (var income in pageResult.Data.Where(e => e.Status == IncomeExpenseStatusEnum.Dispatched))
                        {
                            var refundResult = await ZeroOneApiService.CashIncome(new ZOCashIncomeExpenseReq(
                                ZOIncomeExpenseCategory.UnlockEarnings,
                                income.UserId,
                                income.Amount * income.Rebate,
                                income.Id
                            ));
                            if (!refundResult.IsSuccess)
                            {
                                _logger.LogError("收益单Id:{Id}，{AuditAbnormalOrder}/{CashIncome}请求失败，返回结果：{RefundResult}", income.Id, nameof(AuditAbnormalOrder), nameof(ZeroOneApiService.CashIncome), JsonConvert.SerializeObject(refundResult));
                                continue;
                            }
                        }
                    }
                    pageNo++;
                } while (!pageResult.Data.IsEmpty());
                return new BaseReturnModel(ReturnCode.Success);
            });
        }
    }

    internal class CheckIncomeStatus
    {
        public CheckIncomeStatus(MMIncomeExpenseModel income)
        {
            income.UnusualMemo = string.Empty;
            Income = income;
        }

        private MMIncomeExpenseModel Income { get; }

        /// <summary>
        /// 派發
        /// </summary>
        private bool IsDistribute => IsVipIdentity && !IsReported;

        /// <summary>
        /// Vip
        /// </summary>
        private bool IsVip { get; set; } = false;

        /// <summary>
        /// 是否有身份
        /// </summary>
        private bool IsIdentity { get; set; } = false;

        /// <summary>
        /// 同时有VIP和身份
        /// </summary>
        private bool IsVipIdentity { get; set; } = false;

        /// <summary>
        /// 被舉報(已通過、未審核 都視為被舉報)
        /// </summary>
        private bool IsReported { get; set; } = false;

        /// <summary>
        /// 異常備註
        /// </summary>
        private List<string> UnusualMemos { get; set; } = new List<string>();

        /// <summary>
        /// 檢查是否有會員
        /// </summary>
        /// <param name="userIds">有會員的 UserIds</param>
        /// <returns></returns>
        public CheckIncomeStatus CheckIsVip(HashSet<int> userIds)
        {
            IsVip = userIds.Contains(this.Income.UserId);

            if (IsVip == false)
            {
                UnusualMemos.Add("无会员卡");
            }
            return this;
        }

        /// <summary>
        /// 檢查是否有身份和会员卡
        /// </summary>
        /// <param name="vipUserIds">有會員的 UserIds</param>
        /// <param name="identityUserIds">有身份的 UserIds</param>
        /// <returns></returns>
        public CheckIncomeStatus CheckIsIdentityAndVip(HashSet<int> vipUserIds, HashSet<int> identityUserIds)
        {
            IsVip = vipUserIds.Contains(this.Income.UserId);
            IsIdentity = identityUserIds.Contains(this.Income.UserId);
            if (IsIdentity == false && IsVip == false)
            {
                UnusualMemos.Add("无身份且无会员卡");
            }
            else
            {
                IsVipIdentity = true;
            }

            return this;
        }

        /// <summary>
        /// 檢查被舉報
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckIncomeStatus CheckIsReported(ReviewStatus? status)
        {
            //沒有舉報單 不改狀態
            if (status.HasValue == false)
            {
                return this;
            }

            switch (status)
            {
                case ReviewStatus.UnderReview:
                    UnusualMemos.Add("被投诉(审核中)");
                    IsReported = IsReported || true;
                    break;

                case ReviewStatus.Approval:
                    UnusualMemos.Add("被投诉(通过)");
                    IsReported = IsReported || true;
                    break;

                case ReviewStatus.NotApproved:
                    UnusualMemos.Add("被投诉(未通过)");
                    IsReported = IsReported || false;
                    break;

                default:
                    throw new NotImplementedException();
            }
            return this;
        }

        public void UpdateResult(UpateResultType type = UpateResultType.Distribute)
        {
            switch (type)
            {
                case UpateResultType.Distribute:
                    if (IsDistribute == false)
                    {
                        this.Income.Status = IncomeExpenseStatusEnum.Unusual;
                    }

                    this.Income.UnusualMemo = string.Join("，", UnusualMemos);

                    this.Income.UpdateTime = DateTimeExtension.GetCurrentTime();
                    break;
                case UpateResultType.AuditAbnormalOrder:
                    if (IsDistribute)
                    {
                        this.Income.Status = IncomeExpenseStatusEnum.Dispatched;

                        this.Income.UnusualMemo = "-";

                        this.Income.UpdateTime = DateTimeExtension.GetCurrentTime();

                        this.Income.DistributeTime = DateTimeExtension.GetCurrentTime();
                    }
                    break;
                default:
                    break;
            }

        }

        public enum UpateResultType
        {
            Distribute,

            AuditAbnormalOrder
        }
    }
}
