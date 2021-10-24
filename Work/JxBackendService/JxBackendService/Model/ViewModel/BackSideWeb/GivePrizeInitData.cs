using JxBackendService.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.BackSideWeb
{
    public class GivePrizeInitData
    {
        public bool IsWalletTypeVisible { get; set; }

        public bool IsFlowMultipleVisible { get; set; }

        public List<JxBackendSelectListItem<bool>> WalletTypeItems { get; set; }
    }
}
