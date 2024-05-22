using JxBackendServiceNF.Installer;
using JxBackendService.Model.Enums;

namespace ProductTransferService
{
    public partial class ProjectInstaller : BaseProjectInstaller
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override PlatformProduct Product => PlatformProduct.OBSP;
    }
}
