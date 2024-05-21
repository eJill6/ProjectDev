using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.Chat;

namespace JxBackendService.Model.StoredProcedureParam.Chat
{
    public class ProClearUnreadCountParam : OwnerRoomParam
    {
        public string RC_Success => ReturnCode.Success.Value;

        public string RC_UpdateFailed => ReturnCode.UpdateFailed.Value;
    }
}