namespace JxBackendService.Model.ViewModel.Report
{
    public class BaseStatModel
    {
        public int TotalCount { get; set; }
    }

    public class AmountStatModel : BaseStatModel
    {
        public decimal TotalAmount { get; set; }
    }

    public class TotalAndPageStatModel : AmountStatModel
    {
        public decimal PageTotalCount { get; set; }

        public decimal PageTotalAmount { get; set; }
    }
}
