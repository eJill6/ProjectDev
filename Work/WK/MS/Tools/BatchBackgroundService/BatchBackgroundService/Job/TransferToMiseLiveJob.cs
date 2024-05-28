using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;

namespace BatchService.Job
{
    public class TransferToMiseLiveJob : BaseTaskJob, ITaskJob
    {
        private readonly int _workerCount = 4;

        public TransferToMiseLiveJob()
        {
        }

        /// <summary>
        /// 轉回Mise帳戶
        /// </summary>
        protected override void DoWork(CancellationToken cancellationToken)
        {
            IInternalMessageQueueService internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>().Value;

            for (int i = 1; i <= _workerCount; i++)
            {
                internalMessageQueueService.StartNewDequeueJob(TaskQueueName.TransferToMiseLive, DoJobAfterReceived);
            }
        }

        private bool DoJobAfterReceived(DoDequeueJobAfterReceivedParam doDequeueJobAfterReceivedParam)
        {
            TransferToMiseLiveParam transferToMiseLiveParam;

            try
            {
                transferToMiseLiveParam = doDequeueJobAfterReceivedParam.Message.Deserialize<TransferToMiseLiveParam>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"TransferToMiseLive DoJobAfterReceived:{doDequeueJobAfterReceivedParam.Message}");
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);

                return true;
            }

            string returnMessage = string.Empty;
            EnvironmentUser environmentUser = CreateEnvironmentUser(transferToMiseLiveParam.UserID);
            var withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(environmentUser, DbConnectionTypes.Master).Value;

            if (transferToMiseLiveParam.Amount >= GlobalVariables.MinTransferToMiseAmount)
            {
                BaseReturnModel baseReturnModel = withdrawService.WithdrawToMiseLive(transferToMiseLiveParam.Amount, transferToMiseLiveParam.ProductCode);
                returnMessage = baseReturnModel.Message;
            }
            else
            {
                returnMessage = string.Format(MessageElement.InsufficientBalance, GlobalVariables.MinTransferToMiseAmount);
            }

            if (transferToMiseLiveParam.RoutingSetting != null && !transferToMiseLiveParam.RoutingSetting.RoutingKey.IsNullOrEmpty())
            {
                var messageQueueService = DependencyUtil.ResolveService<IMessageQueueService>().Value;

                messageQueueService.SendBackSideWebTransferMessage(
                    transferToMiseLiveParam.RoutingSetting.RoutingKey,
                    new TransferMessage()
                    {
                        ProductCode = transferToMiseLiveParam.ProductCode,
                        RequestId = transferToMiseLiveParam.RoutingSetting.RequestId,
                        Summary = string.Format(MessageElement.RelayWalletToMiseLive, returnMessage),
                        IsReloadMiseLiveBalance = true
                    });
            }

            //不管成功或失敗要讓queue繼續處理下一筆, 最多讓用戶重新轉帳回去
            return true;
        }
    }
}