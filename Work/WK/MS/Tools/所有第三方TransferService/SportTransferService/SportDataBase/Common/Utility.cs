using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JxBackendService.Common.Util;
using SportDataBase.Model;

namespace SportDataBase.Common
{
    public static class Utility
    {
        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <returns></returns>
        public static ApiResult<UserBalanceItem> GetBalance(string userId)
        {
            var apiClient = new ApiClient();
            ApiResult<List<UserBalanceItem>> checkBalanceResult = apiClient.CheckUserBalance(userId);

            if (checkBalanceResult == null)
            {
                return null;
            }

            var result = new ApiResult<UserBalanceItem>
            {
                error_code = checkBalanceResult.error_code,
                message = checkBalanceResult.message
            };

            if (result.error_code != 0)
            {
                LogsManager.Error("用户" + userId + "在体育平台获取余额失败，错误代码：" + checkBalanceResult.error_code.ToString()
                    + "，错误信息：" + checkBalanceResult.message);
                return result;
            }

            if (checkBalanceResult.Data != null && checkBalanceResult.Data.Count > 0)
            {
                result.Data = checkBalanceResult.Data[0];
                return result;
            }

            LogsManager.ForcedDebug($"用户 {userId} 在体育平台获取余额失败，checkBalanceResult：{checkBalanceResult.ToJsonString()}");
            return null;
        }
    }
}
