using JxBackendService.Common.Util;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ThirdParty.IM.Lottery;
using JxBackendService.Model.ViewModel;
using System.Linq;

namespace JxBackendService.Service.SubGame.IMOne
{
    public class IMPPSubGameService : IMOneSubGameService
    {
        protected virtual string IMOneProvider => IMOneProviderType.IMPP.Value;

        public IMPPSubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override GameLobbyType GameLobbyType => GameLobbyType.IMPP;

        protected override string ConvertToAmount(string response)
        {
            IMPPJackpotResult imppJackpotResult = response.Deserialize<IMPPJackpotResponseModel>().Result;

            if (imppJackpotResult == null)
            {
                return "0";
            }

            IMSlotGame imSlotGame = imppJackpotResult.Games.Where(w => w.Provider == IMOneProvider).SingleOrDefault();

            if (imSlotGame == null)
            {
                return "0";
            }

            return imSlotGame.Amount.ToString();
        }

        protected override IIMOneAppSetting GetProductAppSetting() => GameAppSettingService.GetIMPPAppSetting();
    }
}