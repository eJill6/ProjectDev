using BackSideWeb.Model.Entity.MM;
using JxBackendService.Interface.Service.Web.BackSideWeb;

namespace BackSideWeb.Model.ViewModel.Game
{
    public class QueryAdvertisingContentModel : MMAdvertisingContentBs, IDataKey
    {
        /// 廣告類型。1：什么是XX、2 : 如何发XX帖。
        public string AdvertisingText
        {
            get
            {
                string result = string.Empty;
                if (AdvertisingType == 1)
                    result = "什么是XX";
                else if (AdvertisingType == 2)
                    result = "如何XX";
                else if (AdvertisingType == 3)
                    result = "管理员账号";
                else if (AdvertisingType == 4)
                    result = "已解锁tip";
                else if (AdvertisingType == 5)
                    result = "下载提示文字";
                else if (AdvertisingType == 6)
                    result = "下载URL";
                else if (AdvertisingType == 7)
                    result = "XX跑马灯";
                else if (AdvertisingType == 8)
                    result = "XX新客必看";
                else if (AdvertisingType == 9)
                    result = "XX官方提示";
                else
                    return result;
                if (AdvertisingType == 2)
                {
                    switch (ContentType)
                    {
                        case 1:
                            result = result.Replace("XX", "发广场帖");
                            break;

                        case 2:
                            result = result.Replace("XX", "发寻芳阁帖");
                            break;

                        case 3:
                            result = result.Replace("XX", "发官方帖");
                            break;

                        case 4:
                            result = result.Replace("XX", "发体验帖");
                            break;

                        case 5:
                            result = result.Replace("XX", "成为觅经纪");
                            break;

                        case 6:
                            result = result.Replace("XX", "成为觅老板");
                            break;

                        case 7:
                            result = result.Replace("XX", "成为觅女郎");
                            break;

                        case 8:
                            result = result.Replace("XX", "成为星觅官");
                            break;
                    }
                }
                else
                {
                    switch (ContentType)
                    {
                        case 1:
                            result = result.Replace("XX", "广场");
                            break;

                        case 2:
                            result = result.Replace("XX", "寻芳阁");
                            break;

                        case 3:
                            result = result.Replace("XX", "官方");
                            break;

                        case 4:
                            result = result.Replace("XX", "体验");
                            break;

                        case 5:
                            result = result.Replace("XX", "觅经纪");
                            break;

                        case 6:
                            result = result.Replace("XX", "觅老板");
                            break;

                        case 7:
                            result = result.Replace("XX", "觅女郎");
                            break;

                        case 8:
                            result = result.Replace("XX", "星觅官");
                            break;
                    }
                }
                return result;
            }
        }

        public string IsActiveText => IsActive ? "显示" : "隐藏";

        public string AdvertisingContentText => AdvertisingContent.Length > 120 ? $"{AdvertisingContent.Substring(0, 120)}..." : AdvertisingContent;
        public string KeyContent => Id.ToString();
    }
}