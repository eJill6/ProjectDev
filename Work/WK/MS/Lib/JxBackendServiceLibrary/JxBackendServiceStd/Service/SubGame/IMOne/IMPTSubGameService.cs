using JxBackendService.Common.Util;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.SubGame.IMOne
{
    public class IMPTSubGameService : IMOneSubGameService
    {
        protected override GameLobbyType GameLobbyType => GameLobbyType.IMPT;

        public override string MobileApiBannerImageFileName => "banner_pt_rwd.png";

        public IMPTSubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override string ConvertToAmount(string response)
        {
            IMPTJackpotResult imptJackpotResult = response.Deserialize<IMPTJackpotResponseModel>().Result;

            if (imptJackpotResult == null)
            {
                return "0";
            }

            return imptJackpotResult.Amount.Floor(2).ToString();
        }

        protected override IIMOneAppSetting GetProductAppSetting() => GameAppSettingService.GetIMPTAppSetting();
    }
}