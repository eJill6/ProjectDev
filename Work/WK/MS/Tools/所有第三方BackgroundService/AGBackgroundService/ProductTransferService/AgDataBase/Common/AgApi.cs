namespace ProductTransferService.AgDataBase.Common
{
    public interface IAgApi
    {
        string GetDateTimeRemotePath();

        string GetDateTimeRemotePath(int addDays);
    }

    public class AgApi : IAgApi
    {
        public string GetDateTimeRemotePath() => GetDateTimeRemotePath(0);

        public string GetDateTimeRemotePath(int addDays)
        {
            // 美東時間
            DateTime easternStandardTime = DateTime.Now.AddHours(-12).AddMinutes(-30);

            if (addDays != 0)
            {
                easternStandardTime = easternStandardTime.AddDays(addDays);
            }

            return easternStandardTime.ToString("yyyyMMdd");
        }
    }

    public class AgMockApi : IAgApi
    {
        public string GetDateTimeRemotePath() => "20230104";

        public string GetDateTimeRemotePath(int addDays) => DateTime.Parse("2023-01-04").AddDays(addDays).ToString("yyyyMMdd");
    }
}