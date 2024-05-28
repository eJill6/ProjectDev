using BatchService.Interface;
using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Queue;
using JxBackendService.Model.Param.Finance;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using System;
using System.Collections.Generic;

namespace BatchService.Job
{
    public class CheckIdleAvailableScoresJob : BaseBatchServiceQuartzJob
    {
        private readonly IUserInfoRelatedReadService _userInfoRelatedReadService;

        private readonly IMessageQueueService _messageQueueService;

        public CheckIdleAvailableScoresJob()
        {
            _userInfoRelatedReadService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvUser, DbConnectionTypes.Slave);
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(EnvUser.Application);
        }

        public override void DoJob()
        {
            List<UserInfo> userInfos = _userInfoRelatedReadService.GetIdleScoreUsers();

            foreach (UserInfo userInfo in userInfos)
            {
                var transferToMiseLiveParam = new TransferToMiseLiveParam()
                {
                    UserID = userInfo.UserID,
                    Amount = userInfo.AvailableScores.GetValueOrDefault()
                };

                _messageQueueService.EnqueueTransferToMiseLiveMessage(transferToMiseLiveParam);
            }
        }
    }
}