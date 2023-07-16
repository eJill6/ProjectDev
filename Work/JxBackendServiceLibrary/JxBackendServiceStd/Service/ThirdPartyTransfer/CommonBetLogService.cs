using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class CommonBetLogService : BaseService, ICommonBetLogService
    {
        public CommonBetLogService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public void SaveBetLogToPlatform(PlatformProduct product, List<InsertTPGameProfitlossParam> insertTPGameProfitlossParams)
        {
            var tpGameApiService = DependencyUtil.ResolveJxBackendService<ITPGameApiService>(product, Merchant, EnvLoginUser, DbConnectionTypes.Master);
            var transferSqlLiteRepository = DependencyUtil.ResolveKeyed<ITransferSqlLiteRepository>(product, Merchant);

            Func<string, SaveBetLogFlags, bool> updateSQLiteToSavedStatus = (keyId, saveBetLogFlag) =>
            {
                int affectRowCount;

                switch (saveBetLogFlag)
                {
                    case SaveBetLogFlags.Success:
                        affectRowCount = transferSqlLiteRepository.SaveProfitlossToPlatformSuccess(keyId);
                        break;

                    case SaveBetLogFlags.Fail:
                        affectRowCount = transferSqlLiteRepository.SaveProfitlossToPlatformFail(keyId);
                        break;

                    case SaveBetLogFlags.Ignore:
                        affectRowCount = transferSqlLiteRepository.SaveProfitlossToPlatformIgnore(keyId);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                return affectRowCount > 0;
            };

            tpGameApiService.SaveProfitlossToPlatform(insertTPGameProfitlossParams, updateSQLiteToSavedStatus);
        }
    }
}