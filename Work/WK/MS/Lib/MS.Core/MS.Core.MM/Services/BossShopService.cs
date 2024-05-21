using Microsoft.Extensions.Logging;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Entities.BossShop;
using MS.Core.MM.Repos.interfaces;
using MS.Core.MM.Service;
using MS.Core.MM.Services.interfaces;
using MS.Core.Models;
using MS.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Services
{
    public class BossShopService : BaseService, IBossShopService
    {
        private readonly IBossShopRepo _bossShopReop;
        public BossShopService(IBossShopRepo bossShopReop, ILogger<BannerService> logger) : base(logger)
        {
            _bossShopReop = bossShopReop;
        }
        public async Task<BaseReturnModel> Create(MMBossShop param)
        {
            return await TryCatchProcedure(async (param) =>
            {
                var repo = await _bossShopReop.Create(param);
                BaseReturnModel result = new BaseReturnModel();
                result.SetCode(ReturnCode.Success);
                return result;
            }, param);
        }
    }
}
