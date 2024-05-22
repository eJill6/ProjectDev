using ControllerShareLib.Models.Base;
using JxBackendService.Model.ReturnModel;
using M.Core.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace M.Core.Controllers;

public class EntranceController : BaseApiController
{
    public EntranceController()
    {
    }

    /// <summary>
    /// App 全域資訊入口
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public AppResponseModel<EntranceInfoModel> Info()
    {
        return new AppResponseModel<EntranceInfoModel>()
        {
            Success = true,
            Data = new EntranceInfoModel()
            {
                MqUrl = ConfigUtilService.Get("RabbitMQ.AMQP"),
                MqUrls = ConfigUtilService.Get<RabbitMqSettings[]>("Default:RabbitMQ.AMQPs"),
            }
        };
    }
}