using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.Param.Audit.VIP;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;

namespace JxBackendService.Service
{
    public class AuditInfoService : BaseService, IAuditInfoService
    {
        private readonly IAuditInfoRep _auditInfoRep;
        private readonly IUserInfoRep _userInfoRep;

        public AuditInfoService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _auditInfoRep = ResolveJxBackendService<IAuditInfoRep>();
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
        }

        public PagedResultModel<AuditInfo> GetList(AuditInfoQueryParam param, BasePagingRequestParam pageParam)
        {
            return _auditInfoRep.GetList(param, pageParam);
        }

        /// <summary>
        /// 建單
        /// </summary>
        public BaseReturnModel CreateAuditInfo(AuditInfoParam param)
        {
            //檢查目前有沒有尚未審核的單
            if (_auditInfoRep.CheckUnProcessAuditInfo((int)param.AuditType.Value, param.RefID))
            {
                return new BaseReturnDataModel<string>(ReturnCode.AlreadyExistAuditInfo, string.Empty);
            }

            if (_auditInfoRep.CheckUnProcessAuditInfoIsExistData((int)param.AuditType.Value, param.UserID, param.AddtionalAuditValue))
            {
                string errorMsg = null;

                if (param.AuditType == AuditTypeValue.GivePrize)
                {
                    errorMsg = MessageElement.AuditDataByUserIsExists;
                }
                else
                {
                    errorMsg = string.Format(MessageElement.AuditErrorMessage, param.AuditType.CheckFieldName);
                }

                return new BaseReturnDataModel<string>(errorMsg, "");
            }

            AuditInfo auditInfo = new AuditInfo
            {
                ID = _auditInfoRep.GetTableSequence(),
                AuditType = (int)param.AuditType.Value,
                UserID = param.UserID,
                UserName = _userInfoRep.GetFrontSideUserName(param.UserID),
                AuditContent = ProcessAuditContent(param),
                AuditValue = param.AuditValue,
                AuditStatus = AuditStatusType.Unprocessed.Value,
                RefTable = param.RefTable,
                RefID = param.RefID,
                Memo = param.Memo,
                AddtionalAuditValue = param.AddtionalAuditValue
            };

            var returnModel = _auditInfoRep.CreateByProcedure(auditInfo);

            if (returnModel.IsSuccess)
            {
                //log
                ProcessOperationLog(auditInfo.ID, GetOperationContent(auditInfo.AuditStatusText, auditInfo.AuditContent, auditInfo.Memo));
                return new BaseReturnDataModel<string>(ReturnCode.AuditInfoSubmit, auditInfo.ID);
            }

            return new BaseReturnDataModel<string>(ReturnCode.SystemError, "");
        }

        public BaseReturnModel Deal(string id, int auditStatus, int auditorUserId, string auditorUserName, string memo = "")
        {
            AuditInfoDealParam param = new AuditInfoDealParam();
            param.ID = id;
            param.AuditorUserId = auditorUserId;
            param.AuditorUserName = auditorUserName;
            param.AuditStatus = auditStatus;
            param.Memo = memo;

            BaseReturnModel returnModel = _auditInfoRep.Deal(param);

            if (returnModel.Code == ReturnCode.DataIsExist.Value ||
                returnModel.Message.ToNonNullString().Contains("已被绑定")) //搭配舊的SP關係先用寫死方式比對
            {
                //資料重覆要特別處理訊息
                var auditInfo = _auditInfoRep.GetSingleByKey(InlodbType.Inlodb, new AuditInfo(id));
                var auditType = AuditTypeValue.GetSingle(auditInfo.AuditType);
                return new BaseReturnModel(string.Format(AuditElement.DataIsExist, "新" + auditType.CheckFieldName));
            }
            return returnModel;
        }

        public string GetOperationContent(string auditStatusTypeName, string auditContent, string memo)
        {
            return string.Format(MessageElement.AuditOperationContent, auditContent, auditStatusTypeName, memo);
        }

