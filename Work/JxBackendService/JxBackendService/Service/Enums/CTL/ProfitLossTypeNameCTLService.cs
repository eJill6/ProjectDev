using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Enums.CTL
{
    public class ProfitLossTypeNameCTLService : BaseValueModelService<string, ProfitLossTypeName>, IProfitLossTypeNameService
    {
        private static readonly ConcurrentDictionary<WalletType, List<ProfitLossTypeName>> _walletPrizeLists =
            new ConcurrentDictionary<WalletType, List<ProfitLossTypeName>>();

        protected override List<ProfitLossTypeName> CreateAllList()
        {
            string[] profitLossTypeNames =
            {
                ProfitLossTypeName.CZ.Value,
                ProfitLossTypeName.TX.Value,
                ProfitLossTypeName.FD.Value,
                ProfitLossTypeName.KY.Value,
                ProfitLossTypeName.HB.Value,
                ProfitLossTypeName.ZZ.Value,
                ProfitLossTypeName.YJ.Value,
                ProfitLossTypeName.Commission.Value,
                ProfitLossTypeName.Prizes.Value
            };

            return base.CreateAllList().Where(w => profitLossTypeNames.Contains(w.Value)).ToList();
        }

        public List<ProfitLossTypeName> GetAllByAgent()
        {
            throw new NotSupportedException();
        }

        public List<ProfitLossTypeName> GetGivePrizeByAgent()
        {
            throw new NotSupportedException();
        }

        public List<ProfitLossTypeName> GetGivePrizeList()
        {
            if (_walletPrizeLists.ContainsKey(WalletType.Center))
            {
                return _walletPrizeLists[WalletType.Center];
            }

            List<ProfitLossTypeName> list = CreateAllList()
                .Where(w => w == ProfitLossTypeName.HB || 
                w == ProfitLossTypeName.YJ ||
                w == ProfitLossTypeName.Prizes ||
                w == ProfitLossTypeName.CZ).ToList();

            _walletPrizeLists[WalletType.Center] = list;

            return list;
        }
    }
}
