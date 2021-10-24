using ApolloService;
using ApolloService.Commands;
using ApolloService.Invoker;
using ApolloServiceModel.Info;
using ApolloServiceModel.Request;
using ApolloServiceModel.Response;
using JxBackendService.Common.Extensions;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.Config;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Finance.Apollo;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendServiceNet45.Interface.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JxBackendServiceNet45.Service.Base
{
    public abstract class BaseSLPolyGameWCFService : BaseApplicationService, IBaseSLPolyGameWCFService
    {
        private readonly decimal _minRechargeAmount = 10;

        protected abstract Action AfterSaveLoginPasswordSuccess { get; }

        /// <summary>檢查資金密碼</summary>
        public BaseReturnModel CheckMoneyPwd(int webActionTypeValue, string moneyPassword, bool isEncrypted)
        {
            return DoWorkWithErrorHandle(() =>
            {
                var userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>(DbConnectionTypes.Slave);
                UserInfo userInfo = userInfoRelatedService.GetUserInfo(EnvLoginUser.LoginUser.UserId);

                if (userInfo == null)
                {
                    return new BaseReturnModel(ReturnCode.UserNotFound);
                }

                if (!userInfo.IsActive)
                {
                    return new BaseReturnModel(ReturnCode.YourAccountIsDisabled);
                }

                if (!string.IsNullOrEmpty(moneyPassword) && !isEncrypted)
                {
                    moneyPassword = moneyPassword.ToPasswordHash();
                }

                if (userInfo.MoneyPwd == moneyPassword)
                {
                    return new BaseReturnModel(ReturnCode.Success);
                }

                //寫入後台操作日誌與錯誤處理
                var failureOperationService = ResolveJxBackendService<IFailureOperationService>(DbConnectionTypes.Master);
                WebActionType webActionType = WebActionType.GetSingle(webActionTypeValue);
                failureOperationService.AddFailOperation(webActionType, SubActionType.MoneyPasswordWrong);

                return new BaseReturnModel(ReturnCode.YourMoneyPasswordIsWrong);

            }, new BaseReturnModel(ReturnCode.OperationFailed));
        }

        /// <summary>密码修改</summary>
        public BaseReturnModel UpdatePwd(bool isEncrypt, string oldPassword, string newPassword, int passwordTypeValue,
                                         string validateCode)
        {
            return DoWorkWithErrorHandle(() =>
            {
                var userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>(DbConnectionTypes.Master);

                if (!userInfoRelatedService.GetUserInfo(EnvLoginUser.LoginUser.UserId).IsActive)
                {
                    return new BaseReturnModel(ReturnCode.YourAccountIsDisabled);
                }

                PasswordType passwordType = PasswordType.GetSingle(passwordTypeValue);

                var validateCodeService = ResolveJxBackendService<IValidateCodeService>(DbConnectionTypes.Slave);

                BaseReturnModel checkReturnModel = validateCodeService.CheckGraphicValidateCode(
                    passwordType.ModifyPasswordActionType,
                    EnvLoginUser.LoginUser.UserKey,
                    validateCode);

                var ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
                string ipAddress = ipUtilService.GetIPAddress();

                if (!checkReturnModel.IsSuccess)
                {
                    if (checkReturnModel.Code == ReturnCode.ValidateCodeIncorrect)
                    {
                        //驗證碼錯誤，要確認是否已達到輸入錯誤上限
                        var _blockActionService = ResolveJxBackendService<IBlockActionService>(DbConnectionTypes.Master);
                        bool isBlock = _blockActionService.BlockWebActionClient(ipAddress,
                                                                      passwordType.ModifyPasswordActionType.Value);

                        // IP已進入黑名單
                        if (isBlock)
                        {
                            return new BaseReturnModel(SecurityElement.ModifiedManyTimesToBlockIp);
                        }
                    }

                    return checkReturnModel;
                }

                string oldPasswordHash = oldPassword;
                string newPasswordHash = newPassword;

                if (!isEncrypt)
                {
                    if (!Regex.IsMatch(newPassword, passwordType.ValidRegularExpressionType.Pattern))
                    {
                        return new BaseReturnModel(passwordType.ValidRegularExpressionType.ErrorMsg);
                    }

                    oldPasswordHash = oldPassword.ToPasswordHash();
                    newPasswordHash = newPassword.ToPasswordHash();
                }

                var failureLoginHistoryRep = ResolveJxBackendService<IFailureLoginHistoryRep>(DbConnectionTypes.Master);
                var failureOperationService = ResolveJxBackendService<IFailureOperationService>(DbConnectionTypes.Master);

                BaseReturnModel returnModel = userInfoRelatedService.SavePassword(
                    new SavePasswordParam()
                    {
                        OldPasswordHash = oldPasswordHash,
                        NewPasswordHash = newPasswordHash,
                        UserID = EnvLoginUser.LoginUser.UserId,
                        SavePasswordType = passwordType,
                    }, AfterSaveLoginPasswordSuccess);

                return returnModel;

            }
            , new BaseReturnModel(MessageElement.OperationFail));
        }

        /// <summary>更新用户下级充值功能</summary>
        public BaseReturnModel SaveChildTransferStatus(int childUserId, bool isEnabled)
        {
            var userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>(DbConnectionTypes.Master);
            BaseReturnModel returnModel = userInfoRelatedService.SaveChildTransferStatus(
                childUserId,
                TransferToChildStatus.GetSingle(Convert.ToInt32(isEnabled)),
                null);

            return returnModel;
        }

        public GetAllServiceAmountLimit_ResponseV2 ApolloGetAllServiceAmountLimitV2(int cardGroupSeq = -1)
        {
            var defaultResult = new GetAllServiceAmountLimit_ResponseV2()
            {
                //server發生錯誤
                Code = "-1",
                Message = ""
            };

            return DoWorkWithErrorHandle(() =>
            {
                var merchantSettingService = DependencyUtil.ResolveKeyed<IMerchantSettingService>(SharedAppSettings.PlatformMerchant);

                Setting setting = merchantSettingService.GetApolloSetting(EnvLoginUser.Application);
                AbstractInvoker<GetAllServiceAmountLimit_ResponseV2> invoker = new Invoker<GetAllServiceAmountLimit_ResponseV2>();

                Apollo_Request apolloRequest = new GetAllServiceAmountLimit_Request();
                apolloRequest.CardGroupSeq = cardGroupSeq;
                apolloRequest.MerchantNo = setting.MerchantNO;
                apolloRequest.SignType = setting.SignType;
                apolloRequest.Sign = setting.GetSignMD5Encryption(apolloRequest.ToString());

                ICommand command = new GetAllServiceAmountLimit_CommandV2(apolloRequest, setting);
                invoker.AddCommand(command);

                ApolloReturnDataModel<GetAllServiceAmountLimit_ResponseV2> returnDataModel = invoker.ExecuteDetailCommand();
                var debugUserService = DependencyUtil.ResolveService<IDebugUserService>();
                debugUserService.ForcedDebug(EnvLoginUser.LoginUser.UserName, $"{nameof(ApolloGetAllServiceAmountLimitV2)} " +
                    $"request:{returnDataModel.RequestBody}  " +
                    $"response:{returnDataModel.ResponseContent}");

                return returnDataModel.DataModel;
            }, defaultResult);
        }

        public List<ApolloServiceTypeInfo> GetRechargeServiceTypeInfos()
        {
            return DoWorkWithErrorHandle(() =>
            {
                var userInfoRelatedReadService = ResolveJxBackendService<IUserInfoRelatedReadService>(DbConnectionTypes.Slave);
                UserLevel userLevel = userInfoRelatedReadService.GetUserLevel(EnvLoginUser.LoginUser.UserId);

                var jxCacheService = DependencyUtil.ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
                List<ApolloServiceTypeInfo> returnResult = jxCacheService.GetCache(new SearchCacheParam()
                {
                    Key = CacheKey.GetApolloServiceTypeInfos(userLevel.Level),
                    IsSlidingExpiration = false,
                    IsForceRefresh = false,
                    IsCloneInstance = false,
                    CacheSeconds = 60,
                },
                () =>
                {
                    var merchantSettingService = DependencyUtil.ResolveKeyed<IMerchantSettingService>(SharedAppSettings.PlatformMerchant);
                    Setting setting = merchantSettingService.GetApolloSetting(EnvLoginUser.Application);
                    AbstractInvoker<GetAllServiceAmountLimit_ResponseV2> invoker = new Invoker<GetAllServiceAmountLimit_ResponseV2>();

                    Apollo_Request apolloRequest = new GetAllServiceAmountLimit_Request
                    {
                        MerchantNo = setting.MerchantNO,
                        SignType = setting.SignType
                    };

                    apolloRequest.Sign = setting.GetSignMD5Encryption(apolloRequest.ToString());

                    ICommand command = new GetAllServiceAmountLimit_CommandV2(apolloRequest, setting);
                    invoker.AddCommand(command);

                    ApolloReturnDataModel<GetAllServiceAmountLimit_ResponseV2> returnDataModel = invoker.ExecuteDetailCommand();
                    var debugUserService = DependencyUtil.ResolveService<IDebugUserService>();
                    debugUserService.ForcedDebug(EnvLoginUser.LoginUser.UserName, $"{nameof(ApolloGetAllServiceAmountLimitV2)} " +
                        $"request:{returnDataModel.RequestBody}  " +
                        $"response:{returnDataModel.ResponseContent}");

                    var configService = ResolveJxBackendService<IConfigService>(DbConnectionTypes.Slave);
                    var apolloServiceTypeInfos = new List<ApolloServiceTypeInfo>();

                    if (returnDataModel.DataModel.Code != ApolloReturenCodes.Success.Code)
                    {
                        return apolloServiceTypeInfos;
                    }

                    HashSet<int> moneyInTypeItemKeys = configService.GetConfigSettingsList(ConfigGroupNameEnum.MoneyInType, isActive: true)
                        .Select(s => s.ItemKey)
                        .ToHashSet();

                    foreach (GetAllServiceAmountLimit_DetailV2 detailV2 in returnDataModel.DataModel.data)
                    {
                        ApolloServiceType apolloServiceType = ApolloServiceType.GetSingle(detailV2.serviceType);

                        if (apolloServiceType == null)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        if (!apolloServiceType.IsRechargeService)
                        {
                            continue;
                        }

                        //disabled by ConfigSetting
                        if (!moneyInTypeItemKeys.Contains(apolloServiceType.ConfigSettingItemKey))
                        {
                            continue;
                        }

                        GetAllServiceAmountLimit_DetailV2_GroupInfo groupInfo = detailV2.groupInfo.Where(w => w.groupCode == userLevel.Level.ToString()).Single();

                        if (groupInfo == null)
                        {
                            continue;
                        }

                        //設定最低充值金額
                        if (groupInfo.minAmount.HasValue && groupInfo.minAmount < _minRechargeAmount)
                        {
                            groupInfo.minAmount = _minRechargeAmount;
                        }

                        var apolloServiceTypeInfo = new ApolloServiceTypeInfo()
                        {
                            ServiceType = apolloServiceType.Value,
                            ServiceTypeName = apolloServiceType.Name,
                            IsInputAmount = apolloServiceType.IsInputAmount,
                            IsRedirectToOuterPage = apolloServiceType.IsRedirectToOuterPage,
                            IsUserBankCardRequired = apolloServiceType.IsUserBankCardRequired,
                            ConfigSettingItemKey = apolloServiceType.ConfigSettingItemKey,
                            GroupInfo = groupInfo,
                        };

                        apolloServiceTypeInfos.Add(apolloServiceTypeInfo);
                    }

                    return apolloServiceTypeInfos;
                });

                return returnResult;
            }, new List<ApolloServiceTypeInfo>());
        }
    }
}