        /// <summary>
        /// 處理LOG(建單時，審核完成時)
        /// </summary>
        public void ProcessOperationLog(string auditInfoId, string operationContent)
        {
            if (string.IsNullOrWhiteSpace(operationContent))
            {
                return;
            }

            AuditInfo auditInfo = _auditInfoRep.GetSingleByKey(InlodbType.Inlodb, new AuditInfo(auditInfoId));
            var operationLogService = ResolveJxBackendService<IOperationLogService>();

            operationLogService.InsertModifyMemberOperationLog(new InsertModifyMemberOperationLogParam()
            {
                Category = JxOperationLogCategory.Member,
                Content = operationContent,
                AffectedUserId = auditInfo.UserID,
                AffectedUserName = auditInfo.UserName
            });
        }

        /// <summary>
        /// 處理審核內容文字
        /// </summary>
        public string ProcessAuditContent(AuditInfoParam param)
        {
            string result = string.Empty;

            if (param.AuditType == AuditTypeValue.GivePrize)
            {
                AuditGivePrizeParam givePrizeParam = param.AuditValue.Deserialize<AuditGivePrizeParam>();
                result = string.Format(AuditElement.AuditGivePrizeContent, 
                    AuditTypeValue.GivePrize.Name, 
                    ProfitLossTypeName.GetName(givePrizeParam.ProfitLossType), 
                    givePrizeParam.Amount);
            }
            else if (param.AuditType == AuditTypeValue.BankName)
            {
                AuditBankParam obj = param.AuditValue.Deserialize<AuditBankParam>();
                AuditBankParam before = param.BeforeValue.Deserialize<AuditBankParam>();
                result = string.Format(AuditElement.AuditBankContent, before.BankTypeName, StringUtil.MaskBankCardContent(before.BankCard), before.CardUser, before.IsActiveText,
                                                                    obj.BankTypeName, StringUtil.MaskBankCardContent(obj.BankCard), obj.CardUser, obj.IsActiveText);
            }
            else if (param.AuditType == AuditTypeValue.USDT)
            {
                AuditUsdtParam obj = param.AuditValue.Deserialize<AuditUsdtParam>();
                AuditUsdtParam before = param.BeforeValue.Deserialize<AuditUsdtParam>();
                result = string.Format(AuditElement.AuditUsdtContent, StringUtil.MaskUSDT(before.WalletAddr), before.IsActiveText,
                                                                      StringUtil.MaskUSDT(obj.WalletAddr), obj.IsActiveText);
            }
            else if (param.AuditType == AuditTypeValue.Mobile)
            {
                AuditUserDataParam obj = param.AuditValue.Deserialize<AuditUserDataParam>();
                AuditUserDataParam before = param.BeforeValue.Deserialize<AuditUserDataParam>();
                result = string.Format(AuditElement.AuditMobileContent, before.Content.ToMaskPhoneNumber(), obj.Content.ToMaskPhoneNumber());
            }
            else if (param.AuditType == AuditTypeValue.Email)
            {
                AuditUserDataParam obj = param.AuditValue.Deserialize<AuditUserDataParam>();
                AuditUserDataParam before = param.BeforeValue.Deserialize<AuditUserDataParam>();
                result = string.Format(AuditElement.AuditEmailContent, before.Content.ToMaskEmail(), obj.Content.ToMaskEmail());
            }
            else if (param.AuditType == AuditTypeValue.UnbindUserAuthenticator)
            {
                result = AuditElement.UnbindUserAuthenticatorAuditContent;
            }
            else if (param.AuditType == AuditTypeValue.LoginPassword)
            {
                result = string.Format(AuditElement.AuditTypeResetPassword, param.UserID, AuditElement.LoginPassword);
            }
            else if (param.AuditType == AuditTypeValue.MoneyPassword)
            {
                result = string.Format(AuditElement.AuditTypeResetPassword, param.UserID, AuditElement.MoneyPassword);
            }
            else if (param.AuditType == AuditTypeValue.RegisterVIPAgent)
            {
                AuditVIPAgentParam vipAgentParam = param.BeforeValue.Deserialize<AuditVIPAgentParam>();
                result = string.Format(AuditElement.AuditVIPAgentContent, vipAgentParam.RealName, vipAgentParam.PhoneNumber);
            }

            return result;
        }
    }
}
