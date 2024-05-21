using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace JxBackendService.Installer
{
    [RunInstaller(true)]
    public abstract class BaseProjectInstaller : System.Configuration.Install.Installer
    {
        //private static readonly string _uninstallBatFile = "卸载.bat";
        private static readonly string _uninstallBatFile = "uninstall.bat";        
        private readonly ServiceProcessInstaller _serviceProcessInstaller;
        private readonly ServiceInstaller _serviceInstaller;

        protected abstract PlatformProduct Product { get; }


        public BaseProjectInstaller()
        {
            _serviceProcessInstaller = new ServiceProcessInstaller()
            {
                Account = ServiceAccount.LocalService,
                Username = null,
                Password = null
            };

            Installers.Add(_serviceProcessInstaller);

            _serviceInstaller = new ServiceInstaller
            {
                Description = $"{Product.Value} 轉帳及投注明細處理服務",
                ServiceName = $"{Product.Value}TransferService",
                DisplayName = $"{Product.Value}TransferService",
                StartType = ServiceStartMode.Automatic,
            };

            _serviceInstaller.AfterInstall += ServiceInstaller_AfterInstall;

            Installers.Add(_serviceInstaller);
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            string uninstallBatFilePath = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/" + _uninstallBatFile;

            if (File.Exists(uninstallBatFilePath))
            {
                string uninstallBat = File.ReadAllText(uninstallBatFilePath);
                uninstallBat = uninstallBat.Replace("{ProductCode}", Product.Value);
                File.WriteAllText(uninstallBatFilePath, uninstallBat);
            }

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
                //ignore;
                //有可能因為不明原因無法啟動,不CATCH的話安裝程式會直接rollback
            }
        }
    }
}
