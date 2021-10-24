using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Model.Common;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public static class GlobalVariables
    {
        public static readonly string SystemOperator = CommonElement.SystemOperator;

        public static readonly int MaxSearchRowCount = 100000;

        /// <summary>用於區分LotteryInfo這張Table，第三方的資料起始的LotteryId</summary>
        public static readonly int ThirdPartyInLotteryInfoStartIdIndex = 888000;

        /// <summary>首頁顯示推薦彩種的數量</summary>
        public static readonly int HomePageLotteryRecommendCount = 8;

        /// <summary>群聊建立聊天室最大數量</summary>
        public static readonly int ManageChatRoomMaxCount = 20;

        public static readonly int ChatRoomNameMaxLength = 16;

        public static readonly int ChatRoomMessageContentMaxLength = 500;

        public static readonly int MaxPasswordAttempt = 6;

        /// <summary>
        /// 用於每次重新啟動IIS時，組合的靜態資源路徑後面的參數 v={StaticContentVersion} ，避免用戶瀏覽器Cache
        /// </summary>
        public static readonly string StaticContentVersion = Guid.NewGuid().ToString();

        private static readonly Lazy<IMerchantSettingService> _merchantSettingService =
            new Lazy<IMerchantSettingService>(() => DependencyUtil.ResolveKeyed<IMerchantSettingService>(SharedAppSettings.PlatformMerchant));

        /// <summary>品牌代碼</summary>
        public static string BrandCode => _merchantSettingService.Value.BrandCode;

        /// <summary>品牌名稱</summary>
        public static string BrandName => _merchantSettingService.Value.BrandName;

        /// <summary>最上層的用戶id</summary>
        public static readonly int RootUserID = 1;

        /// <summary>查詢前台彩票/第三方盈虧團隊報表限制天數</summary>
        public static readonly int QueryProfitLossAndPlayHistoryLimitDays = 35;

        /// <summary>查詢前台彩票/第三方盈虧個人明細限制天數</summary>
        public static readonly int QueryProfitLossAndPlayHistoryDetailLimitDays = 7;

        /// <summary>圖形驗證碼數字位數</summary>
        public static readonly int ValidateCodeImageNumberCount = 4;

        /// <summary>圖形驗證碼數字位數</summary>
        public static readonly int ValidateCodeImageWidth = ValidateCodeImageNumberCount * 120;

        /// <summary>圖形驗證碼數字位數</summary>
        public static readonly int ValidateCodeImageHeight = ValidateCodeImageNumberCount * 40;

        /// <summary>最低轉帳限額</summary>
        public static readonly int MinTransferAmountLimit = 1;

        /// <summary>最高轉帳限額</summary>
        public static readonly int MaxTransferAmountLimit = 40000;

        /// <summary>轉帳推薦金額</summary>
        public static readonly List<string> RecommendTransferAmountList = new List<string>() { "100", "200", "500", "1000" };

        /// <summary>靜態資源路徑代號</summary>
        public static string MerchantFolderCode = "{MerchantFolder}";

        /// <summary>後台可輸入的排序大小最多可输入3个数字</summary>
        public static readonly int MaxSortSerialLimit = 999;
    }
}