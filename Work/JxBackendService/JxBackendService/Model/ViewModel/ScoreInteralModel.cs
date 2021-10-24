using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel
{
    public class ScoreInteralModel
    {
        public List<StrengthRuleInfo> StrengthRuleInfos { get; set; }
        
        public List<StrengthScoreInterval> StrengthScoreIntervals { get; set; }
        
        public string NewPasswordDefaultInputMessage { get; set; }
        
        public FinalCheckRuleInfo FinalCheckRuleInfo { get; set; }
    }

    public class FinalCheckRuleInfo
    {
        public string RegExp { get; set; }

        public string CheckFailMessage { get; set; }
    }

    public class StrengthRuleInfo
    {
        public string RegExp { get; set; }
        public int Score { get; set; }
    }

    public class StrengthScoreInterval
    {
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public string IntervalName { get; set; }
    }

}
