namespace JxBackendService.Model.ThirdParty.SabaSport
{
    public class SportRegisterData
    {
        public string Vendor_Member_ID { get; set; }
        public string OperatorId { get; set; }
        public string FirstName => string.Empty;
        public string LastName => string.Empty;
        public string UserName { get; set; }
        public string OddsType { get; set; }
        public string Currency { get; set; }
        public string CustomInfo1 { get; set; }
        public string CustomInfo2 { get; set; }
        public string CustomInfo3 => string.Empty;
        public string CustomInfo4 => string.Empty;
        public string CustomInfo5 => string.Empty;
        public string MaxTransfer => "100000000";
        public string MinTransfer => "1";
    }
}
