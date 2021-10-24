using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.DownloadFile;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using JxBackendService.Service.Game;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseThirdPartyApiWCFService : BaseApplicationService
    {
        public BaseReturnDataModel<UserScore> GetBalance(string productCode)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService(productCode, DbConnectionTypes.Slave);
            return gameApiService.GetRemoteUserScore(EnvLoginUser.LoginUser.UserId, false);
        }

        public BaseReturnDataModel<string> GetForwardGameUrl(string productCode, string ip, JxIpInformation ipInfo)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService(productCode, DbConnectionTypes.Master);
            //登入紀錄
            var gameUserService = ResolveJxBackendService<IGameUserService>(DbConnectionTypes.Master);
            gameUserService.CreateLoginHistory(PlatformProduct.GetSingle(productCode), 1, ipInfo);

            bool isMobile = EnvLoginUser.Application == JxApplication.MobileApi;
            return gameApiService.GetForwardGameUrl(EnvLoginUser.LoginUser.UserId, EnvLoginUser.LoginUser.UserName, ip, isMobile);
        }

        public BaseReturnModel TransferIN(string productCode, decimal amount)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService(productCode, DbConnectionTypes.Master);
            return gameApiService.CreateTransferInInfo(EnvLoginUser.LoginUser.UserId, EnvLoginUser.LoginUser.UserName, amount);
        }

        public BaseReturnModel TransferOUT(string productCode, decimal amount)
        {
            ITPGameApiService gameApiService = ResolveJxBackendService(productCode, DbConnectionTypes.Master);
            return gameApiService.CreateTransferOutInfo(EnvLoginUser.LoginUser.UserId, EnvLoginUser.LoginUser.UserName, amount);
        }

        //public void UseMockService(string productCode)
        //{
        //    if (SharedAppSettings.GetEnvironmentCode(EnvLoginUser.Application) == EnvironmentCode.Production)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    PlatformProduct.GetSingle(productCode).FnIsUseMockService = () => true;
        //}

        private ITPGameApiService ResolveJxBackendService(string productCode, DbConnectionTypes dbConnectionType)
        {
            PlatformProduct product = PlatformProduct.GetSingle(productCode);
            return ResolveJxBackendService<ITPGameApiService>(product, dbConnectionType);
        }
    }
}
