using JxBackendService.Service.SMS;
using JxBackendService.Service.SMS.Mxtong;
using System;

namespace JxBackendService.Model.Enums
{
    public class SMSServiceProvider : BaseStringValueModel<SMSServiceProvider>
    {
        public Type SMSServiceType { get; private set; }

        private SMSServiceProvider()
        { }

        /// <summary>麥訊通</summary>
        public static SMSServiceProvider Mxtong = new SMSServiceProvider()
        {
            Value = "Mxtong",
            SMSServiceType = typeof(MxtongSMSService),
            Sort = 1,
        };

        /// <summary>騰訊雲</summary>
        public static SMSServiceProvider TencentCloud = new SMSServiceProvider()
        {
            Value = "TencentCloud",
            SMSServiceType = typeof(TencentCloudSMSService),
            Sort = 2,
        };

        #region 過往amd/d3曾經接過的簡訊商

        ///// <summary>網建短信通</summary>
        //public static SMSServiceProvider WangJian = new SMSServiceProvider()
        //{
        //    Value = "WangJian",
        //    SMSServiceType = typeof(WangJianSMSService),
        //    Sort = 1,
        //};

        ///// <summary>海客信使</summary>
        //public static SMSServiceProvider Heysky = new SMSServiceProvider()
        //{
        //    Value = "Heysky",
        //    SMSServiceType = typeof(HeyskySMSService),
        //    Sort = 2,
        //};

        ///// <summary>閃速碼</summary>
        //public static SMSServiceProvider Shansuma = new SMSServiceProvider()
        //{
        //    Value = "Shansuma",
        //    SMSServiceType = typeof(ShansumaSMSService),
        //    Sort = 3,
        //};

        #endregion 過往amd/d3曾經接過的簡訊商
    }
}