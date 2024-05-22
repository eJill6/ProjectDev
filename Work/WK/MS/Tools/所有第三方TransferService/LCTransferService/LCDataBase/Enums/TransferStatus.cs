using System.ComponentModel;

namespace LCDataBase.Enums
{
    /// <summary>
    /// 轉帳狀態
    /// </summary>
    public enum TransferStatus
    {
        [Description("不存在")]
        NotExist = -1,

        [Description("成功")]
        Success = 0,

        [Description("失败")]
        Fail = 2,

        [Description("处理中")]
        Processing = 3,

        [Description("系统预设-资料解析失败")]
        SysDefault = -9999
    }
}
