using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.SubGame.IMOne
{
    public class IMJDBSubGameService : IMPPSubGameService
    {
        protected override GameLobbyType GameLobbyType => GameLobbyType.IMJDB;

        protected override string IMOneProvider => IMOneProviderType.IMJDB.Value;

        public IMJDBSubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}