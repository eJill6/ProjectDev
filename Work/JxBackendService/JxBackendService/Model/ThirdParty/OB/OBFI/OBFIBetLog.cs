using JxBackendService.Model.ThirdParty.Base;

namespace JxBackendService.Model.ThirdParty.OB.OBFI
{
    public class OBFIBetLog : BaseRemoteBetLog
    {
        public long bi { get; set; }
        public int mi { get; set; }
        public string mmi { get; set; }
        public int st { get; set; }
        public int et { get; set; }
        public int gd { get; set; }
        public int gi { get; set; }
        public string gn { get; set; }
        public int gt { get; set; }
        public string gr { get; set; }
        public int mw { get; set; }
        public int mp { get; set; }
        public int bc { get; set; }
        public int dt { get; set; }
        public string cn { get; set; }
        public int tb { get; set; }
        public int gf { get; set; }
        public override string KeyId => bi.ToString();
        public override string TPGameAccount => mmi;

    }
}
