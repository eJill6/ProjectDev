using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;

namespace JxBackendService.Service.SubGame
{
    public class JDBFISubGameService : SubGameService
    {
        protected override GameLobbyType GameLobbyType => GameLobbyType.JDBFI;

        public JDBFISubGameService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }
    }
}