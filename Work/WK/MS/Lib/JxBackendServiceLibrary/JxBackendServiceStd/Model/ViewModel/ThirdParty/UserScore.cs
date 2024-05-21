namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class UserScore
    {
        public decimal AvailableScores { get; set; }

        public decimal FreezeScores { get; set; }
    }

    public class TPGameTransferReturnResult
    {
        public UserScore RemoteUserScore { get; set; }

        public string ApiResult { get; set; }
    }

    public class TotalUserScore
    {
        public int UserID { get; set; }

        public decimal TotalAvailableScores { get; set; }

        public decimal TotalFreezeScores { get; set; }
    }
}