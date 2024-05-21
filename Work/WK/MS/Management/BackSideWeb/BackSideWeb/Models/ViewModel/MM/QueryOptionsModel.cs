using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;
using MS.Core.MMModel.Models.Post.Enums;
using System.ComponentModel;
using System.Reflection;

namespace BackSideWeb.Model.ViewModel.MM
{
    public class QueryOptionsModel : MMOptionsBs, IDataKey
    {
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

        public string PostTypeText
        {
            get
            {
                return PostType.GetEnumDescription();
            }
        }
        public string IsActiveText => IsActive ? "显示" : "隐藏";
        public string KeyContent => OptionId.ToString();
    }
}
