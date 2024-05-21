using System;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Model.Param.SMS;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Service.SMS.Mxtong
{
    public class MxtongSMSService : ISMSService
    {
        public MxtongSMSService()
        {
        }

        public BaseReturnModel SendSMS(SendUserSMSParam sendUserSMSParam)
        {
            ISMSService smsService = GetMxtongService(sendUserSMSParam.CountryCode);

            return smsService.SendSMS(sendUserSMSParam);
        }

        private ISMSService GetMxtongService(string countryCode)
        {
            if (countryCode == CountryCode.China)
            {
                return new MxtongChinaSMSService();
            }
            else
            {
                return new MxtongInternationalSMSService();
            }
        }
    }
}