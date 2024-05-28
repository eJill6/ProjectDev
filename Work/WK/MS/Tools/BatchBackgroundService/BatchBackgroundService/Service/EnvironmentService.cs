using JxBackendService.Interface.Service;
using JxBackendService.Model.Enums;

namespace BatchService.Service
{
    public class EnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.BatchService;
    }
}