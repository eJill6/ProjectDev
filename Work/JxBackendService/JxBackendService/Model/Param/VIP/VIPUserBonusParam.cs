using System;
using JxBackendService.Model.Enums;

namespace JxBackendService.Model.Param.VIP
{
    public class VIPUserBonusQueryParam
    {
        public string Username { get; set; }

        public int? UserID { get; set; }

        public int? BonusType { get; set; }

        public int? VIPLevel { get; set; }

        public int? ReceivedStatus { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}