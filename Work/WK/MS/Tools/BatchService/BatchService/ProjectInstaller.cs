using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BatchService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller _serviceProcessInstaller;
        private readonly ServiceInstaller _serviceInstaller;
        public ProjectInstaller()
        {
            InitializeComponent();

            _serviceProcessInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalService,
                Username = null,
                Password = null
            };

            Installers.Add(_serviceProcessInstaller);

            _serviceInstaller = new ServiceInstaller
            {
                Description = $"GOG批次處理服務",
                ServiceName = $"BatchService",
                DisplayName = $"BatchService",
                StartType = ServiceStartMode.Automatic,
            };

            _serviceInstaller.AfterInstall += ServiceInstaller_AfterInstall;

            Installers.Add(_serviceInstaller);
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                var serviceInstaller = (ServiceInstaller)sender;

                using (ServiceController serviceController = new ServiceController(serviceInstaller.ServiceName))
                {
                    serviceController.Start();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
