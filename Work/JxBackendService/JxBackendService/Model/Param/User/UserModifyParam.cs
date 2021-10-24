using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.User
{
    public class UserVaildBankParam
    {
        public int UserID { get; set; }
        public int BankTypeID { get; set; }
        public string CardUser { get; set; }
        public string BankCard { get; set; }
        public ModifyUserDataTypes ModifyUserDataType { get; set; }
    }

    public class UserModifyDataParam : UserVaildBankParam
    {
        public string BeforeContent { get; set; }
        public string ModifyContent { get; set; }
        public string Memo { get; set; }
        public bool IsClearMail { get; set; }
    }

    public class SpUpdateUserInfoDataParam
    {
        public int UserID { get; set; }
        public int ModifyUserDataType { get; set; }
        public string EncryptContent { get; set; }
        public string RC_Success => ReturnCode.Success.Value;
        public string RC_DataIsExist => ReturnCode.DataIsExist.Value;
        public string RC_DataIsNotCompleted => ReturnCode.DataIsNotCompleted.Value;
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;
        public string RC_UserInitializeIncomplete => ReturnCode.UserInitializeIncomplete.Value;
        public string RC_SystemError => ReturnCode.SystemError.Value;
    }

    public class SpSaveUserSecurityInfoParam
    {
        public int UserID { get; set; }
        public string MoneyPasswordHash { get; set; }
        public string EmailEncrypt { get; set; }
        public int FirstQuestionId { get; set; }
        public string FirstAnswer { get; set; }
        public int SecondQuestionId { get; set; }
        public string SecondAnswer { get; set; }
        public string RC_Success => ReturnCode.Success.Value;
        public string RC_UserNotFound => ReturnCode.UserNotFound.Value;
        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;
        public string RC_UserIsFrozen => ReturnCode.UserIsFrozen.Value;
        public string RC_SamePassword => ReturnCode.SamePassword.Value;
        public string RC_InitHasFinished => ReturnCode.InitHasFinished.Value;
    }
}
