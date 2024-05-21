using JxBackendService.Model.Common;
using JxBackendService.Common.Util;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models
{
    public class DropdownMenuSetting
    {
        public DropdownMenuSetting()
        {
        }

        public DropdownMenuSetting(List<JxBackendSelectListItem>? selectListItems)
        {
            Items = selectListItems?.CastByJson<List<SelectListItem>>() ?? new List<SelectListItem>();
        }

        public SelectListItem? SelectedItem => Items?.Where(x => x.Selected).SingleOrDefault();

        public IEnumerable<SelectListItem>? Items { get; set; }

        /// <summary>
        /// 執行傳入參數select value的js函式名稱
        /// </summary>
        public string? Callback { get; set; }

        /// <summary>
        /// 用於在同個頁面裡有複數分頁區塊時以此ID做區分
        /// </summary>
        public string? SettingId { get; set; }
    }
}