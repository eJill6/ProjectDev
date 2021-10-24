using ApolloService;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Finance.Apollo;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Model.ViewModel.Merchant;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Merchant.Base;

namespace JxBackendService.Service.Merchant.CTL
{
    public class MerchantSettingCTLService : BaseMerchantSettingService, IMerchantSettingService
    {
        public MerchantSettingCTLService()
        {

        }

        public string BrandCode => "AMD";

        public string BrandName => string.Empty;

        public PlatformCulture PlatformCulture => PlatformCulture.China;

        public bool IsRWD => false;

        public string UrlRegisterKey => "cpolyhag";

        /// <summary> 汉字，英数，以及下划线 </summary>
        public string UserNameRegularExpressionPattern => @"^[a-zA-Z0-9_\u4e00-\u9fa5]+$";

        public RegisterSettingInfo RegisterSettingInfo => new RegisterSettingInfo
        {
            IsUseRebate = true,
            DefaultRebatePro = 0m,
            MaxRebatePro = 0.07m
        };

        public bool IsForceAssignParent => true;

        public ApolloService.Setting GetApolloSetting(JxApplication jxApplication)
        {
            ApolloConfigParam param = GetApolloConfigParam(jxApplication);
            return new JXSetting(param.ApolloPostUrl, param.Apollokey);
        }

        public BackSidePageInitSetting GetBackSidePageInitSetting()
        {
            var initData = new BackSidePageInitSetting()
            {
                IsVIPInfosVisible = false,
                IsRebateInfosVisible = true,
                UserInfoAvailableScoresText = UserRelatedElement.AvailableScoresNameByTypeAgent,
                UserInfoFreezeScoresText = UserRelatedElement.FreezeScoresNameByTypeAgent
            };

            return initData;
        }
    }
}