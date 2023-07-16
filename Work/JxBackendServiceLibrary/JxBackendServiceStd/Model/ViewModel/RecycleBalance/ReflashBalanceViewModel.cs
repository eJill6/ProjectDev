using JxBackendService.Model.MiseLive.Response;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Model.ViewModel.ThirdParty;
using System;
using System.Collections.Generic;
using System.Text;

namespace JxBackendService.Model.ViewModel.RecycleBalance
{
    public class MiseAndTPGameBalance
    {
        public string UserID { get; set; }

        public MiseLiveBalance MiseLiveBalance { get; set; }

        public UserAccountSearchResult UserAccountSearchResult { get; set; } = new UserAccountSearchResult();
    }
}