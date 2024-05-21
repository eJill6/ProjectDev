using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.SubGame
{
    public class PMSLSubGameService : SubGameService
    {
        protected override GameLobbyType GameLobbyType => GameLobbyType.PMSL;

        public override string MobileApiBannerImageFileName => "banner_pm_rwd.png";

        public PMSLSubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}