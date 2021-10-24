using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Security;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.Security
{
    public class BlockActionService : BaseService, IBlockActionService
    {
        private readonly IJxCacheService _jxCacheService;
        private readonly Lazy<IBlackLocationService> _blackLocationService;

        public BlockActionService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
            _blackLocationService = new Lazy<IBlackLocationService>(() => ResolveJxBackendService<IBlackLocationService>());
        }

        public bool BlockWebActionClient(string ipAddress, int webActionTypeValue)
        {
            WebActionType webActionType = WebActionType.GetSingle(webActionTypeValue);
            CacheKey cacheKey = CacheKey.IpValidation(ipAddress, webActionTypeValue);

            InvalidCountViewModel invalidCountViewModel = _jxCacheService.GetCache(new SearchCacheParam()
            {
                Key = cacheKey,
                CacheSeconds = webActionType.CacheSeconds,
                IsForceRefresh = false,
                IsCloneInstance = false,
                IsSlidingExpiration = false,
            },
            () =>
            {
                return new InvalidCountViewModel()
                {
                    CreateDate = DateTime.Now,
                    Count = 0
                };
            });

            //過期就刪除,重新計算
            if (DateTime.Now.Subtract(invalidCountViewModel.CreateDate).TotalSeconds > webActionType.CacheSeconds)
            {
                _jxCacheService.RemoveCache(cacheKey);

                invalidCountViewModel = new InvalidCountViewModel()
                {
                    CreateDate = DateTime.Now,
                    Count = 0
                };
            }

            invalidCountViewModel.Count++;

            _jxCacheService.SetCache(new SetCacheParam()
            {
                Key = cacheKey,
                IsSlidingExpiration = false,
                CacheSeconds = webActionType.CacheSeconds,
            },
            invalidCountViewModel);

            if (invalidCountViewModel.Count >= webActionType.LimitCount)
            {
                string remark = GetRemark(webActionType);
                string userName = string.Empty;

                if (EnvLoginUser.LoginUser.UserId != 0)
                {
                    userName = EnvLoginUser.LoginUser.UserName;
                }

                bool isSuccess = _blackLocationService.Value.CreateBlackIp(ipAddress, BlackIpType.Login, remark, userName);
                return isSuccess;
            }

            return false;
        }

        private string GetRemark(WebActionType webActionType)
        {
            string remark = string.Empty;

            if (webActionType == WebActionType.ModifyLoginPassword || webActionType == WebActionType.ModifyMoneyPassword)
            {
                remark = string.Format(SecurityElement.BlockModifiedLockRemark,
                    EnvLoginUser.LoginUser.UserName,
                    webActionType.CacheSeconds / 60,
                    webActionType.Name,
                    webActionType.LimitCount,
                    DateTime.Now.ToFormatDateTimeString());
            }
            else
            {
                remark = string.Format(SecurityElement.BlockLoginIpRemark,
                     webActionType.CacheSeconds / 60,
                     webActionType.Name,
                     webActionType.LimitCount,
                     DateTime.Now.ToFormatDateTimeString());
            }            

            return remark;
        }
    }
}
