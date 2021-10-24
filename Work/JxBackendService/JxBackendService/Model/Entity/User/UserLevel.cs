using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Entity
{
    public class UserLevel
    {
        [ExplicitKey]
        public int UserID { get; set; }
        
        public int Level { get; set; }
        
        public DateTime? PromotionDate { get; set; }
    }
}
