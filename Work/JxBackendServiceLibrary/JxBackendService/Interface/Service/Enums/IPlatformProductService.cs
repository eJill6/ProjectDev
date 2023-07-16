using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IPlatformProductService : IBaseValueModelService<string, PlatformProduct>
    {
        List<PlatformProduct> GetNonSelfProduct();
    }
}