using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums;
using System;

namespace JxBackendService.Model.ReturnModel
{
    public class SPReturnModel
    {
        public string ReturnCode { get; set; }

        public string ReturnMsg { get; set; }

        public bool? IsEncryptedReturnMsg { get; set; }

        public string GetHandledMsg(string commonHash)
        {
            return GetHandledMsg((msg) =>
            {
                return msg.ToDescryptedData(commonHash);
            });
        }

        public string GetHandledMsg(Func<string, string> descryptJob)
        {
            if (IsEncryptedReturnMsg == true)
            {
                return descryptJob.Invoke(ReturnMsg);
            }

            return ReturnMsg;
        }

        public ReturnCode ToReturnCodeModel() => Enums.ReturnCode.GetSingle(ReturnCode);

        public BaseReturnModel ToBaseReturnModel() => new BaseReturnModel(ToReturnCodeModel());
    }
}
