using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.ThirdParty
{
    public class PGVerifySessionModel
    {
        public string operator_token { get; set; }
        public string secret_key { get; set; }
        public string operator_player_session { get; set; }

        public string traceId { get; set; }
        public string ip { get; set; }
        public string custom_parameter { get; set; }
        public string game_id { get; set; }

    }
}
