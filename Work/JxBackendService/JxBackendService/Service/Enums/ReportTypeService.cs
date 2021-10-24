using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Service.Enums
{
    public abstract class BaseReportTypeService : IReportTypeService
    {
        private readonly JxApplication _jxApplication;
        private List<JxBackendSelectListItem> _menus;
        private readonly ConcurrentDictionary<int, List<JxBackendSelectListItem>> _productSelectListItemsByReportType = new ConcurrentDictionary<int, List<JxBackendSelectListItem>>();
        private readonly ConcurrentDictionary<int, List<PlatformProduct>> _productsByReportType = new ConcurrentDictionary<int, List<PlatformProduct>>();

        public BaseReportTypeService(JxApplication jxApplication)
        {
            _jxApplication = jxApplication;
        }

        protected abstract List<JxBackendSelectListItem> CreateMenuData();

        public List<JxBackendSelectListItem> GetMenus()
        {
            _menus = AssignValueOnceUtil.GetAssignValueOnce(_menus, CreateMenuData);
            return _menus;
        }

        public List<PlatformProduct> GetProductsByReportType(int reportTypeValue)
        {
            ProfitLossReportTabTypes reportType = ProfitLossReportTabTypes.GetSingle(reportTypeValue);

            if (reportType == null)
            {
                return new List<PlatformProduct>();
            }

            _productsByReportType.TryGetValue(reportType.Value, out List<PlatformProduct> list);

            if (list != null)
            {
                return list;
            }

            var platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(_jxApplication, SharedAppSettings.PlatformMerchant);
            list = platformProductService.GetAll()
                .Where(w => reportType.RefProductTypes.Contains(w.ProductType))
                .ToList();

            _productsByReportType.TryAdd(reportType.Value, list);

            return list;
        }

        public List<JxBackendSelectListItem> GetProductSelectItemsByReportType(int reportTypeValue)
        {
            ProfitLossReportTabTypes reportType = ProfitLossReportTabTypes.GetSingle(reportTypeValue);

            if (reportType == null)
            {
                return new List<JxBackendSelectListItem>();
            }

            _productSelectListItemsByReportType.TryGetValue(reportType.Value, out List<JxBackendSelectListItem> list);

            if (list != null)
            {
                return list;
            }

            List<PlatformProduct> products = GetProductsByReportType(reportTypeValue);
            list = BaseStringValueModel<PlatformProduct>.GetSelectListItems(products, false);
            _productSelectListItemsByReportType.TryAdd(reportType.Value, list);

            return list;
        }

        public ReportInnerSetting GetReportInnerSetting(int reportTypeValue)
        {
            ProfitLossReportTabTypes reportType = ProfitLossReportTabTypes.GetSingle(reportTypeValue);

            if (reportType == null)
            {
                return null;
            }

            var reportInnerSettingService = DependencyUtil.ResolveKeyedForModel<IReportInnerSettingService>(reportType);
            return reportInnerSettingService.GetInnerSetting();
        }

        protected IEnumerable<ProfitLossReportTabTypes> GetFrnotSideAndMobileApiMenus()
        {
            return ProfitLossReportTabTypes.GetAll()
                .Where(w => w != ProfitLossReportTabTypes.PlatformLottery && w != ProfitLossReportTabTypes.OtherLottery)
                .OrderBy(o => o.Sort);
        }
    }

    public class FrontSideWebReportTypeService : BaseReportTypeService
    {
        public FrontSideWebReportTypeService(JxApplication jxApplication) : base(jxApplication)
        {
        }

        protected override List<JxBackendSelectListItem> CreateMenuData()
        {
            return GetFrnotSideAndMobileApiMenus().Select(s => new JxBackendSelectListItem()
            {
                Value = s.Value.ToString(),
                Text = s.WebMenuName
            }).ToList();
        }
    }

    public class MobileApiReportTypeService : BaseReportTypeService
    {
        public MobileApiReportTypeService(JxApplication jxApplication) : base(jxApplication)
        {
        }

        protected override List<JxBackendSelectListItem> CreateMenuData()
        {
            return GetFrnotSideAndMobileApiMenus().Select(s => new JxBackendSelectListItem()
            {
                Value = s.Value.ToString(),
                Text = s.AppTabName
            }).ToList();
        }
    }

    public class BackSideReportTypeService : BaseReportTypeService
    {
        public BackSideReportTypeService(JxApplication jxApplication) : base(jxApplication)
        {
        }

        protected override List<JxBackendSelectListItem> CreateMenuData()
        {
            return ProfitLossReportTabTypes.GetAll()
                .Where(w => w != ProfitLossReportTabTypes.Lottery)
                .OrderBy(s => s.Sort)
                .Select(s => new JxBackendSelectListItem()
                {
                    Value = ((int)s.RefProductTypes.Single()).ToString(),
                    Text = s.BackSidePopupMenuName
                }).ToList();
        }
    }
}
