using JxBackendService.Model.Enums;
using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackSideWeb.Models.Enums
{
    public class PublishRecordPostRegional : BaseIntValueModel<PublishRecordPostRegional>
    {
        public PublishRecordPostRegional()
        {
            ResourceType = typeof(SelectItemElement);
        }

        /// <summary>
        /// 体验
        /// </summary>
        public static readonly PublishRecordPostRegional Experience = new PublishRecordPostRegional()
        {
            Value = 1,
            ResourcePropertyName = nameof(SelectItemElement.PublishRecord_Experience),
            Sort = 1
        };

        /// <summary>
        /// 寻芳阁
        /// </summary>
        public static readonly PublishRecordPostRegional IntermediaryAgent = new PublishRecordPostRegional()
        {
            Value = 2,
            ResourcePropertyName = nameof(SelectItemElement.PublishRecord_IntermediaryAgent),
            Sort = 2
        };

        /// <summary>
        /// 官方
        /// </summary>
        public static readonly PublishRecordPostRegional Official = new PublishRecordPostRegional()
        {
            Value = 3,
            ResourcePropertyName = nameof(SelectItemElement.PublishRecord_Official),
            Sort = 3
        };

        /// <summary>
        /// 广场
        /// </summary>
        public static readonly PublishRecordPostRegional Square = new PublishRecordPostRegional()
        {
            Value = 4,
            ResourcePropertyName = nameof(SelectItemElement.PublishRecord_Square),
            Sort = 4
        };
    }
}