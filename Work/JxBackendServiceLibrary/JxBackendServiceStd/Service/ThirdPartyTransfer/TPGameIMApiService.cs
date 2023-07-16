using JxBackendService.Common.Util;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty.Handicap;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public abstract class TPGameIMApiService : TPGameIMOneApiService
    {
        public TPGameIMApiService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public override PlatformProduct Product => PlatformProduct.IM;

        public override IIMOneAppSetting AppSetting => _gameAppSettingService.GetIMAppSetting();

        protected override PlatformHandicap ConvertToPlatformHandicap(string handicap)
        {
            IMOneHandicap imOneHandicap = IMOneHandicap.GetSingle(handicap);

            if (imOneHandicap == null)
            {
                return IMOneHandicap.EURO.PlatformHandicap;
            }

            return imOneHandicap.PlatformHandicap;
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            BaseReturnDataModel<string> returnModel = base.GetRemoteForwardGameUrl(tpGameRemoteLoginParam);

            //測試環境有鎖proxy ip
            if (returnModel.IsSuccess && EnvLoginUser.EnvironmentCode != EnvironmentCode.Production)
            {
                string imProxyUrl = (AppSetting as IIMAppSetting).IMLoginUrl.ToTrimString();
                var proxyUri = new Uri(imProxyUrl);

                var builder = new UriBuilder(returnModel.DataModel)
                {
                    Scheme = proxyUri.Scheme,
                    Host = proxyUri.Host,
                    Port = proxyUri.Port
                };

                returnModel.DataModel = builder.ToString();
            }

            return returnModel;
        }
    }
}