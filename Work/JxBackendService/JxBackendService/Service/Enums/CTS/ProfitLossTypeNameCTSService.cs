using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Enums.CTL
{
    public class ProfitLossTypeNameCTSService : BaseValueModelService<string, ProfitLossTypeName>, IProfitLossTypeNameService
    {
        private static readonly ConcurrentDictionary<WalletType, List<ProfitLossTypeName>> _walletLists =
            new ConcurrentDictionary<WalletType, List<ProfitLossTypeName>>();

        private static readonly ConcurrentDictionary<WalletType, List<ProfitLossTypeName>> _walletPrizeLists =
            new ConcurrentDictionary<WalletType, List<ProfitLossTypeName>>();

        protected override List<ProfitLossTypeName> CreateAllList()
        {
            string[] profitLossTypeNames =
            {
                ProfitLossTypeName.CZ.Value,
                ProfitLossTypeName.TX.Value,
                ProfitLossTypeName.KY.Value,
                ProfitLossTypeName.HB.Value,
                ProfitLossTypeName.ZZ.Value,
                ProfitLossTypeName.YJ.Value,
                //ProfitLossTypeName.Commission.Value,
                ProfitLossTypeName.Prizes.Value,
                ProfitLossTypeName.Activity.Value,
                ProfitLossTypeName.Adjustments.Value,
            };

            return base.CreateAllList().Where(w => profitLossTypeNames.Contains(w.Value)).ToList();
        }

        public List<ProfitLossTypeName> GetAllByAgent()
        {
            if (_walletLists.ContainsKey(WalletType.Agent))
            {
                return _walletLists[WalletType.Agent];
            }

            var list = new List<ProfitLossTypeName>()
            {
                ProfitLossTypeName.TX,
                ProfitLossTypeName.HB,
                ProfitLossTypeName.ZZ,
                ProfitLossTypeName.YJ,
                ProfitLossTypeName.Activity,
                ProfitLossTypeName.Adjustments,
            };

            _walletLists[WalletType.Agent] = list;

            return list;
        }

        public List<ProfitLossTypeName> GetGivePrizeList()
        {
            if (_walletPrizeLists.ContainsKey(WalletType.Center))
            {
                return _walletPrizeLists[WalletType.Center];
            }

            List<ProfitLossTypeName> list = CreateAllList()
                .Where(w => w == ProfitLossTypeName.CZ ||
                w == ProfitLossTypeName.Prizes ||
                w == ProfitLossTypeName.HB ||
                w == ProfitLossTypeName.Activity ||
                w == ProfitLossTypeName.Adjustments).ToList();

            _walletPrizeLists[WalletType.Center] = list;

            return list;
        }
        public List<ProfitLossTypeName> GetGivePrizeByAgent()
        {
            if (_walletPrizeLists.ContainsKey(WalletType.Agent))
            {
                return _walletPrizeLists[WalletType.Agent];
            }

            List<ProfitLossTypeName> list = GetAllByAgent()
                .Where(w => w == ProfitLossTypeName.YJ ||
                w == ProfitLossTypeName.HB ||
                w == ProfitLossTypeName.Adjustments).ToList();

            _walletPrizeLists[WalletType.Agent] = list;

            return list;
        }
    }
}
