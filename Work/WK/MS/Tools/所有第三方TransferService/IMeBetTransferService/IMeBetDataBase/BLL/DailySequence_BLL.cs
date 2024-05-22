using IMeBetDataBase.Common;
using IMeBetDataBase.DLL;
using System;

namespace IMeBetDataBase.BLL
{
    public class DailySequence_BLL
    {
        DailySequence_DLL dailySequence_DLL = null;

        public DailySequence_BLL(string dbFullName)
        {
            dailySequence_DLL = new DailySequence_DLL(dbFullName);
        }

        public void InitializeADailySequence(string dailyDate)
        {
            try
            {
                dailySequence_DLL.InitializeADailySequence(dailyDate);
            }
            catch (Exception ex)
            {
                LogsManager.Error(ex);
                throw;
            }
        }

        public string UpdateAndGetSequenceNumber(string dailyDate)
        {
            string result;
            try
            {
                result = dailySequence_DLL.UpdateAndGetSequenceNumber(dailyDate);
            }
            catch (Exception ex)
            {
                LogsManager.Error(ex);
                throw;
            }

            return result;
        }

        public bool IsExistDailySequenceRecord(string dailyDate)
        {
            bool result;
            try
            {
                result = dailySequence_DLL.IsExistDailySequenceRecord(dailyDate);
            }
            catch (Exception ex)
            {
                LogsManager.Error(ex);
                throw;
            }

            return result;
        }
    }
}
