using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 00開頭只有成功會使用，ReturnCode：000000
    /// E0開頭(E0xxxx) 表單純用彈跳視窗顯示錯誤訊息不做任何動作
    /// E1開頭(E1xxxx) 表單純用彈跳視窗顯示錯誤訊息且返回上一頁
    /// E2開頭(E2xxxx) 表用彈跳視窗顯示錯誤訊息後返回登入頁
    /// </summary>
    public class ReturnCodeActionType : BaseStringValueModel<ReturnCodeActionType>
    {
        private ReturnCodeActionType() { }
        public static ReturnCodeActionType Success = new ReturnCodeActionType() { Value = "00" };
        public static ReturnCodeActionType ShowErrorMessageAndStayOnCurrentPage = new ReturnCodeActionType() { Value = "E0" };
        public static ReturnCodeActionType AppTeamCustomizedAction = new ReturnCodeActionType() { Value = "E1" };
        public static ReturnCodeActionType JustBackToLoginPage = new ReturnCodeActionType() { Value = "E2" };
        public static ReturnCodeActionType ShowErrorMessageAndBackToLoginPage = new ReturnCodeActionType() { Value = "E3" };

    }
}
