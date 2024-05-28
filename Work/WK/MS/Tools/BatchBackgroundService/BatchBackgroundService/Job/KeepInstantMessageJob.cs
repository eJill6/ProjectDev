using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Chat;
using JxBackendService.Common.Extensions;
using JxBackendService.Model.Entity.Chat;

namespace BatchService.Job
{
    public class KeepInstantMessageJob : BaseBatchServiceQuartzJob
    {
        private readonly Lazy<IOneOnOneChatService> _oneOnOneChatService;

        private readonly Lazy<IOneOnOneChatReadService> _oneOnOneChatReadService;

        private static readonly int s_deleteMessageWaitMilliseconds = 300;

        private static readonly int s_deleteQuantity = 300;

        private static readonly int s_overLimitDays = 3;

        private static readonly int s_fetchCount = 100000;

        public KeepInstantMessageJob()
        {
            _oneOnOneChatService = DependencyUtil.ResolveJxBackendService<IOneOnOneChatService>(EnvUser, DbConnectionTypes.IMsgMaster);
            _oneOnOneChatReadService = DependencyUtil.ResolveJxBackendService<IOneOnOneChatReadService>(EnvUser, DbConnectionTypes.IMsgSlave);
        }

        public override void DoJob()
        {
            var queryParam = new QueryOneOnOneMessageParam
            {
                PublishTimestamp = DateTime.UtcNow.AddDays(-s_overLimitDays).ToUnixOfTime(),
            };

            // 根據取資料量&過期天數取得欲刪ChatMessages
            List<MSIMOneOnOneChatMessageKey> overLimitOneOnOneChatMessagesList = _oneOnOneChatReadService.Value.GetOneOnOneChatMessageKeys(queryParam, s_fetchCount);

            // 過濾篩出LastMessage需要的條件
            var lastMessageIds = overLimitOneOnOneChatMessagesList
                .GroupBy(x => new { x.OwnerUserID, x.DialogueUserID })
                .Select(group => new MSIMOneOnOneChatMessageKey()
                {
                    OwnerUserID = group.Key.OwnerUserID,
                    DialogueUserID = group.Key.DialogueUserID,
                    MessageID = group.Max(x => x.MessageID)
                }).Select(s => s.MessageID)
                .ToList();

            // 取得欲刪LastMessages
            List<MSIMLastMessageKey> lastMessageKeys = _oneOnOneChatReadService.Value.GetLastMessageKeys(lastMessageIds);

            // 先刪Last再刪Chat
            while (lastMessageKeys.Any())
            {
                _oneOnOneChatService.Value.DeleteLastMessages(lastMessageKeys.Take(s_deleteQuantity).ToList());

                lastMessageKeys.RemoveRangeByFit(0, s_deleteQuantity);

                TaskUtil.DelayAndWait(s_deleteMessageWaitMilliseconds);
            }

            while (overLimitOneOnOneChatMessagesList.Any())
            {
                _oneOnOneChatService.Value.DeleteChatMessages(overLimitOneOnOneChatMessagesList.Take(s_deleteQuantity).ToList());

                overLimitOneOnOneChatMessagesList.RemoveRangeByFit(0, s_deleteQuantity);

                TaskUtil.DelayAndWait(s_deleteMessageWaitMilliseconds);
            }
        }
    }
}