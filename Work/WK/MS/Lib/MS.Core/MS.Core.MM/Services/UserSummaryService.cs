using Microsoft.Extensions.Logging;
using MS.Core.Extensions;
using MS.Core.Infrastructure.Redis;
using MS.Core.Infrastructures.Providers;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MM.Models.User;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MM.Services.interfaces;
using MS.Core.Models;

namespace MS.Core.MM.Service
{
    public class UserSummaryService : BaseHttpRequestService, IUserSummaryService
    {
        public UserSummaryService(IRequestIdentifierProvider provider, ILogger logger, 
            IUserSummaryRepo userQuantityRepo, 
            IRedisService redisService, 
            IDateTimeProvider dateTimeProvider
            ) : base(provider, logger, dateTimeProvider)
        {
            UserSummaryRepo = userQuantityRepo;
            RedisService = redisService;
        }
        private IRedisService RedisService { get; }
        private IUserSummaryRepo UserSummaryRepo { get; }

        public async Task<BaseReturnDataModel<UserSummaryModel[]>> GetUserSummaris(IEnumerable<int> userIds)
        {
            return await TryCatchProcedure(async (userIds) =>
            {
                var result = await UserSummaryRepo.GetUserSummaries(userIds).SelectAsync(e => new UserSummaryModel
                {
                    Amount = e.Amount,
                    Type = e.Type,
                    UserId = e.UserId,
                    Category = e.Category
                }).ToArrayAsync();

                return new BaseReturnDataModel<UserSummaryModel[]> (ReturnCode.Success)
                {
                    DataModel = result
                };
            }, userIds);
        }

        public async Task<BaseReturnDataModel<UserSummaryModel[]>> GetUserSummaris(int userId)
        {
            return await GetUserSummaris(userId.ToEnumerable());
        }

        public async Task<BaseReturnDataModel<decimal>> GetUserAmount(int userId, UserSummaryTypeEnum type, UserSummaryCategoryEnum category)
        {
            return await TryCatchProcedure(async (userIds) =>
            {
                var result = await UserSummaryRepo.GetUserAmount(userId, type, category);

                return new BaseReturnDataModel<decimal>(ReturnCode.Success)
                {
                    DataModel = result
                };
            }, new { userId, type, category });
            
        }

        public async Task<BaseReturnModel> RestSetUserUnLock()
        {
            return await TryCatchProcedure(async () =>
            {
                var now = DateTimeProvider.Now;

                var next = now.AddDays(1).Date;

                var ts = next - now;

                DateTime res = await RedisService.GetOrSetAsync(RedisCacheKey.RestSetUserUnLock, ts, async () =>
                {
                    await UserSummaryRepo.RestSetAmount(UserSummaryTypeEnum.FreeUnlock, 0M, now);
                    return now;
                });

                return new BaseReturnModel(ReturnCode.Success);
            });
        }
    }
}