using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Repository.User;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.User;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.User;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using JxBackendServiceNet45.Interface.Service.Authenticator;
using JxBackendServiceNet45.Interface.Service.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JxBackendServiceNet45.Service.Finance
{
    public class UserFinanceService : BaseService, IUserFinanceService, IUserFinanceReadService
    {
        private readonly IUserAuthenticatorValidReadService _userAuthenticatorValidReadService;
        private readonly IUserInfoRep _userInfoRep;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IBlockChainInfoRep _blockChainInfoRep;
        private readonly IBankTypeRep _bankTypeRep;
        private readonly IFailureOperationService _failureOperationService;

        public UserFinanceService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _userAuthenticatorValidReadService = ResolveJxBackendService<IUserAuthenticatorValidReadService>();
            _userInfoRep = ResolveJxBackendService<IUserInfoRep>();
            _messageQueueService = DependencyUtil.ResolveServiceForModel<IMessageQueueService>(envLoginUser.Application);
            _blockChainInfoRep = ResolveJxBackendService<IBlockChainInfoRep>();
            _bankTypeRep = ResolveJxBackendService<IBankTypeRep>();
            _failureOperationService = ResolveJxBackendService<IFailureOperationService>();
        }

        public BaseReturnModel TransferToChild(TransferToChildParam param)
        {
            BaseReturnModel returnModel = _userAuthenticatorValidReadService
                .IsUserTwoFactorPinValid(param.LoginUser.UserId, UserAuthenticatorSettingTypes.TransferToChild, param.ClientPin);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }

            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = param.LoginUser.UserId });

            //先把null排除避免後面一直在判斷
            if (userInfo == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            if (!userInfo.IsActive)
            {
                return new BaseReturnModel(ReturnCode.YourAccountIsDisabled);
            }

            BaseReturnDataModel<int> returnDataModel = _userInfoRep.LowMoneyIn(param);

            if (returnDataModel.IsSuccess)
            {
                _failureOperationService.ClearCount(WebActionType.TransferToChild, SubActionType.MoneyPasswordWrong);

                ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
                {
                    //send mq;                    
                    _messageQueueService.SendTransferToChildMessage(returnDataModel.DataModel, param.TransferAmount);
                });

                return returnModel;
            }
            else if (returnDataModel.Message == ReturnCode.YourMoneyPasswordIsWrong)
            {
                _failureOperationService.AddFailOperation(WebActionType.TransferToChild, SubActionType.MoneyPasswordWrong);
                return new BaseReturnModel(SubActionType.MoneyPasswordWrong.DefaultErrorMessage);
            }
            else
            {
                return new BaseReturnModel(returnDataModel.Message);
            }
        }

        public BaseReturnModel UpdateUSDTWallet(BlockChainInfoParam param)
        {
            //檢查字首
            if (param.WalletAddr.Substring(0, 1) == "t")
            {
                param.WalletAddr = "T" + param.WalletAddr.Substring(1);
            }

            if (!Regex.IsMatch(param.WalletAddr, RegularExpressionType.UsdtNumber.Pattern))
            {
                return new BaseReturnModel(MessageElement.UsdtAddressWrong);
            }

            BaseReturnModel returnModel = _userAuthenticatorValidReadService
                 .IsUserTwoFactorPinValid(param.LoginUser.UserId, UserAuthenticatorSettingTypes.ModifyUSDTAccount, param.ClientPin);

            if (!returnModel.IsSuccess)
            {
                return returnModel;
            }
            
            //資金密碼比對
            UserInfo userInfo = _userInfoRep.GetSingleByKey(InlodbType.Inlodb, new UserInfo() { UserID = param.LoginUser.UserId });

            //先把null排除避免後面一直在判斷
            if (userInfo == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            if (!userInfo.IsActive)
            {
                return new BaseReturnModel(ReturnCode.YourAccountIsDisabled);
            }

            if (userInfo.MoneyPwd != param.MoneyPasswordHash)
            {
                _failureOperationService.AddFailOperation(WebActionType.BindUSDTAccount, SubActionType.MoneyPasswordWrong);
                return new BaseReturnModel(SubActionType.MoneyPasswordWrong.DefaultErrorMessage);
            }

            returnModel = _userInfoRep.UpdateBlockChainInfo(param);

            if (returnModel.IsSuccess)
            {
                _failureOperationService.ClearCount(WebActionType.BindUSDTAccount, SubActionType.MoneyPasswordWrong);
            }

            return returnModel;
        }

        public List<ProSelectBankResult> GetUserAllBankCard(int userId)
        {
            return _userInfoRep.GetUserAllBankCard(userId);
        }

        public bool HasUserActiveUsdtAccount()
        {
            return _blockChainInfoRep.HasActiveUsdtAccount(EnvLoginUser.LoginUser.UserId);
        }

        public List<JxBackendSelectListItem> GetBankTypeListItems(int? moneyInType)
        {
            return _bankTypeRep.GetVisibleList()
                .Where(w => (!moneyInType.HasValue) || (moneyInType.HasValue && w.MoneyInType == moneyInType))
                .Select(s => new JxBackendSelectListItem(s.BankTypeID.ToString(), s.BankTypeName)).ToList();
        }

        //private string ProcessFailureLoginHistory(string operationName, BaseBasicUserInfo userInfo, string ipAddress)
        //{
        //    int failureTimes = _failureLoginHistoryRep.GetFailureLoginTimes(userInfo.UserName);

        //    string errorMsg = MessageElement.OperationFail;

        //    if (failureTimes + 1 > GlobalVariables.MaxPasswordAttempt)
        //    {
        //        if (_userInfoRep.UpdateUserActive(userInfo.UserId, false))
        //        {
        //            errorMsg = MessageElement.MoneyPasswordIsWrongTooManyTime;
        //            LogUtil.ForcedDebug($"用户 {userInfo.UserName} 於 {DateTime.Now.ToFormatDateString()} 執行 {operationName} 连续错误输入提现密码次数过多，被冻结");
        //            _failureLoginHistoryRep.DeleteFailureLoginHistory(userInfo.UserName);
        //        }
        //    }
        //    else
        //    {
        //        _failureLoginHistoryRep.CreateByProcedure(new FailureLoginHistory()
        //        {
        //            UserName = userInfo.UserName,
        //            LoginIp = ipAddress,
        //            LoginTime = DateTime.Now,
        //        });

        //        errorMsg = MessageElement.MoneyPasswordIsWrongWarning;
        //    }

        //    return errorMsg;
        //}

    }
}
