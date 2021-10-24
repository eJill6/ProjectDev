using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.User
{
    public class ProSaveUserTransferChildStatusParam
    {
        public int UserID { get; set; }
        
        public string LoginUserName { get; set; }
        
        public bool IsLowMoneyIn { get; set; }
        
        public bool? IsAllowSetTransferByParent { get; set; }
        
        public string RT_Success { get; set; }
        
        public string RT_UpdateFail { get; set; }
    }
}