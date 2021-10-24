using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IProfitLossTypeNameService : IBaseValueModelService<string, ProfitLossTypeName>
    {
        /// <summary>取得代理的盈虧項目</summary>
        List<ProfitLossTypeName> GetAllByAgent();

        /// <summary>取得贈送彩金項目</summary>
        List<ProfitLossTypeName> GetGivePrizeList();

        /// <summary>取得代理贈送彩金項目</summary>
        List<ProfitLossTypeName> GetGivePrizeByAgent();
    }
}
