using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.BackSideWeb
{
    public class VIPUserBonusInitData
    {
        public List<JxBackendSelectListItem> BonusTypeItems { get; set; }

        public List<JxBackendSelectListItem> VIPLevelItems { get; set; }

        public List<JxBackendSelectListItem> ReceivedStatusItems { get; set; }
    }
}
