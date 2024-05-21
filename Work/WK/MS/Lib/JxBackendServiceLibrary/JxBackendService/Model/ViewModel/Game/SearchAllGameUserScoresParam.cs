namespace JxBackendService.Model.ViewModel.Game
{
    public class SearchAllGameUserScoresParam
    {
        public int UserID { get; set; }

        public bool IsIncludeMainScores { get; set; }

        public bool IsForcedRefresh { get; set; }
    }
}