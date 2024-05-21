using JxBackendService.Interface.Service.Web.BackSideWeb;
using JxBackendService.Model.BackSideWeb;
using JxBackendService.Model.Enums.BackSideWeb.BotBet;
using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class BotParameterViewModel : SettingInfoContext, IDataKey
    {
        public string? KeyContent => Id.ToString();
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
        public string SettingGroupText
        {
            get
            {
                if (Enum.IsDefined(typeof(SettingGroup), SettingGroupId))
                {
                    SettingGroup enumValue = (SettingGroup)SettingGroupId;
                    return GetEnumDescription(enumValue);
                }
                else
                {
                    return SettingGroupId.ToString();
                }
            }
        }
        private static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

            return attribute == null ? value.ToString() : attribute.Description;
        }
        public string TimerValueText { get; set; }
        public int LotteryPatchType { get; set; }
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
        public List<SelectListItem> LotteryPatchTypeItems { get; set; }
        public List<SelectListItem> BotGroupItems { get; set; }
        public List<SelectListItem> TimeTypeItems { get; set; }
        public List<SelectListItem> SettingGroupItems { get; set; }
        public int OriginalLotteryPatchType { get; set; }
    }
}
