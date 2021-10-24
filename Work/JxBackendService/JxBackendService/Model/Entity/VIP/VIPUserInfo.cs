using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.VIP
{
    public class VIPUserInfo : BaseEntityModel
    {
        [Key]
        public int UserID { get; set; }

        public int CurrentLevel { get; set; }

        public DateTime LevelSettleDate { get; set; }        
    }
}