using JxBackendService.Common.Util;

namespace JxBackendService.Model.ViewModel.Game
{
    public class UserProductScore
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public decimal AvailableScores { get; set; }

        public decimal FreezeScores { get; set; }

        public decimal TransferIn { get; set; }

        public string AvailableScoresText
        {
            get
            {
                if (AvailableScores == -9999)
                {
                    return "N/A";
                }

                return AvailableScores.ToCurrency();
            }
            set { }
        }

        public string FreezeScoresText
        {
            get
            {
                if (FreezeScores == -9999)
                {
                    return "N/A";
                }

                return FreezeScores.ToCurrency();
            }
            set { }
        }
    }
}