using BatchService.Job.Base;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Finance;

namespace BatchService.Job
{
    public class CheckIdleAvailableScoresJob : BaseBatchServiceQuartzJob
    {
        private readonly Lazy<IUserInfoRelatedReadService> _userInfoRelatedReadService;

        private readonly Lazy<IInternalMessageQueueService> _internalMessageQueueService;

        public CheckIdleAvailableScoresJob()
        {
            _userInfoRelatedReadService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvUser, DbConnectionTypes.Slave);
            _internalMessageQueueService = DependencyUtil.ResolveService<IInternalMessageQueueService>();
        }

        public override void DoJob()
        {
            List<UserInfo> userInfos = _userInfoRelatedReadService.Value.GetIdleScoreUsers();

            foreach (UserInfo userInfo in userInfos)
            {
                var transferToMiseLiveParam = new TransferToMiseLiveParam()
                {
                    UserID = userInfo.UserID,
                    Amount = userInfo.AvailableScores.GetValueOrDefault()
                };

                _internalMessageQueueService.Value.EnqueueTransferToMiseLiveMessage(transferToMiseLiveParam);
            }
        }
    }
}