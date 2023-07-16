namespace JxBackendService.Common.Extensions
{
    public static class MathExtensions
    {
        public static decimal AchievementRate(this decimal number, decimal @base)
        {
            if (@base < 1)
            {
                return 0;
            }

            var rate = number / @base;

            return rate > 1 ? 1 : rate;
        }
    }
}