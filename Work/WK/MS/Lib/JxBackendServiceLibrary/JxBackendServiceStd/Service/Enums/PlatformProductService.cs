using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Enums
{
    public class BasePlatformProductService : BaseValueModelService<string, PlatformProduct>, IPlatformProductService
    {
        private static readonly List<string> s_closePlatformProducts = new List<string>
        {
            PlatformProduct.AWCSP.Value,
        };

        protected override List<PlatformProduct> CreateAllList()
        {
            return base.CreateAllList().Where(w => !s_closePlatformProducts.Contains(w.Value)).ToList();
        }

        public List<PlatformProduct> GetNonSelfProduct()
        {
            return GetAll().Where(x => !x.IsSelfProduct && x.HasContract).ToList();
        }

        public List<JxBackendSelectListItem> GetContractSelectListItems(bool hasBlankOption, bool? isSupportHotGame)
        {
            List<PlatformProduct> platformProducts = GetAll()
                .Where(p => p.HasContract && !isSupportHotGame.HasValue || p.IsSupportHotGame == isSupportHotGame)
                .ToList();

            return GetSelectListItems(platformProducts, hasBlankOption);
        }
    }
}