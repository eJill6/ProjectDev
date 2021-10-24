using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service.VIP.Activity;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.VIP;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.VIP.Activity
{
    public abstract class BaseVIPUserEventDetailService : BaseService, IVIPUserEventDetailService
    {
        protected IVIPUserInfoRep VIPUserInfoRep { get; private set; }

        protected IVIPUserEventDetailRep VIPUserEventDetailRep { get; private set; }

        public BaseVIPUserEventDetailService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            VIPUserInfoRep = ResolveJxBackendService<IVIPUserInfoRep>();
            VIPUserEventDetailRep = ResolveJxBackendService<IVIPUserEventDetailRep>();
        }

        public abstract BaseReturnModel VIPUserApplyForActivity();

        protected BaseReturnModel CreateVIPUserEventInfo(CreateVIPUserEventAuditParam auditParam)
        {
            // 建立活動審核單 
            var vipUserEvent = new VIPUserEventDetail
            {
                SEQID = VIPUserEventDetailRep.GetTableSequence(),
                EventTypeID = auditParam.EventTypeID,
                UserID = EnvLoginUser.LoginUser.UserId,
                UserName = EnvLoginUser.LoginUser.UserName,
                AuditStatus = auditParam.AuditStatus,
                Auditor = string.Empty,
                AuditMemo = string.Empty,
                CurrentLevel = auditParam.CurrentLevel,
                ApplyAmount = auditParam.ApplyAmount,
                EventAmount = auditParam.EventAmount,
                BonusAmount = auditParam.BonusAmount,
                FlowMultiple = auditParam.FlowMultiple,
                FinancialFlowAmount = auditParam.FinancialFlowAmount,
                RefID = auditParam.RefID,
                MemoJson = auditParam.MemoJson
            };

            BaseReturnDataModel<long> returnDataModel = VIPUserEventDetailRep.CreateByProcedure(vipUserEvent);

            return new BaseReturnModel(ReturnCode.GetSingle(returnDataModel.Code));
        }

        // 處理活動審核單
        public BaseReturnModel ProcessVIPUserEventAudit(BacksideEventAuditParam auditParam)
        {
            VIPUserEventDetail sourceAuditInfo = VIPUserEventDetailRep.GetSingleByKey(InlodbType.Inlodb, new VIPUserEventDetail { SEQID = auditParam.SEQID });

            if (sourceAuditInfo == null)
            {
                return new BaseReturnModel(ReturnCode.SearchResultIsEmpty);
            }

            if (sourceAuditInfo.AuditStatus != AuditStatusType.Unprocessed.Value)
            {
                return new BaseReturnModel(ReturnCode.AuditIsAlreadyCompleted);
            }

            string auditStatusName = AuditStatusType.GetName(auditParam.AuditStatus);

            if (auditStatusName.IsNullOrEmpty() || auditParam.AuditMemo.IsNullOrEmpty())
            {
                return new BaseReturnModel(ReturnCode.OperationFailed);
            }

            string memoContent = sourceAuditInfo.MemoJson.ToLocalizationContent();
            
            string operationLogContent = string.Format(VIPContentElement.EventAuditOperationContent, auditStatusName, memoContent, auditParam.AuditMemo);

            var processEventAuditParam = auditParam.CastByJson<ProcessEventAuditParam>();

            processEventAuditParam.AuditorUserID = EnvLoginUser.LoginUser.UserId;
            processEventAuditParam.Auditor = EnvLoginUser.LoginUser.UserName;
            processEventAuditParam.OperationLogContent = operationLogContent;

            return VIPUserEventDetailRep.ProcessVIPUserEventAudit(processEventAuditParam);
        }
    }
}
