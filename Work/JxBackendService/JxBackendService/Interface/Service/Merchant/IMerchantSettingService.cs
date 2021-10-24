using ApolloService;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Model.ViewModel.Merchant;

namespace JxBackendService.Interface.Service.Merchant
{
    public interface IMerchantSettingService
    {
        string BrandCode { get; }

        string BrandName { get; }

        PlatformCulture PlatformCulture { get; }

        bool IsRWD { get; }

        /// <summary>註冊連結加解密KEY，不可異動會無法反解</summary>
        string UrlRegisterKey { get; }

        /// <summary>帳號規則正規表達式</summary>
        string UserNameRegularExpressionPattern { get; }

        /// <summary>不同商戶註冊設定條件相關資訊</summary>
        RegisterSettingInfo RegisterSettingInfo { get; }

        /// <summary>是否強制指定上級，不啟用的話代表會有預設上級，UserId = 1</summary>
        bool IsForceAssignParent { get; }

        /// <summary>Apollo串接設定</summary>
        Setting GetApolloSetting(JxApplication jxApplication);

        /// <summary>後台頁面設定</summary>
        BackSidePageInitSetting GetBackSidePageInitSetting();
    }
}