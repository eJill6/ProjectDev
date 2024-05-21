using Castle.Core.Internal;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Model.Param.Game;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.Game;
using JxBackendService.Resource.Element;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.Param.Game
{
    public class LiveGameManageQueryParam : BaseGameQueryParam
    {
        public string LotteryType { get; set; }

        public string SearchStatus { get; set; }

        public string TabType { get; set; }

        public int? IsActive
        {
            get
            {
                if (SearchStatus == true.ToString())
                {
                    return 1;
                }
                else if (SearchStatus == false.ToString())
                {
                    return 0;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public class BaseLiveGameManageParam : BaseGameManageParam, ISearchLiveGameManageDataType, ILiveGameManageSetDefaultParam
    {
        [Display(Name = nameof(DisplayElement.Name), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired, CustomizedMaxLength(50)]
        public string LotteryType { get; set; }

        [Display(Name = nameof(DisplayElement.GameID), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        public int LotteryId { get; set; } = 0;

        [DisplayName("URL")]
        [CustomizedMaxLength(1000)]
        public string Url { get; set; }

        [DisplayName("API URL")]
        [CustomizedMaxLength(1000)]
        public string ApiUrl { get; set; }

        [Display(Name = nameof(DisplayElement.FrameRatio), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]
        [CustomizedRegularExpression(RegularExpressionEnumTypes.OneIntAndTwoDigits)]
        public decimal FrameRatio { get; set; }

        [Display(Name = nameof(DisplayElement.Style), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]        
        public int Style { get; set; }

        public bool IsCountdown { get; set; }

        [Display(Name = nameof(DisplayElement.Duration), ResourceType = typeof(DisplayElement))]
        [CustomizedRequired]        
        public int Duration { get; set; }

        public bool IsH5 { get; set; }

        public bool IsFollow { get; set; }

        public int TabType { get; set; }

        public string ProductCode { get; set; }

        public string GameCode { get; set; }

        [CustomizedMaxLength(50)]
        public string RemoteCode { get; set; }

        public int LiveGameManageDataTypeValue { get; set; }
    }

    public class LiveGameManageCreateParam : BaseLiveGameManageParam
    {
    }

    public class LiveGameManageUpdateParam : BaseLiveGameManageParam
    {
    }

    public class LiveGameManageDataType : BaseIntValueModel<LiveGameManageDataType>
    {
        private LiveGameManageDataType()
        {
            ResourceType = typeof(SelectItemElement);
        }

        public static readonly LiveGameManageDataType GameCenter = new LiveGameManageDataType()
        {
            Value = 1,
            ResourcePropertyName = nameof(SelectItemElement.LiveGameManageDataType_GameCenter),
            Sort = 1
        };

        public static readonly LiveGameManageDataType DirectPlay = new LiveGameManageDataType()
        {
            Value = 2,
            ResourcePropertyName = nameof(SelectItemElement.LiveGameManageDataType_DirectPlay),
            Sort = 2
        };

        public static readonly LiveGameManageDataType MiseLottery = new LiveGameManageDataType()
        {
            Value = 3,
            ResourcePropertyName = nameof(SelectItemElement.LiveGameManageDataType_MiseLottery),
            Sort = 3
        };

        public static string GetName(ISearchLiveGameManageDataType searchLiveGameManageDataType)
        {
            LiveGameManageDataType liveGameManageDataType = GetSingle(searchLiveGameManageDataType);

            if (liveGameManageDataType == null)
            {
                return string.Empty;
            }

            return liveGameManageDataType.Name;
        }

        public static LiveGameManageDataType GetSingle(ISearchLiveGameManageDataType searchLiveGameManageDataType)
        {
            int lotteryId = searchLiveGameManageDataType.LotteryId;

            if (lotteryId > 0)
            {
                return MiseLottery;
            }

            if (!searchLiveGameManageDataType.ProductCode.IsNullOrEmpty())
            {
                if (searchLiveGameManageDataType.RemoteCode.IsNullOrEmpty())
                {
                    return GameCenter;
                }
                else
                {
                    return DirectPlay;
                }
            }

            return null;
        }
    }
}