using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Enums;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.Enums
{
    public class BasePlatformProductService : BaseValueModelService<string, PlatformProduct>, IPlatformProductService
    {
        private static readonly List<string> s_closePlatformProducts = new List<string>
        {
        };

        protected override List<PlatformProduct> CreateAllList()
        {
            return base.CreateAllList().Where(w => !s_closePlatformProducts.Contains(w.Value)).ToList();
        }

        public List<PlatformProduct> GetNonSelfProduct()
        {
            return GetAll().Where(x => !x.IsSelfProduct && x.HasContract).ToList();
        }
    }
}