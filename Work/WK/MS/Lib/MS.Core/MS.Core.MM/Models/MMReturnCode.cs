using MS.Core.Models;

namespace MS.Core.MM.Models
{
    public class MMReturnCode : ReturnCode
    {
        public MMReturnCode(string code, bool isSuccess = false) : base(code, isSuccess)
        {
        }

        // BannerService ReturnCode
        public static ReturnCode DateIsExpired = Factory("E10001");

        public static ReturnCode DateIncorrect = Factory("E10002");

        public static ReturnCode SortIsUsed = Factory("E10003");

        public static ReturnCode SortLimit = Factory("E10004");

        // Media
        public static ReturnCode ImageSizeMoreThanLimit = Factory("E20001");

        // HomeAnnouncement
        public static ReturnCode WeightIsUsed = Factory("E30001");
    }
}