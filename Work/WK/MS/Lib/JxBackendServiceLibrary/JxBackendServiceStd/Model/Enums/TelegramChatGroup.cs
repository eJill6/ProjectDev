using System.Collections.Generic;

namespace JxBackendService.Model.Enums
{
    public enum TelegramChatTypes
    {
        Common,

        Network,

        CustomerService,

        PDTeam,
    }

    public class TelegramChatGroup : BaseStringValueModel<TelegramChatGroup>
    {
        private static readonly string s_testingBotToken = "5516244587:AAElP8ybm_mip_gE8LA8nhcYwZ_Gujx31Oo";

        private static readonly string s_productionBotToken = "5663934060:AAHx4qDNl5GjQsOuBHr5LVkceqRgZTu4d3w";

        public string Token { get; private set; }

        public TelegramChatTypes TelegramChatType { get; private set; }

        public HashSet<EnvironmentCode> AllowEnvironmentCodes { get; set; }

        public long ChatId { get; private set; }

        private TelegramChatGroup()
        { }

        #region Production Environment Groups

        public static TelegramChatGroup Production = new TelegramChatGroup()
        {
            Value = "Production",
            ChatId = -1001752531204,
            TelegramChatType = TelegramChatTypes.Common,
            Token = s_productionBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Production }
        };

        public static TelegramChatGroup NetworkNotifyProduction = new TelegramChatGroup()
        {
            Value = "NetworkNotifyProduction",
            ChatId = -1001607053780,
            TelegramChatType = TelegramChatTypes.Network,
            Token = s_productionBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Production }
        };

        public static TelegramChatGroup CustomerServiceProduction = new TelegramChatGroup()
        {
            Value = "CustomerServiceProduction",
            ChatId = -1001965594770,
            TelegramChatType = TelegramChatTypes.CustomerService,
            Token = s_productionBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Production }
        };

        public static TelegramChatGroup PDTeamServiceProduction = new TelegramChatGroup()
        {
            Value = "PDTeamServiceProduction",
            ChatId = -1002134012944,
            TelegramChatType = TelegramChatTypes.PDTeam,
            Token = s_productionBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Production }
        };

        #endregion Production Environment Groups

        #region Testing Environment Groups

        public static TelegramChatGroup Testing = new TelegramChatGroup()
        {
            Value = "Testing",
            ChatId = -1001534596608,
            TelegramChatType = TelegramChatTypes.Common,
            Token = s_testingBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Development, EnvironmentCode.SIT, EnvironmentCode.UAT }
        };

        public static TelegramChatGroup NetworkNotifyTesting = new TelegramChatGroup()
        {
            Value = "NetworkNotifyTesting",
            ChatId = -1001534596608,
            TelegramChatType = TelegramChatTypes.Network,
            Token = s_testingBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Development, EnvironmentCode.SIT, EnvironmentCode.UAT }
        };

        public static TelegramChatGroup CustomerServiceTesting = new TelegramChatGroup()
        {
            Value = "CustomerServiceTesting",
            ChatId = -1001534596608,
            TelegramChatType = TelegramChatTypes.CustomerService,
            Token = s_testingBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Development, EnvironmentCode.SIT, EnvironmentCode.UAT }
        };

        public static TelegramChatGroup PDTeamServiceTesting = new TelegramChatGroup()
        {
            Value = "PDTeamServiceTesting",
            ChatId = -1002050802509,
            TelegramChatType = TelegramChatTypes.PDTeam,
            Token = s_testingBotToken,
            AllowEnvironmentCodes = new HashSet<EnvironmentCode>() { EnvironmentCode.Development, EnvironmentCode.SIT, EnvironmentCode.UAT }
        };

        #endregion Testing Environment Groups
    }

    public class TelegramSendMessageRequest
    {
        public long Chat_id { get; set; }

        public string Text { get; set; }
    }
}