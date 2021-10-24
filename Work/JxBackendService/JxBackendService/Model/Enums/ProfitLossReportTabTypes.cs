using JxBackendService.Resource.Element;
using JxBackendService.Service.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class ProfitLossReportTabTypes : BaseIntValueModel<ProfitLossReportTabTypes>
    {
        private ProfitLossReportTabTypes() { }

        public ProductTypes[] RefProductTypes { get; private set; }

        /// <summary>
        /// App Tab名稱
        /// </summary>
        public string AppTabName => GetNameByResourceInfo(ResourceType, AppTabResourcePropertyName);

        /// <summary>
        /// Web 左邊Menu名稱
        /// </summary>
        public string WebMenuName => GetNameByResourceInfo(ResourceType, WebMenuPrefixResourcePropertyName) + SelectItemElement.Report;

        /// <summary>
        /// 後台popup選單名稱
        /// </summary>
        public string BackSidePopupMenuName => GetNameByResourceInfo(ResourceType, BackSideTabResourcePropertyName) + SelectItemElement.ProfitLossTypeName_KY;

        /// <summary>
        /// InnerMenu-盈虧名稱
        /// </summary>
        public string ProfitLossInnerMenuName => GetNameByResourceInfo(ResourceType, WebMenuPrefixResourcePropertyName) + SelectItemElement.ProfitLoss;

        /// <summary>
        /// InnerMenu-購買紀錄名稱
        /// </summary>
        public string BuyHistoryInnerMenuName => GetNameByResourceInfo(ResourceType, WebMenuPrefixResourcePropertyName) + SelectItemElement.InnerMenu_BuyHistory;

        public string WebMenuPrefixResourcePropertyName { get; private set; }

        public string AppTabResourcePropertyName { get; private set; }

        public string BackSideTabResourcePropertyName { get; private set; }

        public Type ReportInnerSettingServiceType { get; private set; } = typeof(BaseReportInnerSettingService);

        /// <summary>彩票</summary>
        public static readonly ProfitLossReportTabTypes Lottery = new ProfitLossReportTabTypes()
        {
            Value = 1,
            RefProductTypes = new[] { ProductTypes.Lottery, ProductTypes.OtherLottery },
            ResourceType = typeof(SelectItemElement),
            WebMenuPrefixResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_MenuPrefix_Lottery),
            AppTabResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_Tab_Lottery),
            ReportInnerSettingServiceType = typeof(LotteryReportInnerSettingService),
            Sort = 1,
        };

        /// <summary>體育</summary>
        public static readonly ProfitLossReportTabTypes Sport = new ProfitLossReportTabTypes()
        {
            Value = -1,
            RefProductTypes = new[] { ProductTypes.Sports },
            ResourceType = typeof(SelectItemElement),
            WebMenuPrefixResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_MenuPrefix_Sport),
            AppTabResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_Tab_Sport),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_Sports),
            Sort = 3,
        };

        /// <summary>真人</summary> 
        public static readonly ProfitLossReportTabTypes Live = new ProfitLossReportTabTypes()
        {
            Value = 14,
            RefProductTypes = new[] { ProductTypes.LiveCasino },
            ResourceType = typeof(SelectItemElement),
            WebMenuPrefixResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_MenuPrefix_Live),
            AppTabResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_Tab_Live),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_LiveCasino),
            Sort = 2,
        };

        /// <summary>電子遊戲</summary>       
        public static readonly ProfitLossReportTabTypes SlotGame = new ProfitLossReportTabTypes()
        {
            Value = 11,
            RefProductTypes = new[] { ProductTypes.Slots },
            ResourceType = typeof(SelectItemElement),
            WebMenuPrefixResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_MenuPrefix_SlotGame),
            AppTabResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_Tab_SlotGame),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_Slots),
            Sort = 4,
        };

        /// <summary>棋牌</summary>          
        public static readonly ProfitLossReportTabTypes BoardGame = new ProfitLossReportTabTypes()
        {
            Value = 16,
            RefProductTypes = new[] { ProductTypes.BoardGame },
            ResourceType = typeof(SelectItemElement),
            WebMenuPrefixResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_MenuPrefix_BoardGame),
            AppTabResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_Tab_BoardGame),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_BoardGame),
            Sort = 5,
        };

        /// <summary>電競</summary>      
        public static readonly ProfitLossReportTabTypes ESport = new ProfitLossReportTabTypes()
        {
            Value = 9,
            RefProductTypes = new[] { ProductTypes.ESports },
            ResourceType = typeof(SelectItemElement),
            WebMenuPrefixResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_MenuPrefix_ESport),            
            AppTabResourcePropertyName = nameof(SelectItemElement.ProfitLossReportTabType_Tab_ESport),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_ESports),
            Sort = 6,
        };

        /// <summary>AMD彩票</summary>
        public static readonly ProfitLossReportTabTypes PlatformLottery = new ProfitLossReportTabTypes()
        {
            Value = -2,
            RefProductTypes = new[] { ProductTypes.Lottery },
            ResourceType = typeof(SelectItemElement),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_Lottery),
            Sort = -2,
        };

        /// <summary>其他彩票</summary>
        public static readonly ProfitLossReportTabTypes OtherLottery = new ProfitLossReportTabTypes()
        {
            Value = -3,
            RefProductTypes = new[] { ProductTypes.OtherLottery },
            ResourceType = typeof(SelectItemElement),
            BackSideTabResourcePropertyName = nameof(SelectItemElement.ProductType_OtherLottery),            
            Sort = -1,
        };
    }
}
