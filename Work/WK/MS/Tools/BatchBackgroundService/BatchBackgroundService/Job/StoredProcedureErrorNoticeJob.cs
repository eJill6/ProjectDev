using BatchService.Job.Base;
using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service;
using JxBackendService.Model.Common;
using JxBackendService.Model.Entity.StoredProcedureErrorLog;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ErrorHandle;

namespace BatchService.Job
{
    public class StoredProcedureErrorNoticeJob : BaseBatchServiceQuartzJob
    {
        private readonly Lazy<IStoredProcedureErrorLogService> _sqlJobErrorLogService;

        private static int _lastInlodbErrorLogId = 0;

        private static int _lastInlodbBakErrorLogId = 0;

        private static DateTime? _lastJobRunLogStartTime = null;

        public StoredProcedureErrorNoticeJob()
        {
            _sqlJobErrorLogService = DependencyUtil.ResolveJxBackendService<IStoredProcedureErrorLogService>(EnvUser, DbConnectionTypes.Slave);
        }

        public override void DoJob()
        {
            List<string> errorMessages = GetProErrorLogsNoticeMessage(ref _lastInlodbErrorLogId, InlodbType.Inlodb);
            errorMessages.AddRange(GetProErrorLogsNoticeMessage(ref _lastInlodbBakErrorLogId, InlodbType.InlodbBak));
            errorMessages.AddRange(GetJobRunLogNoticeMessage());

            if (errorMessages.Any())
            {
                SendTelegramMessage(errorMessages);
            }
        }

        private List<string> GetProErrorLogsNoticeMessage(ref int logId, InlodbType inlodbType)
        {
            var errorMsgs = new List<string>();

            if (logId == 0)
            {
                Pro_ErrorLogs proErrorLog = _sqlJobErrorLogService.Value.GetProErrorLogs(inlodbType);

                if (proErrorLog != null)
                {
                    errorMsgs.Add(ConverToNoticeMessage(proErrorLog));
                    logId = proErrorLog.ErrorLogID;
                }
            }
            else if (logId > 0)
            {
                List<Pro_ErrorLogs> proErrorLogsList = _sqlJobErrorLogService.Value.GetProErrorLogsList(inlodbType, logId);
                if (proErrorLogsList.Any())
                {
                    logId = proErrorLogsList.Select(s => s.ErrorLogID).Max();
                    errorMsgs = proErrorLogsList.Select(s => ConverToNoticeMessage(s)).ToList();
                }
            }

            return errorMsgs;
        }

        private List<string> GetJobRunLogNoticeMessage()
        {
            var errorMsgs = new List<string>();

            if (_lastJobRunLogStartTime == null)
            {
                _lastJobRunLogStartTime = DateTime.Now.AddDays(-4);
            }

            List<JobRunLog> jobRunLogList = _sqlJobErrorLogService.Value.GetJobRunLogStatus(_lastJobRunLogStartTime);

            if (jobRunLogList.Where(w => w.Status.IsNullOrEmpty()).Any())
            {
                errorMsgs.Add("JobRunLog Error");

                DateTime newStartTime = jobRunLogList.OrderByDescending(o => o.StartTime).Select(s => s.StartTime).FirstOrDefault();
                _lastJobRunLogStartTime = newStartTime;
            }

            return errorMsgs;
        }

        private string ConverToNoticeMessage(Pro_ErrorLogs data)
        {
            return $"ErrorProcedure:{data.ErrorProcedure}, ErrorMessage:{data.ErrorMessage}";
        }

        private void SendTelegramMessage(List<string> messages)
        {
            TelegramUtil.SendMessageWithEnvInfoAsync(new SendTelegramParam()
            {
                ApiUrl = SharedAppSettings.TelegramApiUrl,
                EnvironmentUser = EnvUser,
                Message = messages.ToJsonString()
            });
        }
    }
}