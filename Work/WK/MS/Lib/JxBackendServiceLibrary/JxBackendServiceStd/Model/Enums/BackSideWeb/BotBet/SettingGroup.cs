using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JxBackendService.Model.Enums.BackSideWeb.BotBet
{
    /// <summary>
    /// 設定的分組
    /// </summary>
    public enum SettingGroup
    {
        /// <summary>
        /// 新增投注筆數_小
        /// </summary>
        [Description("投注笔数-小")]
        SmallCount = 0,

        /// <summary>
        /// 新增投注金額_小
        /// </summary>
        [Description("投注金额-小")]
        SmallAmount = 1,

        /// <summary>
        /// 新增投注筆數_大
        /// </summary>
        [Description("投注笔数-大")]
        BigCount = 2,

        /// <summary>
        /// 新增投注金額_大
        /// </summary>
        [Description("投注金额-大")]
        BigAmount = 3
    }
}
