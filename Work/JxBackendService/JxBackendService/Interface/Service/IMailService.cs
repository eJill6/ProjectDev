using JxBackendService.Model.Param;
using JxBackendService.Model.Param.Mail;
using JxBackendService.Model.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Service
{
    public interface IMailService
    {
        BaseReturnModel SendMail(SendMailParam sendMailParam);
    }

    public interface ISendMailManagerService
    {
        BaseReturnModel SendMail(SendMailParam sendMailParam, object refInfo);
    }
}
