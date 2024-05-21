using BackSideWeb.Helpers;
using JxBackendService.Common.Util;
using JxBackendService.Model.Entity.Base;
using MS.Core.MMModel.Models.Post.Enums;
using System.ComponentModel;
using System.Reflection;

namespace BackSideWeb.Model.Entity.MM
{
    public class MMOptionsBs : BaseEntityModel
    {
        public int OptionId { get; set; }
        public string OptionContent { get; set; }
        public byte OptionType { get; set; }

        public string OptionTypeText
        {
            get
            {
                string result = string.Empty;
                switch (OptionType)
                {
                    case 1:
                        result = "信息类型";
                        break;
                    case 2:
                        result = "申请调价";
                        break;
                    case 3:
                        result = "标签";
                        break;
                    case 4:
                        result = "服务项目";
                        break;
                }
                return result;
            }
        }
        public PostType PostType { get; set; }
        public string PostTypeText
        {
            get
            {
                return PostType.GetEnumDescription();
            }
        }
        public DateTime? ModifyDate { get; set; }
        public bool IsActive { get; set; }
        public string ModifyDateText => ModifyDate.ToFormatDateTimeString();
    }
}
