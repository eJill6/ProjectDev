using System;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Repository.VIP;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Entity.VIP;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.Param.Audit.VIP;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;

namespace JxBackendService.Service.User
{
    public class UserRegisterService : BaseService, IUserRegisterService
    {
        private readonly IUserInfoRep _userInfoRep;
        private readonly IVIPUserInfoRep _vipUserInfoRep;
        private readonly IUserInfoAdditionalRep _userInfoAdditionalRep;
        private readonly Lazy<IAuditInfoService> _auditInfoService;

        public UserRegisterService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) :
            base(envLoginUser, dbConnectionType)
        {
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _vipUserInfoRep = ResolveJxBackendService<IVIPUserInfoRep>();
            _userInfoAdditionalRep = ResolveJxBackendService<IUserInfoAdditionalRep>();
            _auditInfoService = new Lazy<IAuditInfoService>(() => ResolveJxBackendService<IAuditInfoService>());
        }

        public BaseReturnModel UrlRegisterUser(UserRegisterParam userRegisterParam)
        {
            if (!IsValidRequired(userRegisterParam.UserName, userRegisterParam.UserPwdHash) ||
                userRegisterParam.ParentID <= 0)
            {
                LogUtil.ForcedDebug($"{nameof(UrlRegisterUser)}Rep: UserName is empty or UserPwd is empty or ParentID < 0 ");
                return new BaseReturnModel(ReturnCode.DataIsNotCompleted);
            }

            BaseReturnModel returnModel = _userInfoRep.UrlRegisterUser(userRegisterParam);

            if (!returnModel.IsSuccess)
            {
                LogUtil.ForcedDebug($@"Pro_UrlRegisterByCustomerType Fail: {new
                {
                    userRegisterParam.UserName,
                    userRegisterParam.ParentID,
                    userRegisterParam.RebatePro,
                    userRegisterParam.MaxRebatePro,
                    userRegisterParam.UpgradRebatePro,
                    userRegisterParam.CustomerType
                }.ToJsonString()} ");

                return returnModel;
            }

            return new BaseReturnModel(ReturnCode.Success);
        }

        public BaseReturnModel CheckVIPAgentQualify()
        {
            BaseReturnDataModel<AuditVIPAgentParam> dataModel = CheckAppliedForVIPAgentQualify();
            
            if (dataModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.Success);
            }

            return new BaseReturnModel(ReturnCode.GetSingle(dataModel.Code));
        }

        private BaseReturnDataModel<AuditVIPAgentParam> CheckAppliedForVIPAgentQualify()
        {
            int userId = EnvLoginUser.LoginUser.UserId;

            // 檢查用戶是否存在
            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo { UserID = userId });
            VIPUserInfo vipUserInfo = _vipUserInfoRep.GetSingleByKey(InlodbType.Inlodb, new VIPUserInfo { UserID = userId });

            if (vipUserInfo == null || userInfo == null)
            {
                return new BaseReturnDataModel<AuditVIPAgentParam>(ReturnCode.NotVIPUser);
            }

            // 檢查是否有手機
            if (userInfo.PhoneNumber.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<AuditVIPAgentParam>(ReturnCode.NoPhoneNumber);
            }

            // 檢查是否有真實姓名
            UserInfoAdditional userInfoAdditional = _userInfoAdditionalRep
               .GetSingleByKey(InlodbType.Inlodb, new UserInfoAdditional() { UserID = userId });

            if (userInfoAdditional == null || userInfoAdditional.RealName.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<AuditVIPAgentParam>(ReturnCode.NoRealName);
            }

            var auditVIPAgentParam = new AuditVIPAgentParam
            {
                PhoneNumber = userInfo.PhoneNumber.ToMaskPhoneNumber(),
                RealName = userInfoAdditional.RealName
            };

            return new BaseReturnDataModel<AuditVIPAgentParam>(ReturnCode.Success, auditVIPAgentParam);
        }

        public BaseReturnModel AppliedForVIPAgent(string userPwd)
        {
            BaseReturnDataModel<AuditVIPAgentParam> returnModel = CheckAppliedForVIPAgentQualify();

            if (!returnModel.IsSuccess)
            {
                return new BaseReturnModel(ReturnCode.GetSingle(returnModel.Code));
            }

            int userId = EnvLoginUser.LoginUser.UserId;

            // 建立審核單
            var auditInfo = new AuditInfoParam()
            {
                AuditType = AuditTypeValue.RegisterVIPAgent,
                RefID = userId.ToNonNullString(),
                RefTable = nameof(VIPAgentInfo),
                UserID = userId,
                Memo = string.Empty,
                BeforeValue = returnModel.DataModel.ToJsonString(),
                AuditValue = new 
                {
                    UserPwd = userPwd.ToPasswordHash()
                }.ToJsonString(),
                AddtionalAuditValue = string.Empty,
            };

            BaseReturnModel auditReturnModel = _auditInfoService.Value.CreateAuditInfo(auditInfo);

            return auditReturnModel;
        }
    }
}
