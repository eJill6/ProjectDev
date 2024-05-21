using JxBackendService.Model.ThirdParty.Base;

namespace JxBackendService.Model.ThirdParty.PM.PMSL
{
    public class PMSLBetLog : BaseRemoteBetLog
    {
        public long Bi { get; set; }
        public int Mi { get; set; }
        public string Mmi { get; set; }
        public int St { get; set; }
        public int Et { get; set; }
        public int Gd { get; set; }
        public int Gi { get; set; }
        public string Gn { get; set; }
        public int Gt { get; set; }
        public string Gr { get; set; }
        public int Mw { get; set; }
        public int Mp { get; set; }
        public int Bc { get; set; }
        public int Dt { get; set; }
        public string Cn { get; set; }
        public int Tb { get; set; }
        public int Gf { get; set; }
        public override string KeyId => Bi.ToString();
        public override string TPGameAccount => Mmi;
    }
}