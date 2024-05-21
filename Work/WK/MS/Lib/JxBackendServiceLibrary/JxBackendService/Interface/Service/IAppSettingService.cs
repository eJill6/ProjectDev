using System.Collections.Generic;
using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.Param.OSS;

namespace JxBackendService.Interface.Service
{
    public interface IAppSettingService
    {
        string GetConnectionString(DbConnectionTypes dbConnectionType);

        string GetRedisConnectionString(DbIndexes dbIndex);

        RabbitMQSetting GetRabbitMQSetting();

        string CommonDataHash { get; }

        EnvironmentCode GetEnvironmentCode();
    }

    public interface IGameAppSettingService
    {
        IIMOneAppSetting GetIMAppSetting();

        IIMOneAppSetting GetIMPPAppSetting();

        IIMOneAppSetting GetIMPTAppSetting();

        IIMOneAppSetting GetIMSportAppSetting();

        IIMOneAppSetting GetIMeBETAppSetting();

        IIMBGAppSetting GetIMBGAppSetting();

        IAGAppSetting GetAGAppSetting();

        ILCAppSetting GetLCAppSetting();

        ISportAppSetting GetSportAppSetting();
    }

    public interface IBatchServiceAppSettingService : IAppSettingService
    {
    }

    public interface ITransferServiceAppSettingService
    {
        List<string> CopyBetLogToMerchantCodes { get; }
    }

    public interface INewTransferServiceAppSettingService : IAppSettingService, ITransferServiceAppSettingService
    {
    }

    public interface IOldTransferServiceAppSettingService : IAppSettingService, ITransferServiceAppSettingService
    {
    }
}