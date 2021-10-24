using JxBackendService.Common.Util;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JxBackendService.Service.Enums
{
    public class BasePlatformProductService : BaseValueModelService<string, PlatformProduct>, IPlatformProductService
    {
        /// <summary>
        /// 取得非自營產品列表
        /// </summary>
        public List<PlatformProduct> GetThirdPartyList()
        {
            return GetSortedPlatformProduct(GetAll().Where(x => !x.IsSelfProduct).ToList());
        }

        public List<JxBackendSelectListItem> GetAllPlatformProductSelectListItems(bool hasBlankOption)
        {
            return GetSortedPlatformProductSelectListItems(GetAll(), hasBlankOption);
        }

        public List<JxBackendSelectListItem> GetProductSelectList(ProductTypes productType, bool hasBlankOption)
        {
            return GetProductSelectList(new List<ProductTypes> { productType }, hasBlankOption);
        }

        public List<JxBackendSelectListItem> GetProductSelectList(List<ProductTypes> productType, bool hasBlankOption)
        {
            List<PlatformProduct> platformProducts = GetAll().Where(w => productType.Contains(w.ProductType)).ToList();
            return GetSelectListItems(platformProducts, hasBlankOption, string.Empty, SelectItemElement.PlzChoice);
        }

        public List<ProfitLossTypeName> GetProfitLossTypeNames(PlatformProduct product)
        {
            if (product.IsSelfProduct)
            {
                return ProfitLossTypeName.GetAll();
            }
            else
            {
                var otherProfitLossTypes = ProfitLossTypeName.GetAll().Where(x => x != ProfitLossTypeName.YJ && x != ProfitLossTypeName.HB && x != ProfitLossTypeName.ZZ).ToList();

                if (product.ProductType == ProductTypes.OtherLottery)
                {
                    otherProfitLossTypes = otherProfitLossTypes.Where(x => x != ProfitLossTypeName.FD).ToList();
                }

                return otherProfitLossTypes;
            }
        }

        public List<PlatformProduct> GetProductWithCondition(List<string> excludeList)
        {
            return GetAll()
                .Where(x => !excludeList.Contains(x.Value)).OrderBy(o => o.Sort)
                .ToList();
        }

        public List<PlatformProduct> GetProductWithoutPT()
        {
            return GetProductWithCondition(new List<string> { PlatformProduct.PT.Value }).ToList();
        }

        public List<PlatformProduct> GetNonSelfProductWithoutPT()
        {
            return GetProductWithCondition(new List<string> { PlatformProduct.PT.Value }).Where(x => !x.IsSelfProduct).ToList();
        }

        public List<JxBackendSelectListItem> GetBackendThirdAccountProductSelectListItems(bool hasBlankOption)
        {
            return GetSortedPlatformProductSelectListItems(GetNonSelfProductWithoutPT(), hasBlankOption);
        }

        public List<PlatformProduct> GetSortedPlatformProduct()
        {
            return GetSortedPlatformProduct(GetAll());
        }

        public List<PlatformProduct> GetSortedPlatformProduct(List<PlatformProduct> platformProducts)
        {
            return platformProducts.OrderBy(o => (int)o.ProductType).ThenBy(o => o.Sort).ToList();
        }

        private List<JxBackendSelectListItem> GetSortedPlatformProductSelectListItems(List<PlatformProduct> platformProducts, bool hasBlankOption)
        {
            return GetSelectListItems(GetSortedPlatformProduct(platformProducts), hasBlankOption, string.Empty, SelectItemElement.PlzChoice);
        }
    }

    //原本MobileApi會有不同步上線的問題
    //public class MobileApiPlatformProductService : BasePlatformProductService
    //{
    //    protected override List<PlatformProduct> CreateAllList()
    //    {
    //        return base.CreateAllList();//.Where(w => w.Value != PlatformProduct.IMVR).ToList();
    //    }
    //}

    public class PlatformProductCTSService : BasePlatformProductService
    {
        protected override List<PlatformProduct> CreateAllList()
        {
            return base.CreateAllList().Where(w => w.Value != PlatformProduct.Lottery).ToList();
        }
    }
}
