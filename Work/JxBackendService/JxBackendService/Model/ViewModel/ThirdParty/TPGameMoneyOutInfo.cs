namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class TPGameMoneyOutInfo : BaseTPGameMoneyInfo
    {
        public string MoneyOutID { get; set; }
        public override string GetMoneyID() => MoneyOutID;

        public override string GetPrimaryKeyColumnName() => nameof(MoneyOutID);
    }
}
