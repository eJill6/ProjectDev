using JxBackendService.Model.ThirdParty.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Interface.Repository
{
    public interface ITransferSqlLiteRepository
    {
        void InitSettings();

        string GetLastSearchToken();

        int UpdateNextSearchToken(string newToken);
    }

    public interface ITransferSqlLiteBackupRepository
    {
        void BackupNewBetLogs<T>(List<T> betLogs) where T : BaseRemoteBetLog;

        void DeleteExpiredDbFile();

        List<T> GetBetLogs<T>(DateTime fileDate, int rowCount);

        void InsertBetLogs<T>(Dictionary<string, T> betLogMap);
    }
}