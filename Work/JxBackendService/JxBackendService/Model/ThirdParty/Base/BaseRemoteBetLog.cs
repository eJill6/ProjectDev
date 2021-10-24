using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ThirdParty.Base
{
    public abstract class BaseRemoteBetLog
    {
        public abstract string KeyId { get; }

        public abstract string TPGameAccount { get; }

        public string Memo { get; set; }

        public DateTime LocalSavedTime { get; set; }
        public int RemoteSaved { get; set; }
        public DateTime? RemoteSavedTime { get; set; }
        public int RemoteSaveTryCount { get; set; }
        public DateTime? RemoteSaveLastTryTime { get; set; }
    }

    public class JsonRemoteBetLog
    {
        public  string KeyId { get; set; }

        public  string TPGameAccount { get; set; }

        public string Memo { get; set; }

        public string BetLogJson { get; set; }

        public DateTime LocalSavedTime { get; set; }
        
        public int RemoteSaved { get; set; }
        
        public DateTime? RemoteSavedTime { get; set; }
        
        public int RemoteSaveTryCount { get; set; }
        
        public DateTime? RemoteSaveLastTryTime { get; set; }        
    }
}