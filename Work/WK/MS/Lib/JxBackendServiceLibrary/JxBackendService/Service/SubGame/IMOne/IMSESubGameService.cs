using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.SubGame.IMOne
{
    public class IMSESubGameService : IMPPSubGameService
    {
        protected override GameLobbyType GameLobbyType => GameLobbyType.IMSE;

        protected override string IMOneProvider => IMOneProviderType.IMSE.Value;

        public IMSESubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}