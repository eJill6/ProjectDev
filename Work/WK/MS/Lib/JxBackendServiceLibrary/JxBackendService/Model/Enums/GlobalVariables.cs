using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using System;

namespace JxBackendService.Model.Enums
{
    public static class GlobalVariables
    {
        public static readonly int MaxSearchRowCount = 100000;

        public static readonly string SystemOperator = CommonElement.SystemOperator;

        private static readonly Lazy<string> s_staticContentVersion = new Lazy<string>(
            () =>
            {
                string staticContentVersion = SharedAppSettings.StaticContentVersion;

                if (staticContentVersion.IsNullOrEmpty())
                {
                    staticContentVersion = Guid.NewGuid().ToString();
                }

                return staticContentVersion;
            });

        /// <summary>
        /// 用於每次重新啟動IIS時，組合的靜態資源路徑後面的參數 v={StaticContentVersion} ，避免用戶瀏覽器Cache
        /// </summary>
        public static string StaticContentVersion => s_staticContentVersion.Value;

        /// <summary>後台可輸入的排序大小最多可输入3个数字</summary>
        public static readonly int MaxSortSerialLimit = 999;

        public static readonly int SendVerifyCodeIntervalSeconds = 90;

        /// <summary>HA架構下的資料庫同步的等待秒數</summary>
        public static readonly int DBSyncResponseMilliSeconds = 800;

        /// <summary>轉帳限額</summary>
        public static readonly TPTransferAmountBound TPTransferAmountBound = new TPTransferAmountBound()
        {
            MinTPGameTransferAmount = 1,
            MaxTPGameTransferAmount = 50000
        };

        public static readonly decimal MinTransferToMiseAmount = 0.01m;
    }
}