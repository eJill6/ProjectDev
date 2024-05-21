using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.ThirdPartyTransfer
{
    public class TPGameTransferOutQueueService : BaseService, ITPGameTransferOutQueueService
    {
        private static readonly int s_transferAllOutWorkerCount = 4;

        public TPGameTransferOutQueueService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public void StartDequeueTransferAllOutJob(PlatformProduct product)
        {
            IMessageQueueService messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.BatchService);
            ITPGameTransferOutService tpGameTransferOutService = ResolveJxBackendService<ITPGameTransferOutService>(DbConnectionTypes.Master);

            for (int i = 1; i <= s_transferAllOutWorkerCount; i++)
            {
                messageQueueService.StartNewDequeueJob(
                    TaskQueueName.TransferAllOut(product),
                    (string message) =>
                    {
                        LogUtil.ForcedDebug($"Product:{product.Value} Received Message:{message}");
                        TransferOutUserDetail transferOutUserDetail;

                        try
                        {
                            transferOutUserDetail = message.Deserialize<TransferOutUserDetail>();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.ForcedDebug($"TransferAllOut DoJobAfterReceived:{message}");
                            ErrorMsgUtil.ErrorHandle(ex, EnvLoginUser);

                            return true;
                        }

                        bool isSuccess = tpGameTransferOutService.ProcessTransferAllOutQueue(product, transferOutUserDetail);

                        if (!isSuccess)
                        {
                            LogUtil.ForcedDebug($"ProcessTransferAllOutQueue Fail:{transferOutUserDetail.ToJsonString()}");
                        }

                        //不管成功或失敗要讓queue繼續處理下一筆, 最多讓用戶重新轉帳回去
                        return true;
                    });
            }
        }
    }
}