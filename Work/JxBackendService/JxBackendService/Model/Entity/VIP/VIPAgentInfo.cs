using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.VIP
{
    public class VIPAgentInfo : BaseEntityModel
    {
        [Key]
        public int UserID { get; set; }

        public string UserPwd { get; set; }    
    }
}