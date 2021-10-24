using JxBackendService.Common.Util;
using JxBackendService.Common.Util.Cache;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Cache;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.Security
{
    public class ValidateCodeService : BaseService, IValidateCodeService
    {
        private readonly IJxCacheService _jxCacheService;

        public ValidateCodeService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _jxCacheService = ResolveServiceForModel<IJxCacheService>(EnvLoginUser.Application);
        }

        /// <summary>
        /// 產生圖形驗證碼
        /// </summary>
        /// <returns></returns>
        public BaseReturnDataModel<string> GetGraphicValidateCodeImage(WebActionType webActionType, string userIdentityKey, bool isForceRefresh = false)
        {
            if (webActionType == null)
            {
                return new BaseReturnDataModel<string>(ReturnCode.ParameterIsInvalid);
            }

            //是登入狀態的Action，使用 LoginUser 的 UserKey當作用戶主識別
            if (webActionType.IsLogined)
            {
                userIdentityKey = EnvLoginUser.LoginUser.UserKey;
            }

            //取得驗證碼答案
            CacheKey intervalCheckCacheKey = CacheKey.GraphicValidateCodeIntervalCheck(webActionType.Value, userIdentityKey);

            //避免取得驗證碼被攻擊，導致CacheServer壞掉，使用 DoIntervalWork 避免時間內被不停重新取得
            //60內如果重複取10次，暫停功能300秒
            var intervalJobParam = new IntervalJobParam()
            {
                CacheKey = intervalCheckCacheKey,
                CacheSeconds = 60,
                MaxNormalTryCount = 10,
                SuspendSeconds = 300,
                EnvironmentUser = EnvLoginUser
            };

            string validateCode = DoIntervalWork(intervalJobParam, () =>
            {
                CacheKey validateCodeCacheKey = CacheKey.GraphicValidateCode(webActionType.Value, userIdentityKey);

                return _jxCacheService.GetCache(new SearchCacheParam()
                {
                    Key = validateCodeCacheKey,
                    IsSlidingExpiration = false,
                    CacheSeconds = webActionType.CacheSeconds,
                    IsForceRefresh = isForceRefresh
                }, () =>
                {
                    return ValidateCodeUtil.CreateRandomCode(GlobalVariables.ValidateCodeImageNumberCount);
                });
            });

            //拿失敗就代表取太多次直接Return出去
            if (validateCode.IsNullOrEmpty())
            {
                return new BaseReturnDataModel<string>(ReturnCode.TryTooOften);
            }

            //開始處理圖形
            int iPixelCalculator = Convert.ToInt32(
                             ((GlobalVariables.ValidateCodeImageWidth / GlobalVariables.ValidateCodeImageHeight)
                             * GlobalVariables.ValidateCodeImageWidth));

            var bitmap = ValidateCodeUtil.CreateImage(validateCode, GlobalVariables.ValidateCodeImageWidth,
                                                      GlobalVariables.ValidateCodeImageHeight, iPixelCalculator);

            // Convert the image to byte[]
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imageBytes = stream.ToArray();

            var imageResult = Convert.ToBase64String(imageBytes);

            return new BaseReturnDataModel<string>(ReturnCode.Success, imageResult);
        }

        /// <summary>
        /// 驗證圖形驗證碼
        /// </summary>
        /// <returns></returns>
        public BaseReturnModel CheckGraphicValidateCode(WebActionType webActionType, string userIdentityKey, string validateCode)
        {
            if (webActionType == null)
            {
                return new BaseReturnModel(ReturnCode.ParameterIsInvalid);
            }

            //是登入狀態的Action，使用 LoginUser 的 UserKey當作用戶主識別
            //規則與產生時一樣
            if (webActionType.IsLogined)
            {
                userIdentityKey = EnvLoginUser.LoginUser.UserKey;
            }

            CacheKey cacheKey = CacheKey.GraphicValidateCode(webActionType.Value, userIdentityKey);

            string cacheValidateCode = _jxCacheService.GetCache<string>(cacheKey);

            if (cacheValidateCode == null)
            {
                return new BaseReturnModel(ReturnCode.ValidateCodeIsExpired);
            }
            else if (!validateCode.ToUpper().Equals(cacheValidateCode.ToUpper()))
            {
                return new BaseReturnModel(ReturnCode.ValidateCodeIncorrect);
            }

            //用過成功的Key要Remove掉
            _jxCacheService.RemoveCache(cacheKey);

            return new BaseReturnModel(ReturnCode.Success);
        }
    }
}
