using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Finance.Apollo;
using JxBackendService.Model.ViewModel.BackSideWeb;
using JxBackendService.Model.ViewModel.Merchant;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Merchant.Base;

namespace JxBackendService.Service.Merchant.CTS
{
    public class MerchantSettingCTSService : BaseMerchantSettingService, IMerchantSettingService
    {
        public MerchantSettingCTSService()
        {

        }

        public string BrandCode => "AMD2";

        public string BrandName => string.Empty;

        public PlatformCulture PlatformCulture => PlatformCulture.China;

        public bool IsRWD => true;

        public string UrlRegisterKey => "frtycwep";

        /// <summary> 只能英数</summary>
        public string UserNameRegularExpressionPattern => @"^[a-zA-Z0-9]*$";

        public RegisterSettingInfo RegisterSettingInfo => new RegisterSettingInfo
        {
            IsUseRebate = false,
            DefaultRebatePro = 0.077m,
            MaxRebatePro = 0.077m,
            UpgradRebatePro = 0m,
        };

        public bool IsForceAssignParent => false;

        public ApolloService.Setting GetApolloSetting(JxApplication jxApplication)
        {
            ApolloConfigParam param = GetApolloConfigParam(jxApplication);
            return new ApolloCTSSetting(param.ApolloPostUrl, param.Apollokey);
        }

        public BackSidePageInitSetting GetBackSidePageInitSetting()
        {
            var initData = new BackSidePageInitSetting()
            {
                IsVIPInfosVisible = true,
                IsRebateInfosVisible = false,
                UserInfoAvailableScoresText = UserRelatedElement.AvailableScoresNameByTypeDirect,
                UserInfoFreezeScoresText = UserRelatedElement.FreezeScoresNameByTypeDirect
            };

            return initData;
        }
    }
}
