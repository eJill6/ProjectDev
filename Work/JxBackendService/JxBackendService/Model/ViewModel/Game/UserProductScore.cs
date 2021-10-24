using JxBackendService.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Game
{
    public class UserProductScore
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public decimal AvailableScores { get; set; }

        public decimal FreezeScores { get; set; }

        public decimal AllGain { get; set; }

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

        public string AllGainText { get => AllGain.ToCurrency(); set { } }
    }
}
