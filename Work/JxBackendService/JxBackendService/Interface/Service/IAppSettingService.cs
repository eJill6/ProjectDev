using JxBackendService.Model.Enums;
using JxBackendService.Model.Finance.Apollo;
using JxBackendService.Model.MessageQueue;

namespace JxBackendService.Interface.Service
{
    public interface IAppSettingService
    {
        string GetConnectionString(DbConnectionTypes dbConnectionType);

        string GetRedisConnectionString(DbIndexes dbIndex);

        RabbitMQSetting GetRabbitMQSetting();

        int AuthenticatorExpiredDays { get; }

        string CommonDataHash { get; }

        string EmailHash { get; }

        bool IsClientPinCompareExactly { get; }

        ApolloConfigParam GetApolloConfigParam();
    }    
}
