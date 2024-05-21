using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Model.Common;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Finance;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.Enums;
using JxBackendService.Model.GlobalSystem;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using RestSharp;
using SLPolyGame.Web.Interface;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiImpl
{
    public abstract class BaseThirdPartyApiService : BaseWebApiService, IThirdPartyApiWebSVService
    {
        private readonly Lazy<IIpUtilService> _ipUtilService;

        private readonly Lazy<ITPGameTransferOutService> _tpGameTransferOutService;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private readonly Lazy<IUserInfoRelatedService> _userInfoRelatedService;

        private readonly Lazy<IRechargeService> _rechargeService;

        private readonly Lazy<IFrontsideMenuService> _frontsideMenuService;

        public BaseThirdPartyApiService()
        {
            _ipUtilService = DependencyUtil.ResolveService<IIpUtilService>();
            _tpGameTransferOutService = DependencyUtil.ResolveJxBackendService<ITPGameTransferOutService>(EnvLoginUser, DbConnectionTypes.Master);
            _platformProductService = DependencyUtil.ResolveKeyed<IPlatformProductService>(EnvLoginUser.Application, SharedAppSettings.PlatformMerchant);
            _userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Master);
            _rechargeService = DependencyUtil.ResolveJxBackendService<IRechargeService>(EnvLoginUser, DbConnectionTypes.Master);
            _frontsideMenuService = DependencyUtil.ResolveJxBackendService<IFrontsideMenuService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        /// <summary> 取得第三方遊戲網址 </summary>
        public BaseReturnDataModel<TPGameOpenParam> GetForwardGameUrl(ForwardGameUrlSVApiParam param)
        {
            ITPGameApiService gameApiService = ResolveTPGameApiService(param.ProductCode, DbConnectionTypes.Master);

            BaseReturnDataModel<TPGameOpenParam> urlResult = gameApiService.GetForwardGameUrl(new ForwardGameUrlParam()
            {
                LoginUser = EnvLoginUser.LoginUser,
                IpAddress = _ipUtilService.Value.GetIPAddress(),
                IsMobile = param.IsMobile,
                LoginInfo = param.LoginInfoJson.Deserialize<LoginInfo>(),
                CorrelationId = param.CorrelationId
            });

            if (urlResult.IsSuccess)
            {
                DoTPGameAutoTransfer(param.ProductCode, param.CorrelationId);
            }

            return urlResult;
        }

        /// <summary> 自動轉帳功能 </summary>
        private void DoTPGameAutoTransfer(string productCode, string correlationId)
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

                PlatformProduct product = _platformProductService.Value.GetSingle(productCode);

                if (product.IsSelfProduct)
                {
                    return;
                }

                //從秘色做轉入
                BaseReturnModel rechargeReturnModel = _rechargeService.Value.RechargeAllFromMiseLive(productCode, correlationId);

                // 取得用戶最新主帳戶餘額
                decimal availableScores = GetUserAvailableScores(userId);

                if (availableScores < GlobalVariables.TPTransferAmountBound.MinTPGameTransferAmount)
                {
                    return;
                }

                decimal transferInAmount = FormatTPGameAutoTransferInAmount(availableScores);

                // 轉到新第三方遊戲
                BaseReturnModel returnModel = DoTransferIN(productCode, transferInAmount, correlationId);

                if (returnModel.IsSuccess)
                {
                    // 更新最後進入遊戲紀錄
                    UpdateLastAutoTransInfo(userId, productCode);
                }
            });
        }

        public FrontsideMenu GetActiveFrontsideMenu(string productCode, string gameCode)
        {
            List<FrontsideMenu> frontsideMenus = _frontsideMenuService.Value.GetActiveFrontsideMenus();

            return frontsideMenus.Where(w => w.ProductCode == productCode && w.GameCode.ToNonNullString() == gameCode.ToNonNullString()
                && w.RemoteCode.IsNullOrEmpty() // 目前為第三方獨立入口使用，排除熱門遊戲，因此只查RemoteCode為空的資料
                ).Single();
        }

        /// <summary> 轉回最後登入第三方的餘額回主帳 </summary>
        private BaseReturnModel DoTPGameAutoTransferOut(string productCode, string correlationId)
        {
            int userId = EnvLoginUser.LoginUser.UserId;
            var userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedReadService>(EnvLoginUser, DbConnectionTypes.Master).Value;
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
                PlatformProduct lastProduct = _platformProductService.Value.GetSingle(lastAutoTransferProductCode);

                // 非自營遊戲要進行第三方遊戲轉回
                if (!lastProduct.IsSelfProduct)
                {
                    IInvocationUserParam invocationUserParam = new InvocationUserParam
                    {
                        UserID = userId,
                        CorrelationId = correlationId
                    };

                    return _tpGameTransferOutService.Value.AllAmountBySingleProduct(lastProduct, invocationUserParam);
                }
            }

            return new BaseReturnModel(ReturnCode.NoDataChanged);
        }

        private BaseReturnModel DoTransferIN(string productCode, decimal amount, string correlationId)
        {
            ITPGameApiService gameApiService = ResolveTPGameApiService(productCode, DbConnectionTypes.Master);

            return gameApiService.CreateTransferInInfo(new TPGameTranfserParam()
            {
                UserID = EnvLoginUser.LoginUser.UserId,
                CorrelationId = correlationId,
                Amount = amount.Floor(0), // 轉入整數
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

            return DependencyUtil.ResolveJxBackendService<ITPGameApiService>(
                product,
                SharedAppSettings.PlatformMerchant,
                EnvLoginUser,
                dbConnectionType).Value;
        }

        private BaseReturnModel UpdateLastAutoTransInfo(int userId, string productCode)
        {
            var userInfoRelatedService = DependencyUtil.ResolveJxBackendService<IUserInfoRelatedService>(EnvLoginUser, DbConnectionTypes.Master).Value;

            return userInfoRelatedService.UpdateLastAutoTransInfo(userId, productCode);
        }

        private decimal GetUserAvailableScores(int userId)
        {
            return _userInfoRelatedService.Value.GetUserAvailableScores(userId);
        }

        public string GetTPGameLaunchURLHTML(string token)
        {
            CacheKey cacheKey = CacheKey.TPGameLaunchURLHTML(token);

            return JxCacheService.GetCache<string>(cacheKey);
        }
    }
}