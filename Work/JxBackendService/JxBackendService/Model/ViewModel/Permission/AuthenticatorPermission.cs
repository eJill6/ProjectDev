using JxBackendService.Model.ReturnModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Permission
{
    public class BaseAuthenticatorPermission : BaseBehaviorModel
    {
        /// <summary>
        /// 是否允許執行該功能
        /// </summary>
        public bool IsAllowExecuted { get; set; }

        /// <summary>
        /// 是否需要驗證Authenticator
        /// </summary>
        public bool IsAuthenticatorRequired { get; set; }

        /// <summary>
        /// 是否需要顯示綁定驗證彈窗
        /// </summary>
        public bool IsShowAuthenticatorConfirm { get; set; }

        /// <summary>
        /// 回傳的錯誤訊息
        /// </summary>
        [JsonIgnore]
        public string ErrorMessage
        {
            get
            {
                if (ClientBehavior == null || ClientBehavior.DialogSetting == null)
                {
                    return null;

                }

                return ClientBehavior.DialogSetting.Message;
            }
        }
    }

    public class AuthenticatorPermission : BaseAuthenticatorPermission
    {
        [JsonIgnore]
        public UserAuthenticatorInfo UserAuthenticatorInfo { get; set; }
    }
}
