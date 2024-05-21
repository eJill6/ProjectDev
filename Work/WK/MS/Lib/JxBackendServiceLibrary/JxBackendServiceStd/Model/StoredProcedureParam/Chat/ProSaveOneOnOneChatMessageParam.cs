using JxBackendService.Model.Entity.Chat;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.StoredProcedureParam.Chat
{
    public class ProSaveOneOnOneChatMessageParam : MSIMOneOnOneChatMessage
    {
        public string RC_Success => ReturnCode.Success.Value;

        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;

        public string RC_SystemError => ReturnCode.SystemError.Value;
    }
}