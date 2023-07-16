namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class BaseCompensationParam
    {
        public int UserID { get; set; }

        public string ProductCode { get; set; }
    }

    public class SaveCompensationParam : BaseCompensationParam
    {
        public string TransferID { get; set; }
    }

    public class ProcessedCompensationParam : BaseCompensationParam
    {
    }
}