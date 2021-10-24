using JxBackendService.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Entity
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public bool? IsOpen { get; set; }
        public DateTime? RegTime { get; set; }
        public int? ParentID { get; set; }
        public bool? IsOnline { get; set; }
        public bool IsActive { get; set; }
        public int? RoleID { get; set; }
        public decimal? AvailableScores { get; set; }
        public string RoleName { get; set; }
        public decimal? FreezeScores { get; set; }
        public decimal? RebatePro { get; set; }
        public decimal? RebateProMoney { get; set; }
        public decimal? AllGain { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public decimal? AddedRebatePro { get; set; }
        public bool? IsPlay { get; set; }
        public int? QuotaTa { get; set; }
        public int? QuotaTb { get; set; }
        public int? QuotaTc { get; set; }
        public int? QuotaTd { get; set; }
        public int? QuotaTe { get; set; }
        public int? QuotaTf { get; set; }
        public string MoneyPwd { get; set; }
        public DateTime? MoneyPwdExpiredDate { get; set; }
        public string UniqueNum { get; set; }
        public string NotifyInfo { get; set; }
        public string UserPaths { get; set; }
        public bool? ISUpgrade { get; set; }
        public decimal? UpgradRebatePro { get; set; }
        public string Email { get; set; }
        public bool? IsOutMoney { get; set; }
        public bool? IslowMoneyIn { get; set; }
        public byte[] Tsamp { get; set; }
        public int? QuotaTg { get; set; }
        public bool? IsPwdRotection { get; set; }
        [Key]
        public int UserID { get; set; }
        public string lastLoginIp { get; set; }
        public string lastLoginAddress { get; set; }
        public System.DateTime LastUpdateTime { get; set; }
        public int? SumQuotaTa { get; set; }
        public int? SumQuotaTb { get; set; }
        public int? SumQuotaTc { get; set; }
        public int? SumQuotaTd { get; set; }
        public int? SumQuotaTe { get; set; }
        public int? SumQuotaTf { get; set; }
        public int? SumQuotaTg { get; set; }
        public decimal? AGAllGain { get; set; }
        public decimal? SportAllGain { get; set; }
        public DateTime? LastAGUpdateTime { get; set; }
        public DateTime? LastSportUpdateTime { get; set; }
        public int? Source { get; set; }
        public string ReservedInfo { get; set; }
        public decimal? NewAddedRebatePro { get; set; }
        public decimal? PtAllGain { get; set; }
        public DateTime? LastPtUpdateTime { get; set; }
        public decimal? DailyRebate { get; set; }
        public decimal? DailyRebateRate { get; set; }
        public int? DailySalaryParentID { get; set; }
        public string PhoneNumber { get; set; }
        public bool? IsPhoneRotection { get; set; }
        public DateTime? PasswordExpiredDate { get; set; }
        public decimal? LCAllGain { get; set; }
        public DateTime? LastLCUpdateTime { get; set; }
        public decimal? IMAllGain { get; set; }
        public DateTime? LastIMUpdateTime { get; set; }
        public decimal? RGAllGain { get; set; }
        public DateTime? LastRGUpdateTime { get; set; }
        public decimal? IMPTAllGain { get; set; }
        public DateTime? LastIMPTUpdateTime { get; set; }
        public decimal? IMPPAllGain { get; set; }
        public DateTime? LastIMPPUpdateTime { get; set; }
        public decimal? IMSportAllGain { get; set; }
        public DateTime? LastIMSportUpdateTime { get; set; }
        public decimal? IMeBETAllGain { get; set; }
        public DateTime? LastIMeBETUpdateTime { get; set; }
        public decimal? IMBGAllGain { get; set; }
        public DateTime? LastIMBGUpdateTime { get; set; }
        public bool? IsOutUSDT { get; set; }
        public decimal? IMSGAllGain { get; set; }
        public DateTime? LastIMSGUpdateTime { get; set; }
        public decimal? IMVRAllGain { get; set; }
        public DateTime? LastIMVRUpdateTime { get; set; }
        public decimal? ABEBAllGain { get; set; }
        public DateTime? LastABEBUpdateTime { get; set; }
        public decimal? PGSLAllGain { get; set; }
        public DateTime? LastPGSLUpdateTime { get; set; }
        public decimal? OBFIAllGain { get; set; }
        public DateTime? LastOBFIUpdateTime { get; set; }
        public decimal? EVEBAllGain { get; set; }
        public DateTime? LastEVEBUpdateTime { get; set; }
        public decimal? BTISAllGain { get; set; }
        public DateTime? LastBTISUpdateTime { get; set; }
        public decimal? OBSPAllGain { get; set; }
        public DateTime? LastOBSPUpdateTime { get; set; }
    }

    public static class UserInfoExtensions
    {
        public static string GetPassword(this UserInfo userInfo, PasswordType passwordType)
        {
            if(passwordType == PasswordType.Login)
            {
                return userInfo.UserPwd;
            }
            else if (passwordType == PasswordType.Money)
            {
                return userInfo.MoneyPwd;
            }

            throw new NotImplementedException();
        }
    }
}
