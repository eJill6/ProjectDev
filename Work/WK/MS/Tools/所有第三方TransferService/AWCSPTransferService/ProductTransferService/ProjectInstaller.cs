
using JxBackendService.Model.Enums;
using JxBackendServiceNF.Installer;

namespace ProductTransferService
{
    public partial class ProjectInstaller : BaseProjectInstaller
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override PlatformProduct Product => PlatformProduct.AWCSP;
    }
}