using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructure.Redis;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZeroOne.Models.Requests;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Infrastructures.Exceptions;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.Filters;
using MS.Core.MM.Models.User;
using MS.Core.MM.Models.Vip;
using MS.Core.MM.Repos;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Service;
using MS.Core.MM.Services.interfaces;
using MS.Core.MMModel.Models.IncomeExpense;
using MS.Core.MMModel.Models.Vip.Enums;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Diagnostics.CodeAnalysis;

namespace MS.Core.MM.Services
{
    public class VipService : BaseTransactionService, IVipService
    {
        public VipService(
            IRequestIdentifierProvider provider,
            ILogger logger,
            IVipRepo vipRepo,
            IVipTransactionRepo vipTransactionRepo,
            IVipWelfareService vipWelfareServic,
            IUserInfoRepo userInfoRepo,
            IIncomeExpenseRepo incomeExpenseRepo,
            IDateTimeProvider dateTimeProvider,
            IUserSummaryService userSummaryService,
            IMemoryCache memoryCache,
            IVipTypeRepo vipTypeRepo,
                IPostRepo postRepo,
            IZeroOneApiService zeroOneApiService) : base(provider, logger, vipTransactionRepo, vipWelfareServic, userInfoRepo, zeroOneApiService, dateTimeProvider)
        {
            VipRepo = vipRepo;
            IncomeExpenseRepo = incomeExpenseRepo;
            UserSummaryService = userSummaryService;
            MemoryCache = memoryCache;
            VipTypeRepo = vipTypeRepo;
            _postRepo = postRepo;
        }

        private IUserSummaryService UserSummaryService { get; }
        private IMemoryCache MemoryCache { get; }
        private IIncomeExpenseRepo IncomeExpenseRepo { get; }
        private IVipRepo VipRepo { get; }
        private IVipTypeRepo VipTypeRepo { get; }

        /// <summary>
        /// 贴子相關
        /// </summary>
        private readonly IPostRepo _postRepo;

        /// <inheritdoc/>
        public async Task<UserInfoData> GetUserInfoData(int userId)
        {
            if (await UserInfoRepo.GetUserInfo(userId) is not MMUserInfo user)
            {
                throw new MMException(ReturnCode.NotMember, "資料異常");
            }

            return await GetUserInfoData(user);
        }

        public async Task<UserSummaryInfoData> GetUserSummaryInfoData(int userId)
        {
            int postCount = await _postRepo.GetPostCountByUserId(userId);
            int officialPostCount = await _postRepo.GetOfficialPostCountByUserId(userId);
            //总发帖次数
            int postTotalCount = postCount + officialPostCount;

            UserInfoData userInfoData = await GetUserInfoData(userId);
            UserSummaryModel[] userSummaries = await UserSummaryService.GetUserSummaris(userId).GetReturnDataAsync();

            return new UserSummaryInfoData(userInfoData, userSummaries, postTotalCount);
        }

        /// <inheritdoc/>
        public async Task<MMVipType[]> GetVipTypes()
        {
            string key = MemoryCacheKey.VipTypes;

            var vipTypes = MemoryCache.Get<MMVipType[]>(key);

            if (vipTypes == null)
            {
                vipTypes = await VipTypeRepo.GetAll();
                MemoryCache.Set(key, vipTypes, TimeSpan.FromHours(1));
            }

            return vipTypes;
        }

        public async Task<bool> IsVip(int userId)
        {
            return (await ZeroOneApiService.GetPermission(new ZOVipPermissionReq
            {
                Permission = VipPermission.MmGround,
                UserId = userId
            })).DataModel;
        }

        /// <inheritdoc/>
        public async Task<UserInfoData> GetUserInfoData([NotNull] MMUserInfo user)
        {
            MMVipType[] vipTpyes = await GetVipTypes();

            MMVipWelfare[] welfares = Array.Empty<MMVipWelfare>();
            UserVipInfo[] userVips = Array.Empty<UserVipInfo>();
            if (await IsVip(user.UserId))
            {
                userVips = userVips.Append(new UserVipInfo
                {
                    UserId = user.UserId,
                    VipType = VipType.Gold,
                    Priority = (int)VipType.Gold,
                    TypeName = VipType.Gold.ToString()
                }).ToArray();
                welfares = await VIPWelfareService.GetVipWelfares(userVips.OrderByDescending(x => x.Priority).First().VipType).ToArrayAsync();
                return new UserInfoData(user, vipTpyes, userVips, welfares);
            }
            return new UserInfoData(user, vipTpyes);
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<ResVip[]>> GetListedVips()
        {
            return await base.TryCatchProcedure(async () =>
            {
                var result = await VipRepo.GetListedVipsAsync();
                return new BaseReturnDataModel<ResVip[]>(ReturnCode.Success)
                {
                    DataModel = result.Select(e => new ResVip
                    {
                        Id = e.Id,
                        Memo = e.Memo,
                        Name = e.Name,
                        Price = e.Price,
                        Type = e.Type,
                        Days = e.Days,
                    }).ToArray(),
                };
            });
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<ResVip[]>> GetVips()
        {
            return await base.TryCatchProcedure(async () =>
            {
                var result = await VipRepo.GetVipsAsync();
                return new BaseReturnDataModel<ResVip[]>(ReturnCode.Success)
                {
                    DataModel = result.Select(e => new ResVip
                    {
                        Id = e.Id,
                        Memo = e.Memo,
                        Name = e.Name,
                        Price = e.Price,
                        Type = e.Type,
                        Days = e.Days,
                        Status = e.Status
                    }).ToArray(),
                };
            });
        }

        /// <inheritdoc/>
        public async Task<BaseReturnDataModel<ResUserVipTransLog[]>> GetUserVipTransLogs(ReqUserVipTransLog vipLog)
        {
            return await base.TryCatchProcedure(async () =>
            {
                IncomeExpenseFilter filter = new()
                {
                    UserId = vipLog.UserId,
                    Categories = IncomeExpenseCategoryEnum.Vip.ToEnumerable(),
                    TransactionTypes = new List<IncomeExpenseTransactionTypeEnum> { IncomeExpenseTransactionTypeEnum.Expense }
                };

                IEnumerable<MMIncomeExpenseModel> userTrans = await IncomeExpenseRepo.GetTransactionByFilterOrderByDescending(filter);

                return new BaseReturnDataModel<ResUserVipTransLog[]>(ReturnCode.Success)
                {
                    DataModel = userTrans.Select(e => new ResUserVipTransLog
                    {
                        OrderID = e.Id,
                        PayType = e.PayType,
                        Status = e.Status,
                        Title = e.Title,
                        TransactionTime = e.CreateTime.ToString(GlobalSettings.DateTimeFormat),
                        Amount = e.Amount.ToString(GlobalSettings.AmountFormat),
                    }).ToArray(),
                };
            });
        }
    }
}