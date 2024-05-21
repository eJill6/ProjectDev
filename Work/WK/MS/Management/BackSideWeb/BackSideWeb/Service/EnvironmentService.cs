using JxBackendService.Interface.Service;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;

namespace BackSideWeb.Service
{
    [SingleInstance]
    public class EnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.BackSideWeb;
    }
}