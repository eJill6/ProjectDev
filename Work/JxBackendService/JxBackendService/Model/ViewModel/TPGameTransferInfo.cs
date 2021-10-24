using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel
{
    public class TPGameTransferInfo
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string LogoImageUrl { get; set; }

        public List<string> RecommendAmountList { get; set; }

        public string MinAmountLimit { get; set; }

        public string MaxAmountLimit { get; set; }

        public string AvailableScores { get; set; }
    }
}
