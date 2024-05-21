using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.ThirdPartyTransfer.Base
{
    public abstract class BaseThirdPartyApiWCFService : BaseApplicationService, IThirdPartyApiWCFService
    {
        private IIpUtilService _ipUtilService;

        private ITPGameTransferOutService _tpGameTransferOutService;

        private IPlatformProductService _platformProductService;

        private IUserInfoRelatedService _userInfoRelatedService;

        private IRechargeService _rechargeService;

        private IFrontsideMenuService _frontsideMenuService;

        protected BaseThirdPartyApiWCFService()
        {
            InitServices(EnvLoginUser, DbConnectionTypes.Slave);
        }

        protected BaseThirdPartyApiWCFService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            InitServices(envLoginUser, dbConnectionType);
        }

        public BaseReturnDataModel<UserScore> GetBalance(string productCode)
        {
            ITPGameApiService gameApiService = ResolveTPGameApiService(productCode, DbConnectionTypes.Slave);

            return gameApiService.GetRemoteUserScore(new InvocationUserParam() { UserID = EnvLoginUser.LoginUser.UserId }, isRetry: false);
        }

        /// <summary> 取得第三方遊戲網址 </summary>
        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(string productCode, string loginInfoJson, bool isMobile, string correlationId)
        {
            ITPGameApiService gameApiService = ResolveTPGameApiService(productCode, DbConnectionTypes.Master);

            BaseReturnDataModel<TPGameOpenParam> urlResult = gameApiService.GetForwardGameUrl(new ForwardGameUrlParam()
            {
                LoginUser = EnvLoginUser.LoginUser,
                IpAddress = _ipUtilService.GetIPAddress(),
                IsMobile = isMobile,
                LoginInfo = loginInfoJson.Deserialize<LoginInfo>()
            });

            if (urlResult.IsSuccess)
            {
                DoTPGameAutoTransfer(productCode, correlationId);
            }

            return urlResult;
        }

        public BaseReturnDataModel<string> GetLoginApiResult(string productCode, string loginInfoJson, bool isMobile)
        {
            ITPGameApiService gameApiService = ResolveTPGameApiService(productCode, DbConnectionTypes.Master);

            var apiResult = gameApiService.GetLoginApiResult(new ForwardGameUrlParam()
            {
                LoginUser = EnvLoginUser.LoginUser,
                IpAddress = _ipUtilService.GetIPAddress(),
                IsMobile = isMobile,
                LoginInfo = loginInfoJson.Deserialize<LoginInfo>()
            });

            return apiResult;
        }

        ///// <summary>
        ///// 用非同步的方式把工作轉到Queue，由Transfer各自做轉回,避免併發且後台也能支持此功能
        ///// </summary>
        //public BaseReturnModel TransferAllOUT()
        //{
        //    return _tpGameTransferOutService.AllAmountByAllProducts(EnvLoginUser.LoginUser, isOperateByBackSide: false);
        //}

        ///// <summary> 第三方餘額轉回主帳戶 </summary>
        //public void SelfGameDoTransferToPlatform()
        //{
        //    ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
        //    {
        //        int userId = EnvLoginUser.LoginUser.UserId;
        //        string productCode = PlatformProduct.Lottery.Value;

        //        BaseReturnModel transferOutResult = DoTPGameAutoTransferOut(productCode);

        //        if (transferOutResult != null && transferOutResult.IsSuccess)
        //        {
        //            // 更新最後進入遊戲紀錄
        //            UpdateLastAutoTransInfo(userId, productCode);
        //        }
        //    });
        //}

        /// <summary> 自動轉帳功能 </summary>
        public void DoTPGameAutoTransfer(string productCode, string correlationId)
        {
            ErrorMsgUtil.DoWorkWithErrorHandle(EnvLoginUser, () =>
            {
                int userId = EnvLoginUser.LoginUser.UserId;

                BaseReturnModel transferOutResult = DoTPGameAutoTransferOut(productCode, correlationId);

                if (transferOutResult.IsSuccess)
                {
                    // 清除最後進入遊戲紀錄
                    UpdateLastAutoTransInfo(userId, productCode: null);
                }

                //從秘色做轉入
                BaseReturnModel rechargeReturnModel = _rechargeService.RechargeAllFromMiseLive();

                if (!rechargeReturnModel.IsSuccess)
                {
                    ErrorMsgUtil.ErrorHandle(new Exception(rechargeReturnModel.Message), EnvLoginUser);
                }

                // 取得用戶最新主帳戶餘額
                decimal availableScores = GetUserAvailableScores(userId);

                if (availableScores < GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount)
                {
                    return;
                }

                decimal transferInAmount = FormatTPGameAutoTransferInAmount(availableScores);

                // 轉到新第三方遊戲
                BaseReturnModel returnModel = DoTransferIN(productCode, transferInAmount, isSynchronizing: true, correlationId);

                if (returnModel.IsSuccess)
                {
                    // 更新最後進入遊戲紀錄
                    UpdateLastAutoTransInfo(userId, productCode);
                }
            });
        }

        public FrontsideMenu GetActiveFrontsideMenu(string productCode, string gameCode)
        {
            List<FrontsideMenu> frontsideMenus = _frontsideMenuService.GetActiveFrontsideMenus();

            return frontsideMenus.Where(w => w.ProductCode == productCode && w.GameCode == gameCode
                && w.RemoteCode.IsNullOrEmpty() // 目前為第三方獨立入口使用，排除熱門遊戲，因此只查RemoteCode為空的資料
                ).Single();
        }

        /// <summary> 轉回最後登入第三方的餘額回主帳 </summary>
        private BaseReturnModel DoTPGameAutoTransferOut(string productCode, string correlationId)
        {
            int userId = EnvLoginUser.LoginUser.UserId;
            var userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedReadService>(DbConnectionTypes.Master);
            UserInfoAdditional userInfoAdditional = userInfoRelatedService.GetUserInfoAdditional(userId);

            // 沒設定過資料，不做後續處理
            if (userInfoAdditional == null ||
                userInfoAdditional.UserTransferSettingJson == null)
            {
                userInfoAdditional = new UserInfoAdditional();
                userInfoAdditional.SetUserTransferSetting(new UserTransferSetting());
            }

            // 最後自動轉帳ProductCode
            string lastAutoTransferProductCode = userInfoAdditional.GetUserTransferSetting().LastAutoTransProductCode.ToTrimString();

            // 進入的遊戲與前一遊戲系列不相同，舊遊戲先做轉出再做新遊戲轉入
            if (!lastAutoTransferProductCode.IsNullOrEmpty() && lastAutoTransferProductCode != productCode)
            {
                PlatformProduct lastProduct = _platformProductService.GetSingle(lastAutoTransferProductCode);

                // 非自營遊戲要進行第三方遊戲轉回
                if (!lastProduct.IsSelfProduct)
                {
                    IInvocationParam invocationParam = new InvocationUserParam { CorrelationId = correlationId };

                    return _tpGameTransferOutService.AllAmountBySingleProduct(lastProduct, isSynchronizing: true, invocationParam);
                }
            }

            return new BaseReturnModel(ReturnCode.NoDataChanged);
        }

        private BaseReturnModel DoTransferIN(string productCode, decimal amount, bool isSynchronizing, string correlationId)
        {
            ITPGameApiService gameApiService = ResolveTPGameApiService(productCode, DbConnectionTypes.Master);

            return gameApiService.CreateTransferInInfo(new TPGameTranfserParam()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                CorrelationId = correlationId,
                Amount = amount.Floor(0), // 轉入整數
                IsSynchronizing = isSynchronizing
            });
        }

        private decimal FormatTPGameAutoTransferInAmount(decimal amount)
        {
            decimal maxTPGameTransferAmount = GlobalVariables.TPTransferAmountBound.MaxTPGameTransferAmount;

            if (amount > maxTPGameTransferAmount)
            {
                amount = maxTPGameTransferAmount;
            }

            return Math.Floor(amount);
        }

        private ITPGameApiService ResolveTPGameApiService(string productCode, DbConnectionTypes dbConnectionType)
        {
            PlatformProduct product = PlatformProduct.GetSingle(productCode);

            return ResolveJxBackendService<ITPGameApiService>(product, dbConnectionType);
        }

        private BaseReturnModel UpdateLastAutoTransInfo(int userId, string productCode)
        {
            var userInfoRelatedService = ResolveJxBackendService<IUserInfoRelatedService>(DbConnectionTypes.Master);
            return userInfoRelatedService.UpdateLastAutoTransInfo(userId, productCode);
        }

        private decimal GetUserAvailableScores(int userId)
        {
            return _userInfoRelatedService.GetUserAvailableScores(userId);
        }

        /// <summary>
        /// 為了可以從外部指定envLoginUser所以把初始化獨立出來
        /// </summary>
        private void InitServices(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
            _tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(envLoginUser, DbConnectionTypes.Master);
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(envLoginUser.Application, Merchant);
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(envLoginUser, DbConnectionTypes.Master);
            _rechargeService = DependencyUtil.ResolveJxBackendService<IRechargeService>(envLoginUser, DbConnectionTypes.Master);
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(envLoginUser, DbConnectionTypes.Slave);
        }
    }
}