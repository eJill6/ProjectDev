using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.ThirdParty
{
    public class PlayBetRecordQueryType : BaseIntValueModel<PlayBetRecordQueryType>
    {
        private PlayBetRecordQueryType()
        {
        }

        /// <summary>
        /// 平台用户名
        /// </summary>
        public static PlayBetRecordQueryType UserName = new PlayBetRecordQueryType()
        {
            Value = (int)PlayBetRecordQueryTypeValue.UserName,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.UserName)
        };

        /// <summary>
        /// 第三方用户名
        /// </summary>
        public static PlayBetRecordQueryType TPGameAccountName = new PlayBetRecordQueryType()
        {
            Value = (int)PlayBetRecordQueryTypeValue.TPGameAccountName,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.TPGameAccountName)
        };

        /// <summary>
        /// 平台注单号
        /// </summary>
        public static PlayBetRecordQueryType PlayInfoID = new PlayBetRecordQueryType()
        {
            Value = (int)PlayBetRecordQueryTypeValue.PlayInfoID,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.PlayInfoID)
        };

        /// <summary>
        /// 第三方注单号
        /// </summary>
        public static PlayBetRecordQueryType PlayID = new PlayBetRecordQueryType()
        {
            Value = (int)PlayBetRecordQueryTypeValue.PlayID,
            ResourceType = typeof(ThirdPartyGameElement),
            ResourcePropertyName = nameof(ThirdPartyGameElement.PlayID)
        };
    }
}