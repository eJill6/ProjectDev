using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using SportDataBase.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SportTransferService.SportTransferService sport = new SportTransferService.SportTransferService();
            sport.InitLocalAppSettings();

            EnvironmentUser envUser = new EnvironmentUser()
            {
                Application = JxApplication.SportTransferService,
                LoginUser = new BasicUserInfo() { UserId = 0, UserName = GlobalVariables.SystemOperator }
            };

            new Transfer(envUser, DbConnectionTypes.Master).RefreshAvailableScores(false);
        }
    }
}
