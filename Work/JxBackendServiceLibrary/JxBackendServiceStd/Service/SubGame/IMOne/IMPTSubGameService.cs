using JxBackendService.Common.Util;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.SubGame.IMOne
{
    public class IMPTSubGameService : IMOneSubGameService
    {
        public IMPTSubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override GameLobbyType GameLobbyType => GameLobbyType.IMPT;

        protected override string ConvertToAmount(string response)
        {
            IMPTJackpotResult imptJackpotResult = response.Deserialize<IMPTJackpotResponseModel>().Result;

            if (imptJackpotResult == null)
            {
                return "0";
            }

            return imptJackpotResult.Amount.ToString();
        }

        protected override IIMOneAppSetting GetProductAppSetting() => GameAppSettingService.GetIMPTAppSetting();
    }
}