using JxBackendService.Common.Util;

namespace JxBackendService.Common.Extensions
{
    public static class AESExtension
    {
        public static string ToDescryptedPhone(this string encryptedPhone, string phoneHash)
        {
            if (encryptedPhone.IsNullOrEmpty())
            {
                return string.Empty;
            }

            DESTool tool = new DESTool(phoneHash.ToTrimString());
            string descryptedContent = CommUtil.CommUtil.DecryptDES(tool.DESDeCode(encryptedPhone));
            return descryptedContent;
        }

        public static string ToEncryptedPhone(this string descryptedPhone, string phoneHash)
        {
            DESTool tool = new DESTool(phoneHash);
            string encryptedContent = tool.DESEnCode(CommUtil.CommUtil.EncryptDES(descryptedPhone));
            return encryptedContent;
        }

        public static string ToDescryptedEmail(this string encryptedEmail, string emailHash)
        {
            DESTool tool = new DESTool(emailHash.ToTrimString());
            string descryptedContent = CommUtil.CommUtil.DecryptDES(tool.DESDeCode(encryptedEmail));
            return descryptedContent;
        }

        public static string ToEncryptedEmail(this string descryptedEmail, string emailHash)
        {
            DESTool tool = new DESTool(emailHash.ToTrimString());
            string content = tool.DESEnCode(CommUtil.CommUtil.EncryptDES(descryptedEmail));
            return content;
        }

        public static string ToDescryptedData(this string encryptedData, string commonDataHash)
        {
            DESTool tool = new DESTool(commonDataHash.ToTrimString());
            string descryptedContent = CommUtil.CommUtil.DecryptDES(tool.DESDeCode(encryptedData));
            return descryptedContent;
        }

        public static string ToEncryptedData(this string descryptedData, string commonDataHash)
        {
            DESTool tool = new DESTool(commonDataHash.ToTrimString());
            string content = tool.DESEnCode(CommUtil.CommUtil.EncryptDES(descryptedData));
            return content;
        }
    }
}
