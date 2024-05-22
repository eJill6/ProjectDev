using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;

namespace ProductTransferService.Service
{
    public class EnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.CQ9SLTransferService;
    }
}