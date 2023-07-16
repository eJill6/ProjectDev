using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.User;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.User
{
    /// <summary>
    /// 後來才開設的ReadService, 原先用到的讀取method有機會在搬
    /// </summary>
    public interface IUserInfoRelatedReadService
    {
        UserInfo GetUserInfo(int userId);

        UserInfoAdditional GetUserInfoAdditional(int userId);

        /// <summary>取得有閒置資金的用戶</summary>
        List<UserInfo> GetIdleScoreUsers();
    }

    public interface IUserInfoRelatedService
    {
        BaseReturnModel UpdateLastAutoTransInfo(int userId, string productCode);

        /// <summary>取得餘額,必要時需使用Master連線取得最新餘額</summary>
        decimal GetUserAvailableScores(int userId);
    }
}