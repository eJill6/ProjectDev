using JxBackendService.Common.Util;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.User
{
    public class UserInfoAdditional : BaseEntityModel
    {
        [ExplicitKey]
        public int UserID { get; set; }

        /// <summary> 用戶錢包轉出轉入設定紀錄 </summary>
        public string UserTransferSettingJson { get; set; }

        public UserTransferSetting GetUserTransferSetting() => UserTransferSettingJson.Deserialize<UserTransferSetting>();

        public void SetUserTransferSetting(UserTransferSetting userTransferSetting) => UserTransferSettingJson = userTransferSetting.ToJsonString();
    }

    public class UserTransferSetting
    {
        /// <summary> 最後自動遊戲轉入轉出的產品代號 </summary>
        public string LastAutoTransProductCode { get; set; }
    }
}