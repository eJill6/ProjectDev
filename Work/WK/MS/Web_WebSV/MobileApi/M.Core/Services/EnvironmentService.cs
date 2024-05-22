using JxBackendService.Interface.Service;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;

namespace M.Core.Services
{
    [SingleInstance]
    public class EnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.MobileApi;
    }
}