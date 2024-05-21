using System;

namespace JxBackendService.Model.ThirdParty.Base
{
    public interface IBaseRemoteBetLog
    {
        string KeyId { get; }

        string TPGameAccount { get; }

        string Memo { get; set; }

        DateTime LocalSavedTime { get; set; }

        int RemoteSaved { get; set; }

        DateTime? RemoteSavedTime { get; set; }

        int RemoteSaveTryCount { get; set; }

        DateTime? RemoteSaveLastTryTime { get; set; }
    }

    public abstract class BaseRemoteBetLog : IBaseRemoteBetLog
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

    public class JsonRemoteBetLog : BaseRemoteBetLog
    {
        public override string KeyId  { get;}

        public override string TPGameAccount { get; }

        public string BetLogJson { get; set; }
    }
}