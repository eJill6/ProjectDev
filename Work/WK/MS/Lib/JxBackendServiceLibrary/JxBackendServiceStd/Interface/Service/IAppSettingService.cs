using JxBackendService.Model.Common;
using JxBackendService.Model.Common.IMOne;
using JxBackendService.Model.Enums;
using JxBackendService.Model.MessageQueue;
using JxBackendService.Model.ViewModel.AppDownload;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service
{
    public interface IAppSettingService
    {
        string GetConnectionString(DbConnectionTypes dbConnectionType);

        string GetRedisConnectionString(DbIndexes dbIndex);

        string CommonDataHash { get; }

        EnvironmentCode GetEnvironmentCode();

        List<RabbitMQWebSocketSetting> GetEndUserRabbitMQWebSocketSettings();

        List<RabbitMQSetting> GetEndUserRabbitMQSettings();

        List<RabbitMQSetting> GetInternalRabbitMQSettings();

        int? MinWorkerThreads { get; }

        bool IsEnabledMethodInvocationLog { get; }
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

    public interface IAppDownloadWebAppSettingService : IAppSettingService
    {
        string OpenInstallAppKey { get; }

        string CustomerServiceUrl { get; }

        IOSLiteSetting IOSLiteSetting { get; }
    }
}