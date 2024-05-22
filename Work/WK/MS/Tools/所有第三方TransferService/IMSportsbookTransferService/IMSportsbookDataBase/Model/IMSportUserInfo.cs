namespace IMSportsbookDataBase.Model
{
    public class IMSportsbookUserInfo
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public decimal? TransferIn { get; set; }

        public decimal? TransferOut { get; set; }

        public decimal? WinOrLoss { get; set; }

        public decimal? Rebate { get; set; }

        public decimal? AvailableScores { get; set; }

        public decimal? FreezeScores { get; set; }
    }
}