using Microsoft.Extensions.Logging;
using MS.Core.Infrastructures.Providers;
using MS.Core.Infrastructures.ZoneOne;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Services.Bases;
using MS.Core.MM.Services.interfaces;

namespace MS.Core.MM.Services
{
    public class BaseTransactionService : BaseHttpRequestService
    {
        protected IZeroOneApiService ZeroOneApiService { get; }
        protected IVipTransactionRepo VipTransactionRepo { get; }
        protected IVipWelfareService VIPWelfareService { get; }
        protected IUserInfoRepo UserInfoRepo { get; }
        public BaseTransactionService(
            IRequestIdentifierProvider provider, 
            ILogger logger,
            IVipTransactionRepo vipTransactionRepo,
            IVipWelfareService vipWelfareServic,
            IUserInfoRepo userInfoRepo,
            IZeroOneApiService zeroOneApiService,
            IDateTimeProvider dateTimeProvider) : base(provider, logger, dateTimeProvider)
        {
            ZeroOneApiService = zeroOneApiService;
            VipTransactionRepo = vipTransactionRepo;
            VIPWelfareService = vipWelfareServic;
            UserInfoRepo = userInfoRepo;
        }
    }
}
