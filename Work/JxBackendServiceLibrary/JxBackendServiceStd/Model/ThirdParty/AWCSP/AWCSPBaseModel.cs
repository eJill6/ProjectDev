using JxBackendService.Model.Common;

namespace JxBackendService.Model.ThirdParty.AWCSP
{
    public class AWCSPBaseRequestModel
    {
        /// <summary>
        /// 认证码
        /// </summary>
        public string Cert => AWCSPSharedAppSetting.Cert;

        /// <summary>
        /// 代理 ID 作为身分确认用
        /// </summary>
        public string AgentId => AWCSPSharedAppSetting.AgentId;
    }

    public class AWCSPBaseResponseModel
    {
        public bool IsSuccess => Status == AWCSPResponseCode.Success;

        /// <summary>
        /// 状态代码
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///  错误讯息
        /// </summary>
        public string Desc { get; set; }
    }
}