using JxBackendService.Interface.Service;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;

namespace SLPolyGame.Web.Core.Service
{
    [SingleInstance]
    public class EnvironmentService : IEnvironmentService
    {
        public JxApplication Application => JxApplication.FrontSideWeb;
    }
}