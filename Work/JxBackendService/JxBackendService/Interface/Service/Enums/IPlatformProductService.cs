using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service.Enums
{
    public interface IPlatformProductService : IBaseValueModelService<string, PlatformProduct>
    {
        /// <summary>
        /// 取得非自營產品列表
        /// </summary>
        List<PlatformProduct> GetThirdPartyList();

        List<JxBackendSelectListItem> GetAllPlatformProductSelectListItems(bool hasBlankOption);

        List<JxBackendSelectListItem> GetProductSelectList(ProductTypes productType, bool hasBlankOption);

        List<JxBackendSelectListItem> GetProductSelectList(List<ProductTypes> productType, bool hasBlankOption);

        List<ProfitLossTypeName> GetProfitLossTypeNames(PlatformProduct product);

        List<PlatformProduct> GetProductWithCondition(List<string> excludeList);

        List<PlatformProduct> GetProductWithoutPT();

        List<PlatformProduct> GetNonSelfProductWithoutPT();

        List<JxBackendSelectListItem> GetBackendThirdAccountProductSelectListItems(bool hasBlankOption);

        List<PlatformProduct> GetSortedPlatformProduct();

        List<PlatformProduct> GetSortedPlatformProduct(List<PlatformProduct> platformProducts);

    }
}
