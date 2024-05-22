using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;

namespace ProductTransferService.LCDataBase.DLL
{
    public interface ILCConfigService
    {
        string AgentID { get; }

        string LCUserHeader { get; }
    }

    public class LCConfigService : ILCConfigService
    {
        private readonly Lazy<IConfigUtilService> _configUtilService;

        public LCConfigService()
        {
            _configUtilService = DependencyUtil.ResolveService<IConfigUtilService>();
            AgentID = _configUtilService.Value.Get("AgentID").Trim();
            LCUserHeader = AgentID + "_";
        }

        public string AgentID { get; private set; }

        public string LCUserHeader { get; private set; }
    }
}