using JxBackendService.Common.Extensions;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace JxBackendService.Model.BackSideWeb
{
    public class BotParameterInput : SettingInfoContext
    {
        public int LotteryPatchType { get; set; }

        public int OriginalLotteryPatchType { get; set; }

        public string LotteryPatchTypeText
        {
            get
            {
                if (Enum.IsDefined(typeof(LotteryPatchType), LotteryPatchType))
                {
                    LotteryPatchType enumValue = (LotteryPatchType)LotteryPatchType;
                    return GetEnumDescription(enumValue);
                }
                else
                {
                    return SettingGroupId.ToString();
                }
            }
        }

        public string GroupIdText
        {
            get
            {
                if (Enum.IsDefined(typeof(BotGroup), GroupId))
                {
                    return ((BotGroup)GroupId).ToString();
                }
                else
                {
                    return GroupId.ToString();
                }
            }
        }

        public string SettingGroupIdText
        {
            get
            {
                if (Enum.IsDefined(typeof(SettingGroup), SettingGroupId))
                {
                    return ((SettingGroup)SettingGroupId).GetDescription();
                }
                else
                {
                    return SettingGroupId.ToString();
                }
            }
        }

        public string TimeTypeText
        {
            get
            {
                if (Enum.IsDefined(typeof(TimeType), TimeType))
                {
                    return ((TimeType)TimeType).ToString();
                }
                else
                {
                    return TimeType.ToString();
                }
            }
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
