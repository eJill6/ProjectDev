using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using System;

namespace BatchService.Job
{
    public class TransferToMiseLiveJob : BaseQueueUserWorkItemJob, IQueueUserWorkItemJob
    {
        private readonly int _workerCount = 4;

        public TransferToMiseLiveJob()
        {
        }

        /// <summary>
        /// 轉入第三方帳戶
        /// </summary>
        public void DoJob(object state)
        {
            IMessageQueueService messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(JxApplication.BatchService);

            for (int i = 1; i <= _workerCount; i++)
            {
                messageQueueService.StartNewDequeueJob(TaskQueueName.TransferToMiseLive, DoJobAfterReceived);
            }
        }

        private bool DoJobAfterReceived(string message)
        {
            TransferToMiseLiveParam transferToMiseLiveParam;

            try
            {
                transferToMiseLiveParam = message.Deserialize<TransferToMiseLiveParam>();
            }
            catch (Exception ex)
            {
                LogUtilService.ForcedDebug($"TransferToMiseLive DoJobAfterReceived:{message}");
                ErrorMsgUtil.ErrorHandle(ex, EnvUser);

                return true;
            }

            EnvironmentUser environmentUser = CreateEnvironmentUser(transferToMiseLiveParam.UserID);
            var withdrawService = DependencyUtil.ResolveJxBackendService<IWithdrawService>(environmentUser, DbConnectionTypes.Master);

            if (transferToMiseLiveParam.Amount < GlobalVariables.MinTransferToMiseAmount)
            {
                return true;
            }

            BaseReturnModel baseReturnModel = withdrawService.WithdrawToMiseLive(transferToMiseLiveParam.Amount);

            if (!baseReturnModel.IsSuccess)
            {
                LogUtilService.ForcedDebug($"WithdrawToMiseLive Fail:{baseReturnModel.Message}");
            }

            //不管成功或失敗要讓queue繼續處理下一筆, 最多讓用戶重新轉帳回去
            return true;
        }
    }
}