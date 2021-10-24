using System;
using System.ComponentModel.DataAnnotations;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity.VIP
{
    public class VIPAgentScore : BaseEntityModel
    {
        [Key]
        public int UserID { get; set; }

        public decimal AvailableScores { get; set; }

        public decimal FreezeScores { get; set; }        
    }
}