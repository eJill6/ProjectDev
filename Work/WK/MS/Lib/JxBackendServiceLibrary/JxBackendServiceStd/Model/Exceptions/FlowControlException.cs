using JxBackendService.Model.Attributes;
using System;

namespace JxBackendService.Model.Exceptions
{
    /// <summary>
    /// 用於流程控管的場景例外處理, 
    /// 通常為原程式只有單純回傳資料而無法夾帶其他錯誤資訊, 
    /// 利用拋例外的方式作流程控管, 
    /// 所以不會發出錯誤通知與記錄log
    /// </summary>
    [IgnoreErrorHandle]
    public class FlowControlException : Exception
    {
        public FlowControlException() { }

        public FlowControlException(string message) : base(message)
        {

        }
    }
}
