using JxBackendService.Model.Entity.Base;
using System.ComponentModel.DataAnnotations;

namespace JxBackendService.Model.BackSideWeb
{
    public class SettingInfoContext 
    {
        public int Id { get; set; }
        /// <summary>
        /// 設定組別編號(ex: 新增投注筆數_小、新增投注金額_小)
        /// </summary>

        public int SettingGroupId { get; set; }
        /// <summary>
        /// 組別編號(ex: A、B、C)
        /// </summary>

        public int GroupId { get; set; }
        /// <summary>
        /// 時間類型(ex: T1、T2、T3)
        /// </summary>

        public int TimeType { get; set; }
        /// <summary>
        /// 值
        /// </summary>

        public int Amount { get; set; }
        /// <summary>
        /// 權重
        /// </summary>

        public int Rate { get; set; }
    }
}
