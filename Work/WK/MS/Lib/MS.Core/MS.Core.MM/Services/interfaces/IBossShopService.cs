using MS.Core.MM.Model.Media;
using MS.Core.MM.Models.Entities.BossShop;
using MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Services.interfaces
{
    public interface  IBossShopService
    {
        Task<BaseReturnModel> Create(MMBossShop param);
    }
}
